using Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.DTO
{
    public class SalePostSendRequest : PostSendRequest
    {
        [Required]
        public PostType PostType { get; set; }

        public decimal Price { get; set; }

        [Required]
        public int CategoryID { get; set; }

        public static SalePostSendRequest ToSalePostSendRequest(SalePost p)
        {

            SalePostSendRequest s = new SalePostSendRequest (ToPostSendRequest(p))
            {
                PostType = p.PostType,
                Price = p.Price,
                CategoryID = p.CategoryID
            };
            
            return s;
        }

        public SalePostSendRequest(PostSendRequest p)
        {
            PostID = p.PostID;
            AuthorID = p.AuthorID;
            ContentText = p.ContentText;
            Images = p.Images;
            Author = p.Author;
            PostTime = p.PostTime;
        }
    }
}
