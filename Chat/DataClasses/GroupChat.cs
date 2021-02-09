using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chat.DataClasses
{
    [Table("Chats", Schema = "Chat")]
    public class GroupChat
    {
        [Key]
        public int Id { get; set; }
        public string ChatName { get; set; }
        public DateTime CreateTime { get; set; }
        public HashSet<ChatMessage> Messages { get; set; }
        public HashSet<Chats_Participants> Participants { get; set; }

        public GroupChat()
        {
            Messages = new HashSet<ChatMessage>();
            Participants = new HashSet<Chats_Participants>();
        }
    }
}
