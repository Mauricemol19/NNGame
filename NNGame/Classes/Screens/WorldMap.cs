using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace NNGame.Classes.Screens
{
    public class WorldMap : GameScreen
    {
        private Main main => (Main)base.Game;

        public WorldMap(Main main) : base(main) { }

        public string name = "WorldMap";

        public OrthographicCamera _camera;
        public Vector2 _cameraPosition;

        public override void LoadContent()
        {
            
            main._tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, main._tiledMap);
            main._spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);
            main._current_screen = name;
        }

        public override void Update(GameTime gameTime)
        {
           
        }

        public override void Draw(GameTime gameTime)
        {          
           
        }
    }
}
