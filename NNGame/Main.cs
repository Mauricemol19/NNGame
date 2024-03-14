using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Screens;
using NNGame.Classes;
using NNGame.Classes.Gui;
using GeonBit.UI;
using GeonBit.UI.Entities;
using System.Collections.Generic;
using Autofac.Core;

namespace NNGame
{
    public class Main : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

        public TiledMap _tiledMap;
        public TiledMapRenderer _tiledMapRenderer;

        public OrthographicCamera _camera;
        public Vector2 _cameraPosition;

        private readonly ScreenManager _screenManager;

        private ScreenLoader _screenLoader;

        public Vector2 _worldPosition;

        public string current_screen = "Menu";

        public GameMenu _gameMenu;

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
            //TODO: adjust scaling/fullscreen etc.  
            //var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1920, 1080);
            //var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 480, 270);
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 640, 360);

            _graphics.PreferredBackBufferWidth = viewportadapter.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = viewportadapter.GraphicsDevice.Adapter.CurrentDisplayMode.Height;

            //Debug
            _graphics.ToggleFullScreen();
            Window.IsBorderless = true;
            Window.AllowUserResizing = true;            

            //_graphics.HardwareModeSwitch = false;

            //TODO: Player/cam class
            _camera = new OrthographicCamera(viewportadapter);
         
            _screenLoader = new ScreenLoader(this, _screenManager, _graphics.GraphicsDevice);

            //TODO: Add Menu

            //Debug
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            _gameMenu = new GameMenu();

            base.Initialize();
        }

        /// <summary>
        /// Load Content
        /// </summary>
        protected override void LoadContent()
        {
            //use this.Content to load your game content here
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// Update Logic
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();          

            if (current_screen != "Menu" && _camera != null)
            {
                const float movementSpeed = 200;
                _camera.Move(GetMovementDirection() * movementSpeed * gameTime.GetElapsedSeconds());

                //Debug
                var mouseState = Mouse.GetState();
                _worldPosition = _camera.ScreenToWorld(new Vector2(mouseState.X, mouseState.Y));         

                var p = _gameMenu.panel1.Children[0] as Paragraph;
                {
                    p.Text = "x:" + mouseState.X + "\n" + "y:" + mouseState.Y + "\n" + "Tile: ";
                }             
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Drawing Logic
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {      
            if (current_screen != "Menu" && _tiledMapRenderer != null)
            {           
                const float movementSpeed = 200;
                _camera.Move(GetMovementDirection() * movementSpeed * gameTime.GetElapsedSeconds());

                GraphicsDevice.Clear(Color.White);
                _tiledMapRenderer.Draw(_camera.GetViewMatrix());

                UserInterface.Active.Draw(_spriteBatch);   

                /*
                 * Drawing with spritebatch
                var transformMatrix = _camera.GetViewMatrix();
                _spriteBatch.Begin(transformMatrix: transformMatrix);
                _spriteBatch.DrawRectangle(new RectangleF(250, 250, 50, 50), Color.Black, 1f);
                _spriteBatch.End();
                */
            }

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

        public void MoveCamera(GameTime gameTime)
        {
            var speed = 200;
            var seconds = gameTime.GetElapsedSeconds();
            var movementDirection = GetMovementDirection();
            _cameraPosition += speed * movementDirection * seconds;
        }
    }
}
