using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace App.Security.Requirements
{
    public class GenZRequirement : IAuthorizationRequirement
    {
        public int FromYear {get; set;}
        
        public int ToYear {get; set;}
        public GenZRequirement(int fromYear = 2000, int toYear = 2005)
        {
            FromYear = fromYear;
            ToYear = toYear;
        }
    }
}