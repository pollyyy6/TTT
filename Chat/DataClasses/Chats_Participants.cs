using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chat.DataClasses
{
    [Table("Chats_Participants", Schema = "Chat")]
    public class Chats_Participants
    {
        [Key]
        public UInt32 Id { get; set; }
        public string UserId { get; set; }
        public DateTime AddTime { get; set; }
        public int ChatId { get; set; }
        [ForeignKey("ChatId")]
        public GroupChat Chat { get; set; }
        public int Status { get; set; }
        public string InvitatorId { get; set; }

        public Chats_Participants()
        {
            if (this.Id == 0)
            {
                this.Id = GenId();
            }
        }

        private static uint GenId()
        {
            return (uint)DateTime.Now.Ticks;
        }
    }
}
