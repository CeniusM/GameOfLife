using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.GUI.SDL2;

namespace GameOfLife.GUI
{
    class SDLHelper
    {
		public static IntPtr LoadImage(string file, IntPtr rendere)
		{
			IntPtr loadedImage = IntPtr.Zero;
			IntPtr texture = IntPtr.Zero;
			loadedImage = SDL.SDL_LoadBMP(file);

			if (loadedImage != IntPtr.Zero)
			{
				texture = SDL.SDL_CreateTextureFromSurface(rendere, loadedImage);
				SDL.SDL_FreeSurface(loadedImage);
			}
			else
				Console.WriteLine(SDL.SDL_GetError());
			return texture;
		}
	}
}
