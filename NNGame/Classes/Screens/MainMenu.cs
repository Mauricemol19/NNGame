using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace NNGame.Classes.Screens
{
    /// <summary>
    /// Main Menu
    /// </summary>
    public class MainMenu : GameScreen
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        private Main main => (Main)base.Game;

        private Texture2D _logo;
        private SpriteFont _font;
        private Vector2 _position = new(50, 50);

        public MainMenu(Main main) : base(main) { }

        public override void LoadContent()
        {
            //base.LoadContent();
            //_font = main.Content.Load<SpriteFont>("Fonts/font");
            //_logo = main.Content.Load<Texture2D>("Sprites/test");
        }

        public override void Update(GameTime gameTime)
        {
            //_position = Vector2.Lerp(_position, Mouse.GetState().Position.ToVector2(), 1f * gameTime.GetElapsedSeconds());
        }

        public override void Draw(GameTime gameTime)
        {
            /*
            main.GraphicsDevice.Clear(new Color(16, 139, 204));
            main._spriteBatch.Begin();
            main._spriteBatch.DrawString(_font, nameof(Menu), new Vector2(10, 10), Color.White);
            main._spriteBatch.Draw(_logo, _position, Color.White);
            main._spriteBatch.End();      
            */
        }
    }
}
