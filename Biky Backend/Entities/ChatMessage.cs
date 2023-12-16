using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class ChatMessage
    {
        [Key]
        public Guid MessageID { get; set; }

        [ForeignKey("User")]
        public Guid SenderID { get; set; }

        [ForeignKey("User")]
        public Guid ReceiverID { get; set; }

        public string Content { get; set; }

        public DateTime DateTime { get; set; }

    }
}