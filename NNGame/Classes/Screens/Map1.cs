using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended;
using System.Reflection.PortableExecutable;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.ViewportAdapters;
using GeonBit.UI;

namespace NNGame.Classes.Screens
{
    public class Map1 : GameScreen
    {
        private Main main => (Main)base.Game;

        public Map1(Main main) : base(main) { }

        public OrthographicCamera _camera;
        public Vector2 _cameraPosition;

        public override void LoadContent()
        {
            main._tiledMap = main.Content.Load<TiledMap>("Tilemaps/Grass");
            main._tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, main._tiledMap);
            main._spriteBatch = new SpriteBatch(GraphicsDevice);

            main.current_screen = "Map1";
        }

        public override void Update(GameTime gameTime)
        {
           
        }

        public override void Draw(GameTime gameTime)
        {          
           
        }
    }
}
