using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chat.DataClasses
{
    [Table("ChatMessages", Schema ="Chat")]
    public class ChatMessage
    {
        [Key]
        public Int64 Id { get; set; }
        public string MessageText { get; set; }
        public DateTime SentTime { get; set; }
        public String SenderId { get; set; }
        public int State { get; set; }
        public int ChatId { get; set; }
        [ForeignKey("ChatId")]
        public GroupChat Chat { get; set; }
        [InverseProperty("ForwardedMessage")]
        public virtual HashSet<FMessage> ForwardedMessages { get; set; }
        //ToDo: что делать если переслал сообщение

    }
}
