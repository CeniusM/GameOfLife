using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using GameOfLife.GUI.SDL2;
using GameOfLife.GUI;

//using PTR = System.UIntPtr;


namespace GameOfLife
{
    class Game
    {
        private static int Width = 400;
        private static int Height = 400;
        private static int PieceWidth = 2;
        private static int PieceHeight = 2;

        private static bool[,] board;
        private static bool[,] newBoard;
        private static IntPtr window;
        private static IntPtr renderer;
        private static int Tick = 0;

        private bool Pause = false; // to pause the generation
        private bool SingleStep = false; // to go one step forward

        public static void Start()
        {
            board = new bool[Width, Height];
            newBoard = new bool[Width, Height];
            
            // Initilizes SDL.
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine($"There was an issue initilizing SDL. {SDL.SDL_GetError()}");
            }

            // Create a new window given a title, size, and passes it a flag indicating it should be shown.
            window = SDL.SDL_CreateWindow("GameOfLife", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, Width * PieceWidth, Height * PieceHeight, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

            if (window == IntPtr.Zero)
            {
                Console.WriteLine($"There was an issue creating the window. {SDL.SDL_GetError()}");
            }

            // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
            renderer = SDL.SDL_CreateRenderer(window,
                                                    -1,
                                                    SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
                                                    SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            if (renderer == IntPtr.Zero)
            {
                Console.WriteLine($"There was an issue creating the renderer. {SDL.SDL_GetError()}");
            }

            // Initilizes SDL_image for use with png files.
            if (SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG) == 0)
            {
                Console.WriteLine($"There was an issue initilizing SDL2_Image {SDL_image.IMG_GetError()}");
            }

            GameLoop();

            CleanUp();
        }

        private static void GameLoop()
        {
            var rect = new SDL.SDL_Rect
            {
                x = 0,
                y = 0,
                w = PieceWidth,
                h = PieceHeight
            };
            Random rnd = new Random();
            Stopwatch sw = new Stopwatch();
            Foo.AVGTime<double> avg = new Foo.AVGTime<double>(100);

            // random board
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    board[x, y] = rnd.Next(0, 2) == 1 ? true : false;

            bool IsRunning = true;
            while (IsRunning)
            {
                sw.Restart();

                // Clear rendere
                SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
                SDL.SDL_RenderClear(renderer);

                bool randomize = false;
                // Check to see if there are any events and continue to do so until the queue is empty.
                while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
                {
                    switch (e.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                            IsRunning = false;
                            break;
                        case SDL.SDL_EventType.SDL_KEYDOWN:
                            randomize = true;
                            break;
                    }
                }

                // -- Game Render Code --
                SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);

                while (generating) // to stop it from generating the frame from an unfinished board
                    SDL.SDL_Delay(1);
                if (randomize) 
                    for (int x = 0; x < Width; x++)
                        for (int y = 0; y < Height; y++)
                            board[x, y] = rnd.Next(0, 2) == 1 ? true : false;

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        if (board[x, y])
                        {
                            rect.x = x * PieceWidth;
                            rect.y = y * PieceHeight;

                            SDL.SDL_RenderFillRect(renderer, ref rect);
                        }
                    }
                }

                // First starts a new thread to regenrate the board where after on this thread you disblay the image
                Task.Run(NewGeneration);
                SDL.SDL_RenderPresent(renderer);
                double time = sw.Elapsed.TotalMilliseconds;
                if (time < 17) // 1000 / 60(fps) = 16.666 = ~17
                    SDL.SDL_Delay(17 - (uint)time);
                avg.Push(time);
                if (avg.IsReady())
                    if (Tick % 100 == 1)
                        Console.WriteLine((uint)avg.GetAVG() + "ms");
                Tick++;
            }
        }

        private static void CleanUp()
        {
            // Clean up the resources that were created.
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
        }

        private static bool generating = false;
        public static void NewGeneration() // needs some cleaning
        {
            generating = true;

            int Lenght0 = board.GetLength(0);
            int Lenght1 = board.GetLength(1);

            // local func
            int GetAliveAmount(int x, int y)
            {
                int Amout = 0;

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0) continue;

                        if ((x + i) < Lenght0 && (y + j) < Lenght1 && (x + i) > -1 && (y + j) > -1)
                            if (board[x + i, y + j])
                                Amout++;
                    }
                }

                return Amout;
            }

            for (int i = 0; i < Lenght0; i++)
            {
                //int* 
                for (int j = 0; j < Lenght1; j++)
                {
                    int AliveAmout = GetAliveAmount(i, j);

                    // alive
                    if (board[i, j])
                    {
                        if (2 > AliveAmout)
                            newBoard[i, j] = false;

                        else if (3 < AliveAmout)
                            newBoard[i, j] = false;

                        else
                            newBoard[i, j] = true;
                    }

                    // dead
                    else
                        if (AliveAmout == 3)
                        newBoard[i, j] = true;
                }
            }

            // switch the refrences so we dont have to make a new board everytime
            var tempRef = newBoard;
            newBoard = board;
            board = tempRef;

            // DONT REMOVE THIS (⊙_⊙;), dont know why it dosent work without
            for (int i = 0; i < Lenght0; i++)
                for (int j = 0; j < Lenght1; j++)
                    newBoard[i, j] = false;

            //board = newBoard;

            generating = false;
        }
    }
}
