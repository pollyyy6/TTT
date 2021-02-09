using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TicTacToe.Classes;

namespace TicTacToe.Models
{
    public class TTTModel
    {
        #region DataModel

        public bool HasTurn = false;
        public int GameId { get; set; }
        public String Winner = null;

        #endregion

        /*

        1      2       3       4       5       6       7       8       9
        1,1    1,2     1,3     2,1     2,2     2,3     3,1     3,2     3,3

        */

        Pos[] pcodes = new Pos[9]
        {
            new Pos(1,1),
            new Pos(1,2),
            new Pos(1,3),
            new Pos(2,1),
            new Pos(2,2),
            new Pos(2,3),
            new Pos(3,1),
            new Pos(3,2),
            new Pos(3,3)
        };

        int[][] vars = new int[8][]
            {
                new int[]{1,2,3 },
                new int[]{4,5,6 },
                new int[]{7,8,9 },
                new int[]{1,4,7 },
                new int[]{2,5,8 },
                new int[]{3,6,9 },
                new int[]{1,5,9 },
                new int[]{3,5,7 },
            };

        public int counter = 0;
        public TTTRow[] rows = new TTTRow[3]
            {
                new TTTRow(1),
                new TTTRow(2),
                new TTTRow(3)
            };

        public void Reset()
        {
            counter = 0;
            rows.ToList().ForEach(y =>
            {
                y.cells.ToList().ForEach(x =>
                {
                    x.State = TTTCell.StateEmpty;
                });
            });
        }

        public String WhoWin()
        {
            for (int i = 0; i < vars.Length; i++)
            {
                String res = RowSame(vars[i]);
                if (res != null && res != TTTCell.StateEmpty)
                {
                    Winner = res;
                    return res;
                }
            }
            if (counter == 9)
            {
                return TTTCell.StateEmpty;
            }
            else
            {
                return null;
            }
        }

        public String RowSame(int[] vars)
        {
            return RowSame(vars[0], vars[1], vars[2]);
        }

        public String RowSame(int p1, int p2, int p3)
        {
            String temp = GetCellState(p1);
            if (temp == GetCellState(p2) && GetCellState(p2) == GetCellState(p3))
            {
                return temp;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">1-9</param>
        /// <returns></returns>
        public String GetCellState(int code)
        {
            Pos p = pcodes[code - 1];
            String temp = GetCellState(p.x, p.y);
            return temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">1-3</param>
        /// <param name="y">1-3</param>
        /// <returns></returns>
        public String GetCellState(int x, int y)
        {
            return this.rows[y - 1].cells[x - 1].State;
        }

        public void NextStep()
        {
            counter++;
        }

        public String RenderCell(int x, int y)
        {
            String temp = this.rows[y - 1].cells[x - 1].Render();
            return temp;
        }

        public void Init(List<IStep> stl)
        {
            stl.ForEach(x =>
            {
                if (this.counter % 2 == 0)
                {
                    this.SetX(x.X, x.Y);
                }
                else
                {
                    this.SetO(x.X, x.Y);
                }
                this.NextStep();
            });
        }

        /// <summary>
        /// sets game value X
        /// </summary>
        /// <param name="x">number of cell, 1-3</param>
        /// <param name="y">number of cell, 1-3</param>
        public void SetX(int x, int y)
        {
            this.rows[y - 1].cells[x - 1].State = TTTCell.StateX;
        }



        public void SetO(int x, int y)
        {
            this.rows[y - 1].cells[x - 1].State = TTTCell.StateO;
        }
    }

    public class TTTRow
    {
        public TTTCell[] cells = new TTTCell[3]
        {
            new TTTCell(1),
            new TTTCell(2),
            new TTTCell(3)
        };
        public int num { get; set; }

        public TTTRow(int num)
        {
            this.num = num;
        }
    }

    public class Pos
    {
        public Pos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x { get; set; }
        public int y { get; set; }
    }


    public class TTTCell
    {
        public String State { get; set; }

        public static String StateX = "X";
        public static String StateO = "O";
        public static String StateEmpty = "-";

        public int num { get; set; }

        public TTTCell(int num)
        {
            this.num = num;
            this.State = StateEmpty;
        }

        public string Render()
        {
            if (State == TTTCell.StateEmpty)
            {
                return "&nbsp";
            }
            else
            {
                return State;
            }
        }
    }
}
