namespace GameOfLife // *Conways Game of life
{
    class Generation
    {
        public static bool[,] New(bool[,] Board)
        {
            bool[,] NewBoard = new bool[Board.GetLength(0), Board.GetLength(1)];

            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    int AliveAmout = GetAliveAmount(Board, i, j);

                    if (Board[i, j])
                    {
                        if (2 > AliveAmout)
                        {
                            NewBoard[i, j] = false;
                            continue;
                        }
                        else if (3 < AliveAmout)
                        {
                            NewBoard[i, j] = false;
                            continue;
                        }
                        else
                        {
                            NewBoard[i, j] = true;
                            continue;
                        }
                    }
                    else
                    {
                        if (AliveAmout == 3)
                        {
                            NewBoard[i, j] = true;
                            continue;
                        }
                    }
                }
            }

            return NewBoard;
        }
        private static int GetAliveAmount(bool[,] Board, int x, int y)
        {
            int Amout = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0) continue;

                    if ((x + i) < Board.GetLength(0) && (y + j) < Board.GetLength(1) && (x + i) > -1 && (y + j) > -1)
                        if (Board[x + i, y + j])
                            Amout++;
                }
            }

            return Amout;
        }
    }
}