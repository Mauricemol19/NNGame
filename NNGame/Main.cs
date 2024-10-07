using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Screens;
using NNGame.Classes;
using NNGame.Classes.Gui;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using NNGame.Classes.Cameras;
using NNGame.Classes.Characters;
using System.Diagnostics;
using MonoGame.Extended;
using Steamworks;
using System;

namespace NNGame
{
    /// <summary>
    /// Main file inherited from Game
    /// </summary>
    public class Main : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

        public SteamworksManager _steamworksManager;

        public TiledMap _tiledMap;
        public TiledMapRenderer _tiledMapRenderer;

        private SpriteFont _tileTextFont;
        private string _tileText;
        private Vector2 _tileTextPosition;

        public PlayerCamera _camera;
        public Vector2 _cameraPosition;

        public Vector2 _worldPosition;

        private readonly ScreenManager _screenManager;

        private ScreenLoader _screenLoader;

        public TiledMapTile _selectedTile;
        public TiledMapTile _playerLocation;

        public string _current_screen = "Menu";

        public GameMenu _gameMenu;

        public Character _playerChar;

        //private bool wDown, aDown, sDown, dDown = false;

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
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 640, 360);

            Window.AllowUserResizing = true;

            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            _graphics.ToggleFullScreen();
            //Window.IsBorderless = true;
            
            //Soft mode
            _graphics.HardwareModeSwitch = false;

            _graphics.ApplyChanges();

            //Steamworks
            _steamworksManager = new SteamworksManager();

            if (!SteamAPI.Init())
            {
                Console.WriteLine("SteamAPI.Init() failed!");
                Exit();
            }
            else
            {
                //Steam is running
                _steamworksManager.IsSteamRunning = true;

                //It's important that the next call happens AFTER the call to SteamAPI.Init().
                _steamworksManager.Initialize(this);
            }            

            _camera = new PlayerCamera(viewportadapter);
                  
            _screenLoader = new ScreenLoader(this, _screenManager, GraphicsDevice);

            //init GUI
            UserInterface.Initialize(this.Content, BuiltinThemes.hd);

            //init Main Menu
            _gameMenu = new GameMenu();

            base.Initialize();
        }

        /// <summary>
        /// Load Content at startup
        /// </summary>
        protected override void LoadContent()
        {
            //Load the spritebatch
            try
            {
                _spriteBatch = new SpriteBatch(GraphicsDevice);
            }
            catch
            {
                Debug.WriteLine("LoadContent(): Unable to Load spritebatch", "Error");
                Exit();
            }

            //Load spritefont
            try
            {               
                _tileTextFont = this.Content.Load<SpriteFont>("Fonts/font");               
            }
            catch 
            {
                Debug.WriteLine("LoadContent(): Unable to Load spritefont", "Error");
                Exit();
            }

            _playerChar = new("Sprites/test", new Vector2(400, 400));       

            //Load player sprite
            try
            {
                _playerChar._spriteTexture = this.Content.Load<Texture2D>(_playerChar.SpriteName);
            }
            catch
            {
                Debug.WriteLine("Unable to load character sprite");
                Exit();
            }            

            _tileText = "";
            _tileTextPosition = new Vector2(-5, -80);
        }       

        /// <summary>
        /// Draw every tick
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            if (_current_screen != "Menu" && _tiledMapRenderer != null)
            {
                //Clear GraphicsDevice 
                GraphicsDevice.Clear(Color.CornflowerBlue);
                GraphicsDevice.Clear(Color.Black);

                var viewMatrix = _camera.GetViewMatrix();
                var transformMatrix = viewMatrix;

                //Draw tiledmap
                _tiledMapRenderer.Draw(viewMatrix);
                //_tiledMapRenderer.Draw(_tiledMap.GetLayer("Floor"), _camera.GetViewMatrix());
                //_tiledMapRenderer.Draw(_tiledMap.GetLayer("Objects"), _camera.GetViewMatrix());

                //Draw GUI
                UserInterface.Active.Draw(_spriteBatch);                                 
                                        
                //Open spritebatch with ref to transformMatrix for scaling
                _spriteBatch.Begin(transformMatrix: transformMatrix);

                //Draw player character
                if (_playerChar != null)
                    _spriteBatch.Draw(_playerChar._spriteTexture, _playerChar.SpritePosition, null, Color.White, 0.0f, Vector2.Zero, 0.08f, SpriteEffects.None, 1);

                //Draw debug playerlocation
                if (_playerChar != null)
                    //_spriteBatch.DrawString(_tileTextFont, _tileText, _tileTextPosition, Color.Blue, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

                if (_steamworksManager.IsSteamRunning)
                {
                    //Draw your Steam Avatar and Steam Name
                    if (_steamworksManager.UserAvatar != null)
                    {                                                        
                        var avatarPosition = new Vector2(GraphicsDevice.Viewport.Width / 2f,
                            GraphicsDevice.Viewport.Height / 2f + (!_steamworksManager.SteamOverlayActive ? _steamworksManager.MoveUpAndDown(gameTime, 2).Y * 25 : 0));
                        _spriteBatch.Draw(_steamworksManager.UserAvatar, avatarPosition, null, Color.White, 0f,
                            new Vector2(_steamworksManager.UserAvatar.Width / 2f, _steamworksManager.UserAvatar.Height / 2f), 1f, SpriteEffects.None, 0f);
                        _spriteBatch.DrawString(_tileTextFont, _steamworksManager.SteamUserName,
                            new Vector2(avatarPosition.X - _tileTextFont.MeasureString(_steamworksManager.SteamUserName).X / 2f,
                                avatarPosition.Y - _steamworksManager.UserAvatar.Height / 2f - _tileTextFont.MeasureString(_steamworksManager.SteamUserName).Y * 1.5f),
                            Color.Yellow);
                    }

                    // Draw data up/left.
                    _spriteBatch.DrawString(_tileTextFont,
                        $"{_steamworksManager.CurrentLanguage}\n{_steamworksManager.AvailableLanguages}\n{_steamworksManager.InstallDir}\n\nOverlay Active: {_steamworksManager.SteamOverlayActive}\nApp PlayTime: {_steamworksManager.PlayTimeInSeconds()}",
                        new Vector2(20, 20), Color.White);

                    // Draw data down/left.
                    _spriteBatch.DrawString(_tileTextFont, $"{_steamworksManager.NumberOfCurrentPlayers}\n{_steamworksManager.PersonaState}\n{_steamworksManager.UserStats}\n{_steamworksManager.LeaderboardData}",
                        new Vector2(20, 375), Color.White);
                }
                else
                {
                    _spriteBatch.DrawString(_tileTextFont, _steamworksManager.STEAM_NOT_RUNNING_ERROR_MESSAGE,
                        new Vector2(GraphicsDevice.Viewport.Width / 2f - _tileTextFont.MeasureString(_steamworksManager.STEAM_NOT_RUNNING_ERROR_MESSAGE).X / 2f,
                            GraphicsDevice.Viewport.Height / 2f - _tileTextFont.MeasureString(_steamworksManager.STEAM_NOT_RUNNING_ERROR_MESSAGE).Y / 2f), Color.White);
                }

                _spriteBatch.End();               

                base.Draw(gameTime);
            }
            else
            {
                //Draw Main Menu
            }
        }       

        /// <summary>
        /// Update Logic every tick
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))          
                Exit();        

            if (_current_screen != "Menu" && _camera != null)
            {                     
                MouseState ms = Mouse.GetState();
                Vector2 wp_xy = _camera.GetWXY(ms, _worldPosition);                
      
                Vector2 movementDirection = GetMovementDirection();           

                //Set player spriteposition and lock camera to player
                if (_playerChar != null)
                {
                    _playerChar.SpritePosition += movementDirection;

                    //TODO: change to dynamic offset of sprite
                    _camera.Move(_playerChar.SpritePosition - new Vector2(-20, -40));           
                }

                //**DEBUG**//
                if (wp_xy.X >= 0 && wp_xy.Y >= 0 && wp_xy.X <= _tiledMap.WidthInPixels && wp_xy.Y <= _tiledMap.HeightInPixels)
                {
                    UpdateTileText((int)wp_xy.X, (int)wp_xy.Y, ms);

                    _camera.GetTileXYAtPoint((int)wp_xy.X, (int)wp_xy.Y, out int tileX, out int tileY);

                    TiledMapTile tile = _tiledMap.TileLayers[0].GetTile((ushort)tileX, (ushort)tileY);

                    _selectedTile = tile;
                }
                //**DEBUG**//
            }
            else 
            {
                //TODO: MainMenu Camera
                //_camera.Move(GetMovementDirection(), gameTime.GetElapsedSeconds());
            }

            //if (_steamworksManager.IsSteamRunning) 
                //SteamAPI.RunCallbacks();

            base.Update(gameTime);
        }

        /// <summary>
        /// Gets Vector2 of movement direction based on user input
        /// </summary>
        /// <returns></returns>
        private static Vector2 GetMovementDirection()
        {
            //Handle inputs
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

            //Zero vector is not normalizable
            if (movementDirection != Vector2.Zero)
            {
                movementDirection.Normalize();
            }

            return movementDirection;

            /*
            if (state.IsKeyDown(Keys.W))
            {
                if (!wDown)
                {
                    movementDirection -= Vector2.UnitY;

                    wDown = true;
                }                
            }
            if (state.IsKeyUp(Keys.W))
            {
                wDown = false;
            }            

            if (state.IsKeyDown(Keys.A))
            {
                if (!aDown)
                {
                    movementDirection -= Vector2.UnitX;

                    aDown = true;
                }
            }
            if (state.IsKeyUp(Keys.A))
            {
                aDown = false;
            }

            if (state.IsKeyDown(Keys.S))
            {
                if (!sDown)
                {
                    movementDirection += Vector2.UnitY;

                    sDown = true;
                }
            }
            if (state.IsKeyUp(Keys.S))
            {
                sDown = false;
            }

            if (state.IsKeyDown(Keys.D))
            {
                if (!dDown)
                {
                    movementDirection += Vector2.UnitX;

                    dDown = true;
                }
            }
            if (state.IsKeyUp(Keys.D))
            {
                dDown = false;
            }
            */
        }

        private void UpdateTileText(int x, int y, MouseState ms)
        {
            if (ContainsXY(x, y))
            {
                //**DEBUG**//
                _camera.GetTileXYAtPoint(x, y, out int tileX, out int tileY);

                var tile = _tiledMap.TileLayers[0].GetTile((ushort)tileX, (ushort)tileY);

                Vector2 wp_xy = new(ms.X, ms.Y);

                var p = _gameMenu.panel1.Children[0] as Paragraph;
                {
                    p.Text = $"Player pos: x:{_playerChar.SpritePosition.X} , y:{_playerChar.SpritePosition.Y}\n";
                    p.Text += $"Mouse tile hoverx:{tileX} y:{tileY} TileType: [" + _camera.GetTileText(_tiledMap, tile.GlobalIdentifier) + "]\n";
                    p.Text += $"Mouse pos in screen: x:{wp_xy.X} y:{wp_xy.Y}";
                }
                //**DEBUG**//
            }
        }

        private bool ContainsXY(int x, int y)
        {
            int tileX = x / 32;
            int tileY = y / 32;

            return (tileX >= 0) && (tileX < _tiledMap.Width) && (tileY >= 0) && (tileY < _tiledMap.Height);
        }
    }
}
