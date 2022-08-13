using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Classes
{
    public class Move
    {
        public int x;
        public int y;
        public ContextConnect4 NextState { get; set; }


    }
}
