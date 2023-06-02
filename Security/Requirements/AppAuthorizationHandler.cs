using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using RAZOR_EF.Models;

namespace RAZOR_EF.Security.Requirements
{
    public class AppAuthorizationHandler : IAuthorizationHandler
    {
        private readonly ILogger<AppAuthorizationHandler> _logger;
        private readonly UserManager<AppUser> _userManager;
        public AppAuthorizationHandler(ILogger<AppAuthorizationHandler> logger, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var requirements = context.PendingRequirements.ToList();
            foreach (var requirement in requirements)
            {
                if (requirement is GenZRequirement)
                {
                    if (IsGenZ(context.User, (GenZRequirement)requirement))
                        context.Succeed(requirement);
                }
                if (requirement is UpdateArticleRequirement)
                {
                    if (IsFromYear(context.User, context.Resource, (UpdateArticleRequirement)requirement))
                        context.Succeed(requirement);
                }
            }


            return Task.CompletedTask;
        }

        private bool IsFromYear(ClaimsPrincipal user, object resource, UpdateArticleRequirement requirement)
        {
            if (user.IsInRole("Admin")) 
            {
                _logger.LogWarning($"Admin updated");
                return true;
            }
            var article = (Article)resource;
            var title = $"{article.Title.Substring(0, 10)}...";
            var createdYear = article.CreatedTime.Year;
            if (createdYear >= requirement.FromYear)
            {
                _logger.LogWarning($"Article '{title}' was created in {createdYear} => Can update");
                return true;
            }
            else
            {
                _logger.LogError($"Article '{title}' was created in {createdYear} => Cannot update");
                return false;
            }
        }

        private bool IsGenZ(ClaimsPrincipal _user, GenZRequirement requirement)
        {
            var task = _userManager.GetUserAsync(_user);
            Task.WaitAll(task);
            var user = task.Result;

            if (user.Birthday == null) 
            {
                _logger.LogError($"Birthday of '{user.UserName}' is null");
                return false;
            }
            int birthYear = user.Birthday.Value.Year;
           
            var result = (birthYear >= requirement.FromYear && birthYear <= requirement.ToYear);
            if (result)
                _logger.LogWarning($"{user.UserName} is GenZ. Birthday: {birthYear}");
            else
                _logger.LogError($"{user.UserName} isn't GenZ. Birthday: {birthYear}");
            return result;
        }
    }
}