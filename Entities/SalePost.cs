using System.ComponentModel.DataAnnotations.Schema;
using Entities.CategorySystem;

namespace Entities
{
    public enum PostType
    {
        BORROW_POST,
        LOST_FOUND_POST,
        LESSON_POST,
        SECOND_HAND_POST,
        TRADE_POST
    }
    
    public class SalePost : Post
    {
        public PostType PostType { get; set; }

        public string Price { get; set; }

        [ForeignKey("Category")]
        public Guid CategoryID { get; set; }

        public virtual Category Category { get; set; }
    }
}
