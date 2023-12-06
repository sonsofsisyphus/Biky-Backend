using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class ImageCollection
    {
        
        [Key]
        public Guid CollectionID { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
    }
}