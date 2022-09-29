using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Classes
{
    public class Board
    {
        public static int WIDTH = 7;
        public static int HEIGHT = 6;
        public static int[] columnOrder = new int[] { 3, 2, 4, 1, 5, 0, 6 };

        //koristim ovo za proveru kraja igre i heuristiku jer mi je lakse da radim sa nizovima
        public int[,] board = new int[WIDTH, HEIGHT];
        private int[] columns = new int[WIDTH];
        public int won = 0;
        /** Koristi se bitboard za lakse indeksiranje u transpozicionu tabelu
         * Bitboard je velicine Width*(Height+1)
         * maska je bitboard koji sadrzi 1 na svim lokacijama gde se nalazi krug
         * current_position sadrzi 1 na svim lokacijama gde trenutni igrac ima krug
         */
        private UInt64 current_position;
        private UInt64 mask;
        private uint moves;

        public Board()
        {
            moves = 0;

            
            for(int i = 0; i < board.GetLength(0); i++)
            {
                for(int j=0; j < board.GetLength(1); j++)
                {
                    board[i, j] = 0;
                }
            }
            for (int k = 0; k < 7; k++)
                columns[k] = 0;
        }
        public Board(Board t)
        {

            moves = t.moves;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = t.board[i,j];
                }
            }
            for (int k = 0; k < 7; k++)
                columns[k] = t.columns[k];
            current_position = t.current_position;
            mask = t.mask;
        }
        //Vraca igraca na polju
        public int Position(int x, int y)
        {
            if (x < 0 || x > board.GetLength(0))
                return 0;
            if (y < 0 || y > board.GetLength(1))
                return 0;
            if (board == null)
                return 0;
            return board[x, y];
        }

        

        public bool IsEndOfGame(out int won)
        {
            int current = 0;
            won = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    //proveri kolone
                    if (j < 3)
                    {
                        if (board[i, j] != 0)
                        {
                            current = board[i, j];
                            if (board[i, j + 1] == current && board[i, j + 2] == current && board[i, j + 3] == current)
                            {
                                won = current;
                                return true;
                            }  
                        }
                    }
                    //proveri redove
                    if (i < 4)
                    {
                        if (board[i, j] != 0)
                        {
                            current = board[i, j];
                            if (board[i+1, j] == current && board[i+2, j] == current && board[i+3, j] == current)
                            {
                                won = current;
                                return true;
                            }
                        }
                    }
                    //proveri sporedne dijagonale
                    if(i<4 && j < 3)
                    {
                        if (board[i, j] != 0)
                        {
                            current = board[i, j];
                            if (board[i + 1, j+1] == current && board[i + 2, j+2] == current && board[i + 3, j+3] == current)
                            {
                                won = current;
                                return true;
                            }
                        }
                    }
                    //proveri sporedne dijagonale
                    if (i < 4 && j >= 3)
                    {
                        if (board[i, j] != 0)
                        {
                            current = board[i, j];
                            if (board[i + 1, j - 1] == current && board[i + 2, j - 2] == current && board[i + 3, j - 3] == current)
                            {
                                won = current;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        
        public List<Move> GetListMoves()
        {
            List<Move> potezi = new List<Move>();
            if (IsEndOfGame(out won))
                return potezi;
            for (int i = 0; i < 7; i++)
            {
                if (columns[columnOrder[i]] < HEIGHT)
                {
                    potezi.Add(new Move()
                    {
                        x = columns[i]-1,
                        y = columnOrder[i]
                    });
                }
            }
            return potezi;
        }



        //Novo za bitboards

        public UInt64 key()
        {
            return current_position + mask;
        }
        public uint nbMoves()
        {
            return moves;
        }
        //provera da li postoji 4 u red
        static bool alignment(UInt64 pos)
        {
            //horizontal
            UInt64 m = pos & (pos >> (HEIGHT + 1));

            if (Convert.ToBoolean(m & (m >> (2 * (HEIGHT + 1))))) return true;

            //diagonal 1
            m = pos & (pos >> HEIGHT);
            if (Convert.ToBoolean(m & (m >> (2 * HEIGHT)))) return true;

            //diagonal 2
            m = pos & (pos >> (HEIGHT + 2));
            if (Convert.ToBoolean(m & (m >> (2 * (HEIGHT + 2))))) return true;

            //vertical
            m = pos & (pos >> 1);
            if (Convert.ToBoolean(m & (m >> 2))) return true;

            return false;
        }
        //provera da li na vrhu moze da se stavi figura na osnovu maske
        public bool canPlay(int col)
        {
            return (mask & top_mask(col)) == 0;
        }
        //vraca masku koja ima jedinicu na vrhu kolone
        static UInt64 top_mask(int col)
        {
            return ((UInt64)(1) << (HEIGHT - 1)) << col * (HEIGHT + 1);
        }

        public void play(int col,int player)
        {
            //xor operator da se promeni current_position tj current player promeni
            current_position ^= mask;
            //dodaj na masku trenutni potez
            mask |= mask + bottom_mask(col);
            moves++;

            //odigraj potez standardno radi kasnije heuristike
            board[col, columns[col]] = player; //1 + (Convert.ToInt32(moves) % 2);
            columns[col]++;
        }
        //vrati masku sa 1 na dnu kolone
        static UInt64 bottom_mask(int col)
        {
            return (UInt64)(1) << col * (HEIGHT + 1);
        }
        //provera da li ce osoba da pobedi ako odigra potez u odgovarajucoj koloni
        public bool isWinningMove(int col)
        {
            UInt64 pos = current_position;
            pos |= (mask + bottom_mask(col)) & column_mask(col);
            return alignment(pos);
        }
        //vrati masku sa 1 na svim kolonama
        static UInt64 column_mask(int col)
        {
            return (((UInt64)(1) << HEIGHT) - 1) << col * (HEIGHT + 1);
        }
        //koristi se za prikaz
        public int Top(int col)
        {
            return columns[col];
        }
    }
}
