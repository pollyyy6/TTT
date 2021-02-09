using Chat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Extensions;
using System;
using Chat.DataClasses;
using System.Linq;

namespace DBTest
{
    [TestClass]
    public class DBChatTest:DBTestBase
    {
        [TestMethod]
        public void A0_empty()
        {
        }
        [TestMethod]
        public void A1_CreateChatGroup()
        {
            GroupChat gc =  db.AddChat("testchat1", "abc1");
            Assert.AreEqual(1, gc.Id);
            Assert.AreEqual(1, gc.Participants.Count);
        }

        [TestMethod]
        public void A2_AddUserToChat()
        {
            GroupChat g = db.Chats.Single();
            GroupChat g1 = db.AddParticipant(g.Id, "u2", g.Participants.Single().UserId);

            Assert.AreEqual(2, g1.Participants.Count);
        }
    }
}
