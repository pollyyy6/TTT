using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TicTacToe.Classes;
using TicTacToe.Data;

namespace TicTacToe.Areas.Stat.Controllers
{
    [Area("Stat")]
    [Authorize]
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stat/Games
        public async Task<IActionResult> Index()
        {
            IIncludableQueryable<Game,IdentityUser> applicationDbContext = _context.Games.Include(g => g.User1).Include(g => g.User2);

            var sql = applicationDbContext.ToString();

            String temp = applicationDbContext.ToSql();

            
            List<Game> games = await applicationDbContext.ToListAsync();



            //games.ForEach(x =>
            //{
            //    GameModel gm = (GameModel)x;
            //    gm.User1Email = "em1";
            //    gm.User2Email = "em2";
            //    models.Add(gm);
            //});


            return View(games);
        }

        // GET: Stat/Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Game g = await _context.Games.Include(x => x.User1).Include(x => x.User2).Include(x => x.Steps).FirstAsync(x => x.Id == id);

            if (g == null)
            {
                return NotFound();
            }

            return View(g);
        }

        // GET: Stat/Games/Create
        public IActionResult Create()
        {
            ViewData["User1Id"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["User2Id"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Stat/Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,User1Id,User2Id,Start,Finish")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["User1Id"] = new SelectList(_context.Users, "Id", "Id", game.User1Id);
            ViewData["User2Id"] = new SelectList(_context.Users, "Id", "Id", game.User2Id);
            return View(game);
        }

        // GET: Stat/Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Game game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["User1Id"] = new SelectList(_context.Users, "Id", "Id", game.User1Id);
            ViewData["User2Id"] = new SelectList(_context.Users, "Id", "Id", game.User2Id);
            return View(game);
        }

        // POST: Stat/Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,User1Id,User2Id,Start,Finish")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["User1Id"] = new SelectList(_context.Users, "Id", "Id", game.User1Id);
            ViewData["User2Id"] = new SelectList(_context.Users, "Id", "Id", game.User2Id);
            return View(game);
        }

        // GET: Stat/Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.User1)
                .Include(g => g.User2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Stat/Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
