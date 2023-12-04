using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class CommentEditRequest
    {
        [Required]
        public Guid CommentID { get; set; }

        [Required]
        public string Content { get; set; } 
      
    }
}
