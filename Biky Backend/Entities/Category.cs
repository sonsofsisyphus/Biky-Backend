using System.ComponentModel.DataAnnotations;

namespace Entities {
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        public int? ParentID { get; set; }

        public string Name { get; set; }

    }
}