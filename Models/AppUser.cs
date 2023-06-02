using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace App.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(255)]
        [DisplayName("Full name")]
        [DataType(DataType.Text)]
        public string? FullName {get; set;}

        [DisplayName("Birthday")]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime CreateTime { get; set; }
    }
}