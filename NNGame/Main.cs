using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
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

namespace NNGame
{
    /// <summary>
    /// Main file inherited from Game
    /// </summary>
    public class Main : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

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

        private bool wDown, aDown, sDown, dDown = false;

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
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 640, 360);

            //_graphics.PreferredBackBufferWidth = viewportadapter.GraphicsDevice.Adapter.CurrentDisplayMode.Width;           
            //_graphics.PreferredBackBufferHeight = viewportadapter.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            //_graphics.PreferredBackBufferWidth = 1920;
            //_graphics.PreferredBackBufferHeight = 1080;

            //TODO: Borderless
            //_graphics.ToggleFullScreen();

            //Window.IsBorderless = true;
            Window.AllowUserResizing = true;            

            //TODO: Neccessary?
            _graphics.HardwareModeSwitch = false;
            
            //TODO: Multiple different custom cams (player, free, cutscene etc.)
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
            }

            //Load spritefont
            try
            {               
                _tileTextFont = this.Content.Load<SpriteFont>("Fonts/font");               
            }
            catch 
            {
                Debug.WriteLine("LoadContent(): Unable to Load spritefont", "Error");
            }

            _playerChar = new("Sprites/test", Vector2.Zero);       

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
            _tileTextPosition = new Vector2(10, 70);
        }

        /// <summary>
        /// Drawing Logic
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            //Todo: change init of function and sorting           
            if (_current_screen != "Menu" && _tiledMapRenderer != null)
            {
                //Clear GraphicsDevice 
                GraphicsDevice.Clear(Color.CornflowerBlue);
                GraphicsDevice.Clear(Color.Black);                

                //Draw tiledmap
                _tiledMapRenderer.Draw(_camera.GetViewMatrix());

                //Draw GUI
                UserInterface.Active.Draw(_spriteBatch);                                           
                        
                _spriteBatch.Begin();

                //Draw player character
                if (_playerChar != null)
                    _spriteBatch.Draw(_playerChar._spriteTexture, _playerChar.SpritePosition, Color.White);

                //Draw debug playerlocation
                if (_playerChar != null)
                    _spriteBatch.DrawString(_tileTextFont, "X: " + _playerChar.SpritePosition.X + " Y:" + _playerChar.SpritePosition.Y, _tileTextPosition, Color.Yellow, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);                

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
                Vector2 movementDirection = GetMovementDirection();

                _camera.Move(movementDirection, gameTime.GetElapsedSeconds());

                MouseState ms = Mouse.GetState();
                Vector2 wp_xy = _camera.GetWXY(ms, _worldPosition);

                //Debug Draw sprite near mouse
                if (wp_xy.X >= 0 && wp_xy.Y >= 0 && wp_xy.X <= _tiledMap.WidthInPixels && wp_xy.Y <= _tiledMap.HeightInPixels)
                {
                    UpdateTileText((int)wp_xy.X, (int)wp_xy.Y);

                    GetTileXYAtPoint((int)wp_xy.X, (int)wp_xy.Y, out int tileX, out int tileY);

                    TiledMapTile tile = _tiledMap.TileLayers[0].GetTile((ushort)tileX, (ushort)tileY);

                    _selectedTile = tile;

                    //Move player near mouse
                    if (_playerChar != null)
                        _playerChar.SpritePosition = new Vector2(wp_xy.X, wp_xy.Y);
                }
            } 
            else 
            {
                //TODO: MainMenu Camera
                //_camera.Move(GetMovementDirection(), gameTime.GetElapsedSeconds());
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Gets Vector2 of movement direction based on user input
        /// TODO: Keybindings
        /// </summary>
        /// <returns></returns>
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

            //Can't normalize zero vector
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

        //*****//
        //Debug//
        //*****//    
        private void UpdateTileText(int x, int y)
        {
            if (ContainsXY(x, y))
            {
                GetTileXYAtPoint(x, y, out int tileX, out int tileY);

                var tile = _tiledMap.TileLayers[0].GetTile((ushort)tileX, (ushort)tileY);

                //Show debug hovered over tile
                //_tileText = $"In tilesheet: {tileX}, {tileY} TileType: [{GetTileText(_tiledMap, tile.GlobalIdentifier)}]";

                //Update ingame UI with on tilesheet x and y
                var p = _gameMenu.panel1.Children[0] as Paragraph;
                {
                    p.Text = "x:" + tileX + " y:" + tileY + "\n" + "TileType: [" + GetTileText(_tiledMap, tile.GlobalIdentifier) + "]";
                }
            }
        }

        private bool ContainsXY(int x, int y)
        {
            int tileX = x / 32;
            int tileY = y / 32;

            return (tileX >= 0) && (tileX < _tiledMap.Width) && (tileY >= 0) && (tileY < _tiledMap.Height);
        }

        private void GetTileXYAtPoint(int x, int y, out int tileX, out int tileY)
        {
            tileX = x / 32;
            tileY = y / 32;
        }

        private string GetTileText(TiledMap map, int id)
        {
            foreach (var tileSet in map.Tilesets)
            {
                int firstGid = map.GetTilesetFirstGlobalIdentifier(tileSet);

                if ((id >= firstGid) && (id < firstGid + tileSet.TileCount))
                    return $"{tileSet.Name}: {id - firstGid}";
            }

            return "Unknown";
        }        
    }
}
