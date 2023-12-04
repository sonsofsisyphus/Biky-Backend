using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Comment
    {
        [Required]
        public Guid CommentID { get; set; }

        [Required]
        public Guid AuthorID { get; set; }

        [Required]
        public Guid PostID { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime PostTime { get; set; }
    }
}
