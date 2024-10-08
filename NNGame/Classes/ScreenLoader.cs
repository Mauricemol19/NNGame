using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.Screens;
using NNGame.Classes.Screens;

namespace NNGame.Classes
{
    public class ScreenLoader
    {
        public string current_screen { get; set; }

        private readonly Main Main;
        private ScreenManager ScreenManager;
        private GraphicsDevice GraphicsDevice;

        public ScreenLoader(Main _main, ScreenManager _screenManager, GraphicsDevice _graphicsDevice)
        {
            Main = _main;
            ScreenManager = _screenManager; 
            GraphicsDevice = _graphicsDevice;

            //LoadMenu();
            LoadWorldMap();
        }

        public void LoadMainMenu()
        {

        }

        public void LoadMap(ushort id)
        {
            //ScreenManager.LoadScreen(new Menu(Main), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadMenu()
        {                        
            //ScreenManager.LoadScreen(new Menu(Main), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadWorldMap()
        {
            ScreenManager.LoadScreen(new WorldMap(Main), new FadeTransition(GraphicsDevice, Color.Black));           
        }
    }
}
