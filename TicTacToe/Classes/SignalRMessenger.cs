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
	public class SignalRMessenger
	{
		private readonly IHubContext<Hubs> _h;
		private readonly GameSRIDS _SRC;
		public SignalRMessenger(IHubContext<Hubs> hub, GameSRIDS SRC)
		{
			_h = hub;
			_SRC = SRC;
		}

		public async Task SendInvite(IdentityUser Initiator, IdentityUser Recipient)
		{
			IReadOnlyList<String> SRIds = _SRC.GetUserConnections(Recipient.Id);
			await _h.Clients.Clients(SRIds).SendAsync("ReceiveInvite", Initiator.Email);
		}

		public async Task UpdateForm(IdentityUser user)
		{
			IReadOnlyList<String> SRIds = _SRC.GetUserConnections(user.Id);
			await _h.Clients.Clients(SRIds).SendAsync("UpdateForm");
		}

		public async Task GoGame(IdentityUser user, int GameId)
		{
			IReadOnlyList<String> SRIDs = _SRC.GetUserConnections(user.Id);
			await _h.Clients.Clients(SRIDs).SendAsync("GoGame", GameId);
		}

		public async Task UpdateGame(String userid)
		{
			IReadOnlyList<String> SRIDs = _SRC.GetUserConnections(userid);
			await _h.Clients.Clients(SRIDs).SendAsync("UpdateGame");
		}
	}
}
