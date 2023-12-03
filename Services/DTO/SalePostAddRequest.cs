using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class SalePostAddRequest
    {
        [Required]
        public Guid AuthorID { get; set; }

        [Required]
        public string ContentText { get; set; }

        public ImageCollection Images { get; set; }

        [Required]
        public PostType PostType { get; set; }

        [Required]
        public string Price { get; set; }

        [Required]
        public Category Category { get; set; }
        public SalePost ToSalePost()
        {
            return new SalePost
            {
                PostID = Guid.NewGuid(),
                AuthorID = AuthorID,
                ContentText = ContentText,
                PostTime = DateTime.Now,
                Images = Images,
                PostType = PostType,
                Price = Price,
                Category = Category
            };
        }
    }
}
