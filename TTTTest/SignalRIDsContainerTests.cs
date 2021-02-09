using Microsoft.Extensions.DependencyModel.Resolution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TicTacToe.Classes;

namespace TTTTest
{
    [TestClass]
    public class SignalRIDsContainerTests
    {
        [TestMethod]
        public void T1_AddUserConnections()
        {
            GameSRIDS c = new GameSRIDS();
            c.AddConnection("a", "c1");
            c.AddConnection("b", "c2");

            IReadOnlyCollection<string> ca = c.GetUserConnections("a");
            IReadOnlyCollection<string> cb = c.GetUserConnections("b");

            Assert.AreEqual(1, ca.Count);
            Assert.AreEqual("c1", ca.First());

            Assert.AreEqual(1, cb.Count);
            Assert.AreEqual("c2", cb.First());

            c.AddConnection("b", "c3");
            IReadOnlyCollection<string> cd = c.GetUserConnections("b");

            Assert.AreEqual(2, cd.Count);
            Assert.AreEqual("c2", cd.ElementAt(0));
            Assert.AreEqual("c3", cd.ElementAt(1));

            c.RemoveConnection("b", "c2");
            IReadOnlyCollection<string> cc = c.GetUserConnections("b");

            Assert.AreEqual(1, cc.Count);
            Assert.AreEqual("c3", cc.Single());

            c.RemoveConnection("b", "c3");
            IReadOnlyCollection<string> cs = c.GetUserConnections("b");
            Assert.IsNull(cs);
        }
    }
}
