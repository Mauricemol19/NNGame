using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended;

namespace NNGame.Classes.Screens
{
    //extern alias TiledPv;
    //using Nspace = TiledPv::MonoGame.Extended.Tiled;

    public class WorldMap : GameScreen
    {
        private Main main => (Main)base.Game;

        public WorldMap(Main main) : base(main) { }

        public string name = "WorldMap";

        public OrthographicCamera _camera;
        public Vector2 _cameraPosition;

        public override void LoadContent()
        {
            //TiledPv::MonoGame.Extended.Tiled.TiledMap f = new
                //TiledPv::MonoGame.Extended.Tiled.TiledMap();

            //main._tiledMap = main.Content.Load<TiledMap>("TileMaps/Grass");
            //main._tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, main._tiledMap);
            //main._spriteBatch = new SpriteBatch(GraphicsDevice);
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
