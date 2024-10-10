using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace NNGame.Classes.Characters
{    
    public class Character
    {
        public string SpriteName { get; set; }

        public Vector2 SpritePosition { get; set; }

        public Texture2D _spriteTexture;

        public AnimatedSprite _animatedSprite;

        public Character(string spriteName, Vector2 spritePosition) 
        { 
            SpriteName = spriteName;
            SpritePosition = spritePosition;
        }
    }
}
