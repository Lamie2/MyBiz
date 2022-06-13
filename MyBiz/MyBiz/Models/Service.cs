using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBiz.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(300)]
        public string Desc { get; set; }
        [MaxLength(50)]
        public string Icon { get; set; }
    }
}
