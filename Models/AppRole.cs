using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RAZOR_EF.Models
{
    public class AppRole : IdentityRole
    {
        [DataType(DataType.Text)]
        public string? Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreateTime { get; set; }
    }
}