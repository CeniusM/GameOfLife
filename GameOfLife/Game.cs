namespace GameOfLife // *Conways Game of life
{
    class Game // make a saving system
    {
        private Form1 _form;
        private CS_GUI.GUI _GUI;
        private bool[,] Board; // true = alive, and should probely have been its own class...
        private bool _Pause = false;
        private bool _isRunning = false;
        public Game(Form1 form)
        {
            _form = form;
            _GUI = new CS_GUI.GUI(form);
            Board = new bool[form.Height / 10, form.Width / 10];
            _form.KeyPress += KeyPress;
            _form.MouseClick += MouseClick;
        }

        public Game(Form1 form, int height, int width)
        {
            _form = form;
            _GUI = new CS_GUI.GUI(form);
            Board = new bool[height, width];
            _form.KeyPress += KeyPress;
            _form.MouseClick += MouseClick;
        }

        private void KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'p') // pauses the game
            {
                if (_Pause) _Pause = false;
                else _Pause = true;
            }
            else if (e.KeyChar == 'r') // creates random board
            {
                Board = GameOfLife.Board.RandomBoard(Board.GetLength(0), Board.GetLength(1), 2);
            }
            else if (e.KeyChar == 'w') // creates random board and makes more fields
            {
                Board = GameOfLife.Board.RandomBoard(Board.GetLength(0) + 1, Board.GetLength(1) + 1, 2);
            }
            else if (e.KeyChar == 's') // creates random board and remove fields
            {
                Board = GameOfLife.Board.RandomBoard(Board.GetLength(0) - 1, Board.GetLength(1) - 1, 2);
            }
            else if (e.KeyChar == 'a') // Removes everything
            {
                Board = new bool[Board.GetLength(0), Board.GetLength(1)];
            }
            else if (e.KeyChar == 'q') // Creates a glider
            {
                Board = new bool[Board.GetLength(0), Board.GetLength(1)];
                Board[3, 0] = true;
                Board[3, 1] = true;
                Board[3, 2] = true;
                Board[2, 2] = true;
                Board[1, 1] = true;
            }
        }

        private void MouseClick(object? sender, MouseEventArgs e)
        {
            if (!_Pause) return;
            int x = (int)((float)e.X / (float)_form.Width * (float)Board.GetLength(1));
            int y = (int)((float)e.Y / (float)_form.Height * (float)Board.GetLength(0));

            //yea idk how to fix it
            // int x = (int)((float)Board.GetLength(1) / (float)_form.Width * e.X);
            // int y = (int)((float)Board.GetLength(0) / (float)_form.Height * e.Y);

            if (Board[x, y]) Board[x, y] = false;
            else Board[x, y] = true;

            PrintBoard();
        }

        public void Start()
        {
            _isRunning = true;

            // test
            Board[4, 5] = true;
            Board[4, 6] = true;
            Board[4, 7] = true;

            while (_isRunning) // make it so it runs a serten amout a second
            {
                Board = Generation.New(Board);
                PrintBoard();
                while (_Pause && _isRunning) Thread.Sleep(100);
            }
        }

        public void Stop() => _isRunning = false;

        public void PrintBoard()
        {
            int BlockRenderHeight = _form.Height / Board.GetLength(0);
            int BlockRenderWidth = _form.Width / Board.GetLength(1);

            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    if (Board[i, j])
                    {
                        int x1 = BlockRenderHeight * i;
                        int y1 = BlockRenderWidth * j;
                        int x2 = BlockRenderHeight * (i + 1);
                        int y2 = BlockRenderWidth * (j + 1);

                        // _GUI.DrawLine(x1, y1, x2, y2, Color.Blue, (BlockRenderHeight / 10) + (BlockRenderWidth / 10));
                        // _GUI.DrawRectangle(x1, y1, x2, y2, Color.Blue);


                        // if (!_Pause)
                        _GUI.FilledRectangle(x1, y1, x2, y2, Color.Blue);
                        // else
                        //     _GUI.FilledRectangle(x1, y1, x2, y2, Color.Red);
                    }
                }
            }

            _GUI.Print();
            _GUI.Reset();
        }
    }
}