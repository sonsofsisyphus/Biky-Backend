using System.ComponentModel.DataAnnotations;

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
