﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Classes
{
    public interface IStep
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
