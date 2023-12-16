using System.ComponentModel.DataAnnotations.Schema;

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

        public decimal Price { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }
    }
}
