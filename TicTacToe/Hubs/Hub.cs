using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Classes;

namespace SignalR.Hubs
{
	[Authorize]
	public class Hubs : Hub
	{
		UserManager<IdentityUser> _um;
		GameSRIDS _SRC;
		public Hubs(UserManager<IdentityUser> um, GameSRIDS SRC)
		{
			_um = um;
			_SRC = SRC;
		}

		public async Task SendMessage(String userId, String message)
		{
			await Clients.All.SendAsync("ReceiveMessage", userId, message);
		}

		public async override Task OnConnectedAsync()
		{
			IdentityUser user = await _um.GetUserAsync(Context.User);
			String cid = Context.ConnectionId;
			lock (_SRC)
			{
				_SRC.AddConnection(user.Id, cid, user.Email);
			}

			await Clients.All.SendAsync("UserAdded", user.Email);
			await base.OnConnectedAsync();
		}

		public async override Task OnDisconnectedAsync(Exception exception)
		{
			IdentityUser user = await _um.GetUserAsync(Context.User);
			String cid = Context.ConnectionId;

			lock (_SRC)
			{
				_SRC.RemoveConnection(user.Id, cid);
			}
			await Clients.All.SendAsync("UserRemoved", user.Email);
			await base.OnDisconnectedAsync(exception);
		}
	}
}
