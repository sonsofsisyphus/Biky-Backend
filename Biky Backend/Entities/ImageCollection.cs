using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class ImageCollection
    {
        
        [Key]
        public Guid CollectionID { get; set; }
        public string Image { get; set; }

        [ForeignKey("Post")]
        public Guid PostID { get; set; }

        public virtual Post Post { get; set; }

        public ImageCollection(string image, Guid postID)
        {
            CollectionID = Guid.NewGuid();
            Image = image;
            PostID = postID;
        }

    }
}