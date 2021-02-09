using Chat;
using Chat.DataClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Classes;
using TicTacToe.Data;

namespace TicTacToe.Controllers
{
    public class ChatController : Controller
    {
        ChatDBContext _cdb;
        ApplicationDbContext _db;
        public ChatController(ChatDBContext cdb, ApplicationDbContext db)
        {
            this._cdb = cdb;
            this._db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public String Update(String data)
        {
            String temp = Guid.NewGuid().ToString();
            String temp1 = String.Join("",temp.Take(5));
            String temp2 = String.Join("", temp.Skip(5).Take(5));
            return JsonConvert.SerializeObject(
                new[]
                {
                    new { user = "1234", text = "msg-"+temp1 },
                    new { user = "5678", text = "hihihi-"+temp2 }
                }
            );
        }

        [HttpPost]
        public String UpdateChats()
        {
            ChatListElement el = new ChatListElement();
            el.Id = 1;
            el.Name = "Chat1";

            ChatListElement el1 = new ChatListElement();
            el1.Id = 2;
            el1.Name = "Chat2";

            return JsonConvert.SerializeObject(new List<ChatListElement> { el, el1 });
        }

        [HttpPost]
        public String ChatMessages([FromForm]int id)
        {
            List<ChatMessage> msgs = _cdb.GetChatMessages(id);

            List<ChatMessageModel> rm = new List<ChatMessageModel>();

            List<String> ids = msgs.Select(x => x.SenderId).Distinct().ToList();

            List<IdentityUser> users = _db.Users.Where(x => ids.Contains(x.Id)).ToList();

            msgs.ForEach(x =>
            {
                ChatMessageModel cmm = new ChatMessageModel();

                cmm.UserId = x.SenderId;
                cmm.MessageText = x.MessageText;
                cmm.MessageTime = x.SentTime;
                //ToDo:  
                cmm.UserEmail = users.Where(y => y.Id == x.SenderId).Single().Email;

                rm.Add(cmm);
            });

            //O(n)

            return "";
        }

        public String NewChat(String data)
        {
            ChatListElement el = new ChatListElement();
            el.Id = 1;
            el.Name = "Chat1";

            ChatListElement el1 = new ChatListElement();
            el1.Id = 2;
            el1.Name = "Chat2";

            ChatListElement el2 = new ChatListElement();
            el1.Id = 3;
            el1.Name = data;

            return JsonConvert.SerializeObject(new List<ChatListElement> { el, el1,el2 });
        }

        public String UserMessage(String data)
        {
            data = data.Replace("'", "''");
            
            String temp = Guid.NewGuid().ToString();
            String temp1 = String.Join("", temp.Take(5));
            String temp2 = String.Join("", temp.Skip(5).Take(5));

            ChatData d = new ChatData();
            d.Users.Add("user1@gmail.com");
            d.Users.Add("bbb@gmail.com");

            ChatMessageModel cm = new ChatMessageModel();
            cm.MessageText = data + temp1;
            cm.UserEmail = "1234";

            ChatMessageModel cm2 = new ChatMessageModel();
            cm2.MessageText = data + temp2;
            cm2.UserEmail = "5678";

            d.Messages.AddRange(new []{ cm, cm2 });

            return JsonConvert.SerializeObject(d);
        }
    }
}
