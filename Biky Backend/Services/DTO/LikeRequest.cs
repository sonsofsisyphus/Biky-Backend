using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities;

namespace Biky_Backend.Services.DTO
{
    public class LikeRequest
    {
        [Required]
        public Guid UserID { get; set; }

        [Required]
        public Guid PostID { get; set; }

        public Like ToLike()
        {
            return new Like()
            {
                UserID = UserID,
                PostID = PostID,
                LikeID = Guid.NewGuid()
            };
        }
    }
}
