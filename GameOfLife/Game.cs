using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using GameOfLife.GUI.SDL2;
using GameOfLife.GUI;

namespace GameOfLife
{
    class Game
    {
        private static int Width = 200;
        private static int Height = 200;
        private static int PieceWidth = 4;
        private static int PieceHeight = 4;

        private static GameOfLife game;
        private static IntPtr window;
        private static IntPtr renderer;
        private static int Tick = 0;

        public static void Start()
        {
            game = new();

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
            Stopwatch sw = new Stopwatch();
            uint[] arr = new uint[10];
            int arrCount = 0;
            bool IsRunning = true;

            while (IsRunning)
            {
                sw.Restart();
                // Clear rendere
                SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
                SDL.SDL_RenderClear(renderer);


                // -- Game Code --

                // Check to see if there are any events and continue to do so until the queue is empty.
                while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
                {
                    switch (e.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                            IsRunning = false;
                            break;
                    }
                }


                Tick++;
                // -- Game Render Code --

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        var rect = new SDL.SDL_Rect
                        {
                            x = x * PieceWidth,
                            y = y * PieceHeight,
                            w = PieceWidth,
                            h = PieceHeight
                        };
                        Random rnd = new Random();

                        SDL.SDL_SetRenderDrawColor(renderer, (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), 255);

                        // Draw a filled in rectangle.
                        SDL.SDL_RenderFillRect(renderer, ref rect);
                    }
                }


                // Switches out the currently presented render surface with the one we just did work on.
                SDL.SDL_RenderPresent(renderer);

                uint time = (uint)sw.ElapsedMilliseconds; // FPS*
                //if (time > 0)
                // Console.WriteLine("Time per frame exeeded the FPS minimum: " + time + "ms");
                //else
                if (time > 17)
                    SDL.SDL_Delay(time - 17); // 1000 / 60(fps) = 16,666 = ~17

                arr[arrCount] = time;
                arrCount++;
                if (arrCount == 10)
                    arrCount = 0;
                long totalCount = 0;
                if (Tick % 10 == 1)
                {
                    for (int i = 0; i < 10; i++) totalCount += arr[i];
                    Console.WriteLine((totalCount / 10) + "ms");
                }
            }
        }

        private static void CleanUp()
        {
            // Clean up the resources that were created.
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();

            // Sprites

        }
    }
}
