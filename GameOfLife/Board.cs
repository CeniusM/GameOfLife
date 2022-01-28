namespace GameOfLife
{
    class Board
    {
        public static bool[,] RandomBoard(int height, int width, int chanceOutOf)
        {
            Random rnd = new Random();
            bool[,] Board = new bool[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (rnd.Next(0, chanceOutOf) == 0)
                        Board[i, j] = true;
                    else
                        Board[i, j] = false;
                }
            }

            return Board;
        }
    }
}