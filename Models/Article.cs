using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RAZOR_EF.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName="nvarchar")]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedTime { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Content{ get; set; }
    }
}