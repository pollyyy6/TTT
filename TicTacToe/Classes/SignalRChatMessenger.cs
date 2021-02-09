using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Classes
{
	/// <summary>
	/// send messages to clients
	/// </summary>
	public class SignalRChatMessenger
	{
		private readonly IHubContext<MessageHub> _h;
		private readonly MesSRIDS _SRC;
		public SignalRChatMessenger(IHubContext<MessageHub> hub, MesSRIDS SRC)
		{
			_h = hub;
			_SRC = SRC;
		}

		public async Task SendChatInvite(IdentityUser Initiator, IdentityUser Recipient)
		{
			IReadOnlyList<String> SRIds = _SRC.GetUserConnections(Recipient.Id);
			await _h.Clients.Clients(SRIds).SendAsync("ReceiveChatInvite", Initiator.Email);
		}

		

		public async Task UpdateChat(String GroupName)
		{
			await _h.Clients.Group(GroupName).SendAsync("UpdateChat");
		}
	}
}
