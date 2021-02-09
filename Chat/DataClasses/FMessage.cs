using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chat.DataClasses
{
    [Table("ForwardedMessages",Schema = "Chat")]
    public class FMessage
    {
        [Key]
        public Int64 Id { get; set; }

        public Int64 ForwardedMessageId { get; set; }
        [ForeignKey("ForwardedMessageId")]
        public virtual ChatMessage ForwardedMessage { get; set; }
        public Int64 ChatMessageId { get; set; }
    }
}
