using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace TicTacToe.Classes
{
    /// <summary>
    /// класс который управляет приглашениями
    /// мы решили избавиться от работы с базой данных и потерять все приглашения при перезапуске.
    /// если приглашение принято, оно удаляется отсюда и идёт в базу
    /// </summary>
    public class InviteManager
    {
        public List<IInvitation> Invitations = new List<IInvitation>();

        /// <summary>
        /// add invitation record to invitation manager
        /// </summary>
        /// <param name="InitiatorId">who invites</param>
        /// <param name="RecipientId">who is invited</param>
        /// <returns>is invitation successful</returns>
        public bool AddInvitation<T>(string InitiatorId, string RecipientId) where T:IInvitation
        {
            //если(я.id == ты.id && я.кого == ты.кого || я.id == ты.кого && я.кого == ты.id)

            lock (Invitations)
            {
                IInvitation Il = this.Invitations.OfType<T>().Where(x =>
                x.InitiatorId == InitiatorId && x.RecipientId == RecipientId ||
                x.InitiatorId == RecipientId && x.RecipientId == InitiatorId).SingleOrDefault();

                if (Il != null)
                {
                    if (Il.Status == GameInvitation.StatusRejected)
                    {
                        int role = this.GetRole<T>(InitiatorId, RecipientId);
                        if (role == GameInvitation.RoleRecipient)
                        {
                            this.RemoveInvitation<T>(RecipientId, InitiatorId);
                            AddInvitation<T>(InitiatorId, RecipientId);
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    T ni = (T)Activator.CreateInstance(typeof(T), new object[] { InitiatorId, RecipientId });
                    Invitations.Add(ni);
                    return true;
                }
            }
        }

        /// <summary>
        /// определяет кто инициатор а кто акцептор приглашения по порядку следования полей в базе
        /// </summary>
        /// <param name="InitiatorId"></param>
        /// <param name="RecipientId"></param>
        /// <returns></returns>
        public int GetRole<T>(String InitiatorId, String RecipientId) where T:IInvitation
        {
            IInvitation Il = this.Invitations.OfType<T>().Where(x => x.InitiatorId == InitiatorId && x.RecipientId == RecipientId ||
           x.InitiatorId == RecipientId && x.RecipientId == InitiatorId).SingleOrDefault();
            if (Il != null)
            {
                if (Il.InitiatorId == InitiatorId)
                {
                    return GameInvitation.RoleInitiator;
                }
                else
                {
                    return GameInvitation.RoleRecipient;
                }
            }
            return 0;
        }

        public bool IsInvitationSent<T>(String InitiatorId, String RecipientId) where T:IInvitation
        {
            IInvitation Il = this.Invitations.OfType<T>().Where(x =>
               x.InitiatorId == InitiatorId && x.RecipientId == RecipientId ||
               x.InitiatorId == RecipientId && x.RecipientId == InitiatorId).SingleOrDefault();
            if (Il != null && Il.Status == GameInvitation.StatusPending)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsInvitationRejected<T>(String InitiatorId, String RecipientId) where T:IInvitation
        {
            IInvitation Il = this.Invitations.OfType<T>().Where(x => x.InitiatorId == InitiatorId && x.RecipientId == RecipientId).SingleOrDefault();
            if (Il != null && Il.Status == GameInvitation.StatusRejected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
        /// <summary>
        /// удаляет приглашение когда оно принято. дальше за это отвечает база данных
        /// </summary>
        /// <param name="InitiatorId"></param>
        /// <param name="RecipientId"></param>
        public void RemoveInvitation<T>(string InitiatorId, string RecipientId) where T:IInvitation
        {
            lock (Invitations)
            {
                IInvitation Il = this.Invitations.OfType<T>().Where(x => x.InitiatorId == InitiatorId && x.RecipientId == RecipientId).SingleOrDefault();
                if (Il != null)
                {
                    Invitations.Remove(Il);
                }
            }
        }

        /// <summary>
        /// при отклонении приглашения заново послать его нельзя пока не перезапустится серверs
        /// </summary>
        /// <param name="InitiatorId"></param>
        /// <param name="RecipientId"></param>
        public void RejectInvitation<T>(string InitiatorId, string RecipientId) where T:IInvitation
        {
            lock (Invitations)
            {
                IInvitation Il = this.Invitations.OfType<T>().Where(x => x.InitiatorId == InitiatorId && x.RecipientId == RecipientId).SingleOrDefault();
                if (Il != null)
                {
                    Il.Status = GameInvitation.StatusRejected;
                }
            }
        }

        public void AcceptInvitation<T>(string InitiatorId, string RecipientId) where T:IInvitation
        {
            lock (Invitations)
            {
                IInvitation Il = this.Invitations.OfType<T>().Where(x => x.InitiatorId == InitiatorId && x.RecipientId == RecipientId).SingleOrDefault();
                if (Il != null)
                {
                    Il.Status = GameInvitation.StatusAccepted;
                }
            }
        }
    }

    public interface IInvitation
    {
        public String InitiatorId { get; set; }
        public String RecipientId { get; set; }
        public DateTime InvitationTime { get; set; }
        public DateTime ReactionTime { get; set; }
        public int Status { get; set; }

        public static int StatusPending = 0;
        public static int StatusAccepted = 1;
        public static int StatusRejected = 2;

        public static int RoleInitiator = 1;
        public static int RoleRecipient = 2;

    }

    public abstract class Invitation:IInvitation
    {
        public Invitation()
        {

        }
        public Invitation(String InitiatorId, String RecipientId)
        {
            Status = StatusPending;
            InvitationTime = DateTime.Now;
            this.InitiatorId = InitiatorId;
            this.RecipientId = RecipientId;
        }

        public String InitiatorId { get; set; }
        public String RecipientId { get; set; }
        public DateTime InvitationTime { get; set; }
        public DateTime ReactionTime { get; set; }
        public int Status { get; set; }

        public static int StatusPending = 0;
        public static int StatusAccepted = 1;
        public static int StatusRejected = 2;

        public static int RoleInitiator = 1;
        public static int RoleRecipient = 2;
    }

    public class GameInvitation:Invitation
    {
        public GameInvitation(String InitiatorId, String RecipientId):base(InitiatorId,RecipientId)
        {
            
        }
    }

    public class ChatInvitation : Invitation
    {
        public ChatInvitation(String InitiatorId, String RecipientId) : base(InitiatorId, RecipientId)
        {

        }
    }
}
