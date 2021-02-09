using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe.Classes;

namespace TTTTest
{
    [TestClass]
    public class InviteManagerTest
    {
        [TestMethod]
        public void AddInvitation_Test()
        {
            InviteManager i = new InviteManager();
            i.AddInvitation<GameInvitation>("i1", "r1");//add 1 a
            i.AddInvitation<ChatInvitation>("i1", "r1");//add 1 b
            i.AddInvitation<ChatInvitation>("i1", "r1");//idempotence test

            Assert.AreEqual(2, i.Invitations.Count);

            i.AddInvitation<GameInvitation>("i2","r2");//add 2 a

            Assert.AreEqual(3, i.Invitations.Count);

            i.RemoveInvitation<GameInvitation>("i1", "r1");//remove 1 a

            bool res = i.IsInvitationSent<GameInvitation>("i1", "r1");
            bool res2 = i.IsInvitationSent<ChatInvitation>("i1", "r1");

            Assert.IsFalse(res);
            Assert.IsTrue(res2);

        }
    }
}
