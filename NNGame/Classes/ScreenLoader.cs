using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.Screens;
using NNGame.Classes.Screens;

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

            LoadMenu();
        }

        public void LoadMenu()
        {
            ScreenManager.LoadScreen(new Menu(Main), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadMap()
        {

        }

        private void LoadTiledMap() 
        {
            //_tiledMap = Content.Load<TiledMap>("Tilemaps/Grass");
            //_tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            //_spriteBatch = new SpriteBatch(GraphicsDevice);
        }
    }
}
