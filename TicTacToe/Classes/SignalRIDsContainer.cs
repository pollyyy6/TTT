using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Classes
{
    public class GameSRIDS : SignalRIDsContainer
    {
    }

    public class MesSRIDS : SignalRIDsContainer
    {
    }

    //ToDo: make abstract, derive from and specify in DI   
    public abstract class SignalRIDsContainer
    {
        public Dictionary<String, SignalRUser> Users = new Dictionary<string, SignalRUser>();

        public IReadOnlyList<String> GetUserConnections(String UserId)
        {
            IReadOnlyList<String> tl = this.Users.Where(x => x.Key == UserId).SingleOrDefault().Value?.Connections;
            return tl;
        }

        public void AddConnection(string UserId, String ConnectionId, String email = "", String GroupName=null)
        {
            KeyValuePair<String, SignalRUser> tuser = this.Users.Where(x => x.Key == UserId).SingleOrDefault();
            if (tuser.Value != null)
            {
                tuser.Value.AddConnection(ConnectionId);
            }
            else
            {
                SignalRUser nu = new SignalRUser();
                nu.UserId = UserId;
                nu.Email = email;
                nu.GroupName = GroupName;
                nu.AddConnection(ConnectionId);
                Users.Add(UserId, nu);
            }
        }

        public void RemoveConnection(string UserId, String ConnectionId)
        {
            KeyValuePair<String, SignalRUser> tuser = this.Users.Where(x => x.Key == UserId).SingleOrDefault();
            if (tuser.Value != null)
            {
                tuser.Value.RemoveConnection(ConnectionId);
                if (tuser.Value.Connections.Count == 0)
                {
                    Users.Remove(UserId);
                }
            }
        }
    }

    public class SignalRUser
    {
        public string UserId { get; set; }
        public String Email { get; set; }
        public String GroupName { get; set; }
        public List<String> Connections = new List<string>();

        public void AddConnection(String connection)
        {
            if (!Connections.Contains(connection))
            {
                Connections.Add(connection);
            }
        }

        public void RemoveConnection(String connection)
        {
            if (Connections.Contains(connection))
            {
                Connections.Remove(connection);
            }
        }
    }
}
