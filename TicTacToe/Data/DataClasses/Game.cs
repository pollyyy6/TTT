using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TicTacToe.Data
{
    [Table("Games")]
    public class Game
    {
        [Key]
        public Int32 Id { get; set; }
        [ForeignKey("User1Id")]
        public IdentityUser User1 { get; set; }
        public String User1Id { get; set; }
        public IdentityUser User2 { get; set; }
        [ForeignKey("User2Id")]
        public String User2Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Finish { get; set; }


        public ICollection<Step> Steps { get; set; }

        public Game()
        {
            Steps = new HashSet<Step>();
        }
    }

}
