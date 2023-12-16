using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Entities;

namespace Biky_Backend.Entities
{
    public class Chat
    {
        [Key]
        public Guid ChatID { get; set; }

        [ForeignKey("User")]
        public Guid SenderID { get; set; }

        [ForeignKey("User")]
        public Guid ReceiverID { get; set; }

        // Navigation properties
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }
}
