using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RAZOR_EF.Security.Requirements
{
    public class UpdateArticleRequirement : IAuthorizationRequirement
    {
        public int FromYear { get; set; }
        public UpdateArticleRequirement(int fromYear = 2010)
        {
            FromYear = fromYear;
        }
    }
}