using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Post
    {
        [Key]
        public Guid PostID { get; set; }

        [ForeignKey("Author")]
        public Guid AuthorID { get; set; }

        [ForeignKey("ImageCollection")]
        public Guid ImagesID { get; set; }

        public virtual User Author { get; set; }

        public virtual ImageCollection Images { get; set; }

        public string ContentText { get; set; }

        public DateTime PostTime { get; set; }
    }
}