using Entities;
using System.ComponentModel.DataAnnotations;

namespace Services.DTO
{
    public class SalePostAddRequest
    {
        [Required]
        public Guid AuthorID { get; set; }

        [Required]
        public string ContentText { get; set; }

        public List<String>? Images { get; set; }

        [Required]
        public PostType PostType { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Category { get; set; }
        
        public SalePost ToSalePost()
        {
            return new SalePost
            {
                PostID = Guid.NewGuid(),
                AuthorID = AuthorID,
                ContentText = ContentText,
                PostTime = DateTime.Now,
                PostType = PostType,
                Price = Price,
                CategoryID = Category
            };
        }
    }
}
