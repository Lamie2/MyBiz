using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyBiz.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(50)]
        public string Subtitle { get; set; }
        [MaxLength(100)]
        public string Image { get; set; }
        [MaxLength(100)]
        public string Icon { get; set; }
        [MaxLength(100)]
        public string DetailIcon { get; set; }
        [NotMapped]       
        public IFormFile ImageFile { get; set; }
    }
}
