using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicTacToe.Classes;
using TicTacToe.Data;
using TicTacToe.Models;
using SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading;

namespace TicTacToe.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		TTTModel game = new TTTModel();
		private readonly InviteManager _im;
		

		private readonly UserManager<IdentityUser> _um;
		private readonly ApplicationDbContext _db;

		private readonly SignalRMessenger _Msgr;
		

		public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> um, ApplicationDbContext db, InviteManager im, SignalRMessenger srm)
		{
			_logger = logger;
			_um = um;
			_db = db;
			_im = im;
			_Msgr = srm;
		}

		public IActionResult PreGame()
		{
			return View();
		}

		public async Task<string> CancelInvite(string email)
		{
			IdentityUser iu = await _um.GetUserAsync(User);
			IdentityUser ru = await _um.FindByEmailAsync(email);

			_im.RemoveInvitation<GameInvitation>(iu.Id, ru.Id);
			await _Msgr.UpdateForm(ru);
			await _Msgr.UpdateForm(iu);
			return "OK";
		}


		public async Task<string> RejectInvitation(string email)
		{
			IdentityUser iu = await _um.FindByEmailAsync(email); 
			IdentityUser ru = await _um.GetUserAsync(User);

			_im.RejectInvitation<GameInvitation>(iu.Id, ru.Id);
			await _Msgr.UpdateForm(ru);
			await _Msgr.UpdateForm(iu);
			return "OK";
		}

		public async Task<String> AcceptInvitation(string email)
		{
			IdentityUser ui = await _um.FindByEmailAsync(email);
			IdentityUser ur = await _um.GetUserAsync(User);

			int? res = _db.HasActiveGames(ui.Id, ur.Id);

			if (res==null)
			{
				Game g = _db.StartGame(ui.Id, ur.Id);
				await _Msgr.GoGame(ur,g.Id);
				await _Msgr.GoGame(ui, g.Id);
				return g.Id.ToString();
			}
			else
			{
				throw new Exception("active games already has! at homecontroller.AcceptInvitation");
			}
		}

		public IActionResult ActiveUsers()
		{
			return PartialView();
		}

		/// <summary>
		/// initiate invitation send to user
		/// </summary>
		/// <param name="email">email of recipient</param>
		/// <returns></returns>
		public async Task<String> SendInvite(String email)
		{
			IdentityUser recipient = await _um.FindByEmailAsync(email);
			IdentityUser initiator = await _um.GetUserAsync(User);

			bool res = _im.AddInvitation<GameInvitation>(initiator.Id, recipient.Id);
			if (res == true)
			{
				await _Msgr.SendInvite(initiator, recipient);
				await _Msgr.UpdateForm(recipient);
				await _Msgr.UpdateForm(initiator);
				return "invitation to " + email + " sent";
			}
			else
			{
				return "error - invitation to " + email + " already sent";
			}
		}

		private async Task initGame(int GameId)
		{
			IPrincipal u = User;
			IdentityUser u1 = await _um.GetUserAsync(User);

			Game g = _db.GetActiveGame(GameId, u1.Id);
			if (g == null)
			{
				//_logger.LogCritical("hack attempt, UserId = " + u1.Id);
			}
			else
			{
				List<Step> stl = _db.GetGameSteps(g.Id);
				
				ViewData["GameId"] = g.Id;

				game.Reset();
				game.GameId = g.Id;

				if (stl.Count > 0 && stl.Last().UserId == u1.Id)
				{
					game.HasTurn = false;
				}
				else
				{
					game.HasTurn = true;
				}

				game.Init(stl.ToList<IStep>());

				String res = game.WhoWin();
				if (res != null)
				{
					_db.RegisterWin(g.Id);
					game.HasTurn = false;
					_im.RemoveInvitation<GameInvitation>(g.User1Id, g.User2Id);
				}
			}
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult test()
		{
			return View();
		}

		[Authorize]
		public IActionResult Game(int? GameId)
		{
			ViewData["GameId"] = GameId;
			return View();
		}

		[Authorize]
		public async Task<IActionResult> TTT(int GameId)
		{
			await initGame(GameId);
			return PartialView(game);
		}

		[Authorize]
		[HttpPost]
		public async Task<string> Go(int x, int y, int GameId)
		{
			IPrincipal u = User;
			IdentityUser u1 = await _um.GetUserAsync(User);

			Game g = _db.GetActiveGame(GameId, u1.Id);
			if (g != null)
			{
				if (_db.AddStep(g.Id, u1.Id, x, y))
				{
					await _Msgr.UpdateGame(g.User1Id);
					await _Msgr.UpdateGame(g.User2Id);
					return "OK";
				}
			}
			return "ERROR";
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
