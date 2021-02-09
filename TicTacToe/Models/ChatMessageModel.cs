using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Classes
{
    public class ChatMessageModel
    {
        public String UserId { get; set; }
        public DateTime MessageTime { get; set; }
        public String UserEmail { get; set; }
        public String MessageText { get; set; }
    }

    public class ChatData
    {
        public List<String> Users { get; set; } = new List<string>();
        public List<ChatMessageModel> Messages { get; set; } = new List<ChatMessageModel>();
        public int ChatId { get; set; }
    }

    public class ChatListElement
    {
        public int Id { get; set; }
        public String Name { get; set; }
    }
}
