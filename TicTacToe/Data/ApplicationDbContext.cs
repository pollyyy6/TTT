using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Transactions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace TicTacToe.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Step> Steps { get; set; }
        public DbSet<Game> Games { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public bool AddStep(Int32 GameId, String UserId, int X, int Y)
        {
            if (this.Steps.Where(x=>x.GameId==GameId).Count() < 9)
            {
                Step s = new Step();
                s.StepTime = DateTime.Now;
                s.GameId = GameId;
                s.UserId = UserId;
                s.X = X;
                s.Y = Y;
                this.Steps.Add(s);
                int res = this.SaveChanges();
                if (res == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public Game StartGame(String User1, String User2)
        {
            Game g = new Game();
            g.User1Id = User1;
            g.User2Id = User2;
            g.Start = DateTime.Now;
            this.Games.Add(g);

            int res = this.SaveChanges();
            if (res == 1)
            {
                return g;
            }
            else
            {
                throw new Exception("Error saving new game: ApplicationDBCintext.StartGame");
            }
        }

       // Expression<Func<Game,String,String,bool>> exp = (x, UserId1, UserId2) => ((x.User1Id == UserId1 && x.User2Id == UserId2) || (x.User1Id == UserId2 && x.User2Id == UserId1)) && x.Finish == null;

        public int? HasActiveGames(string UserId1, string UserId2)
        {
            Game g = GetActiveGame(UserId1, UserId2);
            //if error see thread safe
            if (g != null)
            {
                return g.Id;
            }
            else
            {
                return null;
            }
        }

        public Game GetActiveGame(int GameId, string UserId)
        {
            //где id равен GameId и Id одного из пользоваталей равен UserId
            Game g = this.Games.Where(x => x.Id==GameId && (x.User1Id==UserId || x.User2Id==UserId)).AsNoTracking().SingleOrDefault();
            return g;
        }

        public Game GetActiveGame(string UserId1, string UserId2)
        {
            Game g = this.Games.Where(x => ((x.User1Id == UserId1 && x.User2Id == UserId2) || (x.User1Id == UserId2 && x.User2Id == UserId1)) && x.Finish == null).SingleOrDefault();
            return g;
        }

        public List<Step> GetGameSteps(int gameId)
        {
            List<Step> Steps = this.Games.Where(x => x.Id == gameId).Include(x => x.Steps).Single().Steps.ToList();
            return Steps;
        }

        public Game RegisterWin(int GameId)
        {
            Game g = this.Games.Where(x => x.Id == GameId).Single();
            if (g.Finish == null)
            {
                g.Finish = DateTime.Now;
                this.SaveChanges();
            }
            return g;
        }
    }
}
