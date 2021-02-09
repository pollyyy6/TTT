using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TicTacToe.Classes;

namespace TicTacToe.Data
{
    [Table("Steps")]
    public class Step:IStep
    {
        [Key]
        public Int64 Id { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
        public String UserId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public DateTime StepTime { get; set; }

        [ForeignKey("GameId")]
        public virtual Game game { get; set; }
        public Int32 GameId { get; set; }
    }

}
