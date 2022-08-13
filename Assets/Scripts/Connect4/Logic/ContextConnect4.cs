using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Classes
{
    public class ContextConnect4
    {
        public int CurrentPlayer { get; set; }

        public Board CurrentState { get; protected set; }



        public ContextConnect4(Board t, int player)
        {
            CurrentPlayer = player;
            CurrentState = t;
        }

        public List<Move> GetPosibleMoves()
        {
            List<Move> moves = CurrentState.GetListMoves();
            moves.ForEach(x =>
            {
                x.NextState = new ContextConnect4(new Board(CurrentState), CurrentPlayer);
                x.NextState.CurrentState.play(x.y, CurrentPlayer);
                x.NextState.Next();
            });
            return moves;
        }
        internal void Next()
        {
            if (CurrentPlayer == 1)
                CurrentPlayer = 2;
            else
                CurrentPlayer = 1;
        }

    }
}
