using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class SalePost: Post
    {
        [Required]
        public PostType PostType { get; set; }

        [Required]
        public string Price { get; set; }

        [Required]
        public Category Category { get; set; }
    }
}
