using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNGame.Classes.Characters
{    
    public class Character
    {
        public string _spriteName { get; set; }

        public Vector2 _spritePosition { get; set; }

        public Texture2D _spriteTexture;

        public Character(string spriteName, Vector2 spritePosition) 
        { 
            _spriteName = spriteName;
            _spritePosition = spritePosition;
        }
    }
}
