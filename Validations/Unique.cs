using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using App.Models;

namespace App.Validations
{
    public class Unique : ValidationAttribute
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public Unique(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            ErrorMessage = "{0} is already taken.";
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                string userName = value.ToString();

                var _user = _userManager.FindByNameAsync(userName).GetAwaiter().GetResult();
                if (_user != null) //Username is already taken.
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}