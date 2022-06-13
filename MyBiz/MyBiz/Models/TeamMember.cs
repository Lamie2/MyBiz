using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyBiz.Models
{
    public class TeamMember
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string FullName { get; set; }
        [MaxLength(100)]
        public string Image { get; set; }
        [MaxLength(300)]
        public string Desc { get; set; }
        [MaxLength(100)]
        public string TwitterLink { get; set; }
        [MaxLength(100)]
        public string FaceBookLink { get; set; }
        [MaxLength(100)]
        public string InstaLink { get; set; }
        [MaxLength(100)]
        public string LinkednLink { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
    }
}
