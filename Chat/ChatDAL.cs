using Chat.DataClasses;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chat
{
    public static class ChatDAL
    {
        public static GroupChat AddChat(this ChatDBContext db, String ChatName, String CreatorId)
        {
            GroupChat gc = new GroupChat();
            gc.ChatName = ChatName;
            gc.CreateTime = DateTime.Now;

            Chats_Participants cp = new Chats_Participants();
            cp.AddTime = gc.CreateTime;
            cp.UserId = CreatorId;

            gc.Participants.Add(cp);

            db.Chats.Add(gc);

            int temp = db.SaveChanges();
            if (temp == 2)
            {
                return gc;
            }
            else
            {
                return null;
            }
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public static List<ChatMessage> GetChatMessages(this ChatDBContext db, int ChatId, int take = 50, int offset=0)
        {
            //             SELECT * FROM ChatMessages WHERE ChatId=5 Take ORDER BY Id Descending                                
            List<ChatMessage> temp = db.ChatMessages.OrderByDescending(x => x.Id).Where(x => x.ChatId == ChatId).Skip(offset).Take(take).ToList();
            return temp;
        }

        public static GroupChat AddParticipant(this ChatDBContext db, int ChatId, string ParticipantId, string InvitatorId)
        {
            Chats_Participants cp = db.Chats_Participants.Where(x => x.ChatId == ChatId && x.UserId == ParticipantId).SingleOrDefault();
            if (cp == null)
            {
                cp = new Chats_Participants();
                cp.InvitatorId = InvitatorId;
                cp.UserId = ParticipantId;
                cp.ChatId = ChatId;
                cp.AddTime = DateTime.Now;

                db.Chats_Participants.Add(cp);
                int res = db.SaveChanges();

                if (res == 1)
                {
                    return cp.Chat;
                }
            }
            return null;
        }

        public static List<String> ChatUsers(this ChatDBContext db, int ChatId)
        {
            List<String> AllUsers = db.Chats_Participants.Where(x => x.ChatId == ChatId).Select(x =>x.UserId).ToList();
            return AllUsers;
        }

        public static ChatMessage AddMessage(this ChatDBContext db, int ChatId, string UserId, String message)
        {
            GroupChat ch = db.Chats.Where(x => x.Id==ChatId && x.Participants.Any(y => y.UserId == UserId && y.ChatId==ChatId)).Single();
            if (ch != null)
            {
                ChatMessage m = new ChatMessage();
                m.MessageText = message;
                m.SenderId = UserId;
                m.SentTime = DateTime.Now;

                ch.Messages.Add(m);
                db.SaveChanges();
                return m;
            }
            else
            {
                //ToDo: log hack attempt
                return null;
            }
        }

        public static List<uint> GetUserChatsNames(this ChatDBContext db, String UserId)
        {
            List<uint> temp = db.Chats_Participants.Where(x => x.UserId == UserId).Select(x => x.Id).ToList();
            return temp;
        }
	}

}
