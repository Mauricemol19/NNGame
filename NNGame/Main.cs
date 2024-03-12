using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Screens;
using NNGame.Classes;

namespace NNGame
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

        public TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;

        private OrthographicCamera _camera;
        private Vector2 _cameraPosition;

        private readonly ScreenManager _screenManager;

        private ScreenLoader _screenLoader;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
        }

        /// <summary>
        /// Initialize app
        /// </summary>
        protected override void Initialize()
        {
            //Debug
            var viewportadapter = new BoxingViewportAdapter(Window, base.GraphicsDevice, 800, 600);
            _camera = new OrthographicCamera(viewportadapter);
            //

            base.Initialize();

            //_screenLoader = new ScreenLoader(this, _screenManager, _graphics.GraphicsDevice);
        }

        /// <summary>
        /// Load Content
        /// </summary>
        protected override void LoadContent()
        {
            //Debug
            _tiledMap = Content.Load<TiledMap>("Tilemaps/Grass");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //

            //use this.Content to load your game content here
        }

        /// <summary>
        /// Update Logic
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
           
            _tiledMapRenderer.Update(gameTime);    
            
            MoveCamera(gameTime);
            _camera.LookAt(_cameraPosition);            

            base.Update(gameTime);
        }

        /// <summary>
        /// Drawing Logic
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            _tiledMapRenderer.Draw(_camera.GetViewMatrix());

            //_tiledMapRenderer.Draw();
        }

        private Vector2 GetMovementDirection()
        {
            var movementDirection = Vector2.Zero;
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.W))
            {
                movementDirection -= Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.A))
            {
                movementDirection -= Vector2.UnitX;
            }
            if (state.IsKeyDown(Keys.S))
            {
                movementDirection += Vector2.UnitY;
            }         
            if (state.IsKeyDown(Keys.D))
            {
                movementDirection += Vector2.UnitX;
            }

            // Can't normalize the zero vector so test for it before normalizing
            if (movementDirection != Vector2.Zero)
            {
                movementDirection.Normalize();
            }

            return movementDirection;
        }

        private void MoveCamera(GameTime gameTime)
        {
            var speed = 200;
            var seconds = gameTime.GetElapsedSeconds();
            var movementDirection = GetMovementDirection();
            _cameraPosition += speed * movementDirection * seconds;
        }
    }
}
