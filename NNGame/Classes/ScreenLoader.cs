using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.Screens;
using NNGame.Classes.Screens;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace NNGame.Classes
{
    internal class ScreenLoader
    {
        private readonly Main Main;
        private readonly ScreenManager ScreenManager;
        private readonly GraphicsDevice GraphicsDevice;

        public ScreenLoader(Main _main, ScreenManager _screenManager, GraphicsDevice _graphicsDevice)
        {
            Main = _main;
            ScreenManager = _screenManager; 
            GraphicsDevice = _graphicsDevice;

            //Main._graphics.PreferredBackBufferWidth = viewportadapter.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            //Main._graphics.PreferredBackBufferHeight = viewportadapter.GraphicsDevice.Adapter.CurrentDisplayMode.Height;

            LoadMenu();
            LoadMap();
        }

        public void LoadMenu()
        {                        
            //ScreenManager.LoadScreen(new Menu(Main), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadMap()
        {
            ScreenManager.LoadScreen(new Map1(Main), new FadeTransition(GraphicsDevice, Color.Black));
        }
    }
}
