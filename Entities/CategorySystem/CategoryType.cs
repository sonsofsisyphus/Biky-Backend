using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.CategorySystem
{
    public abstract class CategoryType
    {
        [Key]
        public Guid CategoryID { get; set; }
        
        [ForeignKey("CategoryCollection")]
        public Guid ParentID { get; set; }
        
        public virtual CategoryCollection Parent { get; set; }

        public string Name { get; set; }
    }
}