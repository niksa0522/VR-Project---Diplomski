using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Classes
{
    public class GameLogic
    {

        int[] columnOrder = new int[Board.WIDTH];

        TranspositionTable transpositionTable;

        public GameLogic()
        {
            for (int i = 0; i < Board.WIDTH; i++)
            {
                //kolone idu 3,2,4,1,5,0,6
                columnOrder[i] = Board.WIDTH / 2 + (1 - 2 * (i % 2)) * (i + 1) / 2;
            }
            transpositionTable = new TranspositionTable(8388608);
            transpositionTable.Clear();
        }

        //Funkcija koja se zove da vrati potez, ova funkcija je takodje alpha beta samo bez transpozicione tabele
        public Move ReturnMove(ContextConnect4 context, int dubina)
        {
            List<Move> potezi = context.GetPosibleMoves();
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            if(context.CurrentPlayer == 1)
            {
                int bestValue = int.MinValue;
                Move najbolji = null;
                foreach(Move p in potezi)
                {
                    int val = AlphaBeta(p.NextState, dubina - 1, alpha, beta);
                    if(val > bestValue)
                    {
                        bestValue = val;
                        najbolji = p;
                    }
                    alpha = Math.Max(bestValue, alpha);
                    if (p.NextState.CurrentState.won == 1)
                        return p;
                    if (beta <= alpha)
                        break;
                }
                return najbolji;
            }
            else
            {
                int bestValue = int.MaxValue;
                Move najbolji = null;
                foreach (Move pot in potezi)
                {
                    int val = AlphaBeta(pot.NextState, dubina - 1, alpha, beta);
                    if (val < bestValue)
                    {
                        bestValue = val;
                        najbolji = pot;
                    }
                    beta = Math.Min(bestValue, beta);
                    if (pot.NextState.CurrentState.won == 2)
                        return pot;
                    if (beta <= alpha)
                        break;
                }
                return najbolji;
            }
        }

        public int AlphaBeta(ContextConnect4 context, int dubina, int alpha, int beta)
        {
            //Transposition table kod je uzet iz prethodnog projekta, konkretno IksOks koji sam radio za Teoriju Igara u 3oj godini

            //koristi alphaOrig
            int value;
            HashObject ttEntry = transpositionTable.get(context.CurrentState.key());
            if (ttEntry != null && ttEntry.Dubina >= dubina)
            {
                if (ttEntry.Flag == Flag.Exact)
                    return ttEntry.Value;
                else if (ttEntry.Flag == Flag.Lowerbound)
                    alpha = Math.Max(alpha, ttEntry.Value);
                else
                    beta = Math.Min(beta, ttEntry.Value);
                if (alpha >= beta)
                    return ttEntry.Value;
            }
            List<Move> potezi = context.GetPosibleMoves();
            //int won;
            if(dubina == 0 || context.CurrentState.nbMoves() >= Board.WIDTH*Board.HEIGHT-2)
            {
                value = Evaluate(context);
                if (value <= alpha)
                    transpositionTable.put(context.CurrentState.key(), value, dubina, Flag.Upperbound);
                else if (value >= beta)
                    transpositionTable.put(context.CurrentState.key(), value, dubina, Flag.Lowerbound);
                else
                    transpositionTable.put(context.CurrentState.key(), value, dubina, Flag.Exact);
                
                return value;
            }
            //AKO JE IGRA GOTOVA AUTOMATSKI VRATI POTEZ
            //Ovo je uradjeno radi ubrzanja pretrage ako je neko pobedio u slucaju stavljanja 3 u krajnje desnoj ili levoj koloni
            //Sobzirom da se te kolone na kraju pretrazuju jer je redosled poteza 3,2,4,1,5,0,6, ovo nije veliko ubrzanje
            /*if (context.CurrentState.IsEndOfGame(out won))
            {
                if (won == 1)
                {
                    value = 10000;
                }
                else
                {
                    value = -10000;
                }
                if (value <= alpha)
                    transpositionTable.put(context.TrenutnoStanje.key(), value, dubina, Flag.Lowerbound);
                else if (value >= beta)
                    transpositionTable.put(context.TrenutnoStanje.key(), value, dubina, Flag.Upperbound);
                else
                    transpositionTable.put(context.TrenutnoStanje.key(), value, dubina, Flag.Exact);

                return value;
            }*/
            //Koristim bitboard da proverim da li neki od poteza pobedjuje, ako pobedjuje onda se vraca vrednost
            //Nisam testirao ovaj kod u potpunosti ali bi trebalo da radi
            for (int i=0; i<Board.WIDTH; i++)
            {
                if (context.CurrentState.isWinningMove(i))
                {
                    if (context.CurrentPlayer == 1)
                    {
                        value = 10000;
                    }
                    else
                    {
                        value = -10000;
                    }
                    if (value <= alpha)
                        transpositionTable.put(context.CurrentState.key(), value, dubina, Flag.Upperbound);
                    else if (value >= beta)
                        transpositionTable.put(context.CurrentState.key(), value, dubina, Flag.Lowerbound);
                    else
                        transpositionTable.put(context.CurrentState.key(), value, dubina, Flag.Exact);

                    return value;
                }
            }
            if(context.CurrentPlayer == 1)
            {
                int bestValue = int.MinValue;
                foreach (Move pot in potezi)
                {
                    value = AlphaBeta(pot.NextState, dubina - 1, alpha, beta);
                    if (value > bestValue)
                        bestValue = value;
                    if (bestValue > alpha)
                        alpha = bestValue;
                    if (alpha >= beta)
                        break;
                }
                if (bestValue <= alpha)
                    transpositionTable.put(context.CurrentState.key(), bestValue, dubina, Flag.Upperbound);
                else if (bestValue >= beta)
                    transpositionTable.put(context.CurrentState.key(), bestValue, dubina, Flag.Lowerbound);
                else
                    transpositionTable.put(context.CurrentState.key(), bestValue, dubina, Flag.Exact);
                return bestValue;
            }
            else
            {
                int bestValue = int.MaxValue;
                foreach (Move pot in potezi)
                {
                    value = AlphaBeta(pot.NextState, dubina - 1, alpha, beta);
                    if (value < bestValue)
                        bestValue = value;
                    if (bestValue < beta)
                        beta = bestValue;
                    if (alpha >= beta)
                        break;
                }
                //ovde mozda alphaOrig
                if (bestValue <= alpha)
                    transpositionTable.put(context.CurrentState.key(), bestValue, dubina, Flag.Upperbound);
                else if (bestValue >= beta)
                    transpositionTable.put(context.CurrentState.key(), bestValue, dubina, Flag.Lowerbound);
                else
                    transpositionTable.put(context.CurrentState.key(), bestValue, dubina, Flag.Exact);
                return bestValue;
            }
        }
        
        public IEnumerator ReturnMoveCoroutine(ContextConnect4 context, int dubina, Action<Move> callback = null)
        {
            List<Move> potezi = context.GetPosibleMoves();
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            if(context.CurrentPlayer == 1)
            {
                int bestValue = int.MinValue;
                Move najbolji = null;
                foreach(Move p in potezi)
                {
                    yield return null;
                    
                    yield return AlphaBetaCoroutine(p.NextState, dubina - 1, alpha, beta);
                    int val = AlphaBetaCoroutineHolder;
                    if(val > bestValue)
                    {
                        bestValue = val;
                        najbolji = p;
                    }
                    alpha = Math.Max(bestValue, alpha);
                    if (p.NextState.CurrentState.won == 1)
                    {
                        if (callback != null)
                        {
                            callback.Invoke(p);
                        }
                    }
                    if (beta <= alpha)
                        break;
                }
                if (callback != null)
                {
                    callback.Invoke(najbolji);
                }
            }
            else
            {
                int bestValue = int.MaxValue;
                Move najbolji = null;
                foreach (Move pot in potezi)
                {
                    yield return null;
                    
                    yield return AlphaBetaCoroutine(pot.NextState, dubina - 1, alpha, beta);
                    int val = AlphaBetaCoroutineHolder;
                    if (val < bestValue)
                    {
                        bestValue = val;
                        najbolji = pot;
                    }
                    beta = Math.Min(bestValue, beta);
                    if (pot.NextState.CurrentState.won == 2)
                    {
                        if (callback != null)
                        {
                            callback.Invoke(pot);
                        } 
                    }
                    if (beta <= alpha)
                        break;
                }
                if (callback != null)
                {
                    callback.Invoke(najbolji);
                }
            }
        }

        private int AlphaBetaCoroutineHolder;
        
        public IEnumerator AlphaBetaCoroutine(ContextConnect4 context, int dubina, int alpha, int beta)
        {
            //Transposition table kod je uzet iz prethodnog projekta, konkretno IksOks koji sam radio za Teoriju Igara u 3oj godini

            //koristi alphaOrig
            int value;
            HashObject ttEntry = transpositionTable.get(context.CurrentState.key());
            if (ttEntry != null && ttEntry.Dubina >= dubina)
            {
                if (ttEntry.Flag == Flag.Exact)
                {
                    AlphaBetaCoroutineHolder = ttEntry.Value;
                    yield break;
                }
                else if (ttEntry.Flag == Flag.Lowerbound)
                    alpha = Math.Max(alpha, ttEntry.Value);
                else
                    beta = Math.Min(beta, ttEntry.Value);

                if (alpha >= beta)
                {
                    AlphaBetaCoroutineHolder = ttEntry.Value;
                    yield break;
                }
            }
            List<Move> potezi = context.GetPosibleMoves();
            //int won;
            if(dubina == 0 || context.CurrentState.nbMoves() >= Board.WIDTH*Board.HEIGHT-2)
            {
                value = Evaluate(context);
                if (value <= alpha)
                    transpositionTable.put(context.CurrentState.key(), value, dubina, Flag.Upperbound);
                else if (value >= beta)
                    transpositionTable.put(context.CurrentState.key(), value, dubina, Flag.Lowerbound);
                else
                    transpositionTable.put(context.CurrentState.key(), value, dubina, Flag.Exact);
                
                AlphaBetaCoroutineHolder = value;
                yield break;
            }
            //AKO JE IGRA GOTOVA AUTOMATSKI VRATI POTEZ
            //Ovo je uradjeno radi ubrzanja pretrage ako je neko pobedio u slucaju stavljanja 3 u krajnje desnoj ili levoj koloni
            //Sobzirom da se te kolone na kraju pretrazuju jer je redosled poteza 3,2,4,1,5,0,6, ovo nije veliko ubrzanje
            /*if (context.CurrentState.IsEndOfGame(out won))
            {
                if (won == 1)
                {
                    value = 10000;
                }
                else
                {
                    value = -10000;
                }
                if (value <= alpha)
                    transpositionTable.put(context.TrenutnoStanje.key(), value, dubina, Flag.Lowerbound);
                else if (value >= beta)
                    transpositionTable.put(context.TrenutnoStanje.key(), value, dubina, Flag.Upperbound);
                else
                    transpositionTable.put(context.TrenutnoStanje.key(), value, dubina, Flag.Exact);

                return value;
            }*/
            //Koristim bitboard da proverim da li neki od poteza pobedjuje, ako pobedjuje onda se vraca vrednost
            //Nisam testirao ovaj kod u potpunosti ali bi trebalo da radi
            for (int i=0; i<Board.WIDTH; i++)
            {
                if (context.CurrentState.isWinningMove(i))
                {
                    if (context.CurrentPlayer == 1)
                    {
                        value = 10000;
                    }
                    else
                    {
                        value = -10000;
                    }
                    if (value <= alpha)
                        transpositionTable.put(context.CurrentState.key(), value, dubina, Flag.Upperbound);
                    else if (value >= beta)
                        transpositionTable.put(context.CurrentState.key(), value, dubina, Flag.Lowerbound);
                    else
                        transpositionTable.put(context.CurrentState.key(), value, dubina, Flag.Exact);

                    AlphaBetaCoroutineHolder = value;
                    yield break;
                }
            }
            if(context.CurrentPlayer == 1)
            {
                int bestValue = int.MinValue;
                foreach (Move pot in potezi)
                {
                    //yield return null;
                    
                    IEnumerator alphaBetaIteration = AlphaBetaCoroutine(pot.NextState, dubina - 1, alpha, beta);
                    yield return alphaBetaIteration;
                    value = AlphaBetaCoroutineHolder;
                    if (value > bestValue)
                        bestValue = value;
                    if (bestValue > alpha)
                        alpha = bestValue;
                    if (alpha >= beta)
                        break;
                }
                if (bestValue <= alpha)
                    transpositionTable.put(context.CurrentState.key(), bestValue, dubina, Flag.Upperbound);
                else if (bestValue >= beta)
                    transpositionTable.put(context.CurrentState.key(), bestValue, dubina, Flag.Lowerbound);
                else
                    transpositionTable.put(context.CurrentState.key(), bestValue, dubina, Flag.Exact);
                
                AlphaBetaCoroutineHolder = bestValue;
                yield break;
            }
            else
            {
                int bestValue = int.MaxValue;
                foreach (Move pot in potezi)
                {
                    
                    //yield return null;
                    
                    IEnumerator alphaBetaIteration = AlphaBetaCoroutine(pot.NextState, dubina - 1, alpha, beta);
                    yield return alphaBetaIteration;
                    value = AlphaBetaCoroutineHolder;
                    
                    if (value < bestValue)
                        bestValue = value;
                    if (bestValue < beta)
                        beta = bestValue;
                    if (alpha >= beta)
                        break;
                }
                //ovde mozda alphaOrig
                if (bestValue <= alpha)
                    transpositionTable.put(context.CurrentState.key(), bestValue, dubina, Flag.Upperbound);
                else if (bestValue >= beta)
                    transpositionTable.put(context.CurrentState.key(), bestValue, dubina, Flag.Lowerbound);
                else
                    transpositionTable.put(context.CurrentState.key(), bestValue, dubina, Flag.Exact);
                AlphaBetaCoroutineHolder = bestValue;
                yield break;
            }
        }



        public int Evaluate(ContextConnect4 c)
        {
            int won;
            Board t = c.CurrentState;
            if (t.IsEndOfGame(out won))
            {
                if(won == 1)
                {
                    return 10000;
                }
                else
                {
                    return -10000;
                }
            }
            int valueMax = EvaluateForPlayer(c, 1);
            int valueMin = EvaluateForPlayer(c, 2);
            return valueMax - valueMin;
        }

        public int EvaluateForPlayer(ContextConnect4 context, int Player)
        {
            int score = 0;
            //centralna Kolona
            int numPlayerInCenterKolumn=0;
            for(int i = 0; i < 6; i++)
            {
                if (context.CurrentState.Position(3, i) == Player)
                    numPlayerInCenterKolumn++; 
            }
            score = 3 * numPlayerInCenterKolumn;
            score += EvaluateHorizontal(context, Player);
            score += EvaluateVertical(context, Player);
            score += EvaluateDiagonal(context, Player);
            return score;
        }

        private int EvaluateHorizontal(ContextConnect4 context, int Player)
        {
            int score = 0;
            int blank = 0;
            int playerPiece = 0;
            int opponentPiece = 0;
            for(int j = 0; j < 6; j++)
            {
                for(int i = 0; i < 4; i++)
                {
                    blank = 0;
                    playerPiece = 0;
                    opponentPiece = 0;
                    for(int k = 0; k < 4; k++)
                    {
                        int polje = context.CurrentState.Position(i+k, j);
                        if (polje == 0)
                            blank++;
                        else if (polje == Player)
                            playerPiece++;
                        else
                            opponentPiece++;
                    }
                    score += Heuristic(playerPiece, blank, opponentPiece);
                }
            }
            return score;
        }
        private int EvaluateVertical(ContextConnect4 context, int Player)
        {
            int score = 0;
            int blank = 0;
            int playerPiece = 0;
            int opponentPiece = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    blank = 0;
                    playerPiece = 0;
                    opponentPiece = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        int polje = context.CurrentState.Position(i, j+k);
                        if (polje == 0)
                            blank++;
                        else if (polje == Player)
                            playerPiece++;
                        else
                            opponentPiece++;
                    }
                    score += Heuristic(playerPiece, blank, opponentPiece);
                }
            }
            return score;
        }

        private int EvaluateDiagonal(ContextConnect4 context, int Player)
        {
            int score = 0;
            int blank = 0;
            int playerPiece = 0;
            int opponentPiece = 0;
            for (int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    //sporedne
                    blank = 0;
                    playerPiece = 0;
                    opponentPiece = 0;
                    for (int k =0;k<4;k++)
                    {
                        int polje = context.CurrentState.Position(i + k, j+k);
                        if (polje == 0)
                            blank++;
                        else if (polje == Player)
                            playerPiece++;
                        else
                            opponentPiece++;
                    }
                    score += Heuristic(playerPiece, blank, opponentPiece);

                    //glavna dijagonale
                    blank = 0;
                    playerPiece = 0;
                    opponentPiece = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        int polje = context.CurrentState.Position(i+k, j+3-k);
                        if (polje == 0)
                            blank++;
                        else if (polje == Player)
                            playerPiece++;
                        else
                            opponentPiece++;
                    }
                    score += Heuristic(playerPiece, blank, opponentPiece);
                }
            }
            return score;
        }

        private int Heuristic(int playerPiece, int blank, int opponentPiece)
        {
            int score = 0;
            if (playerPiece == 4)
                score += 100;
            else if (playerPiece == 3 && blank == 1)
                score += 5;
            else if (playerPiece == 2 && blank == 2)
                score += 2;
            if (opponentPiece == 3 && blank == 1)
                score -= 4;
            return score;
        }

    }
}
