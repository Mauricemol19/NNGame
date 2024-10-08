using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;

namespace NNGame.Classes.Characters
{
    public class Player : Character
    {
        public SpriteSheet _spriteSheet;        
        public AnimatedSprite _animatedSprite;

        public Player(string SpriteName, Vector2 SpritePosition) : base(SpriteName, SpritePosition)
        {
            /*
            Texture2D adventurerTexture =  Content.Load<Texture2D>("adventurer");
            TextureAtlas atlas = TextureAtlas.Create("Atlas/adventurer", adventurerTexture, 50, 37);
            _spriteSheet = new SpriteSheet("SpriteSheet/adventurer", atlas);

            spriteSheet.DefineAnimation("attack", builder =>
            {
                builder.IsLooping(true)
                       .AddFrame(regionIndex: 0, duration: TimeSpan.FromSeconds(0.1))
                       .AddFrame(1, TimeSpan.FromSeconds(0.1))
                       .AddFrame(2, TimeSpan.FromSeconds(0.1))
                       .AddFrame(3, TimeSpan.FromSeconds(0.1))
                       .AddFrame(4, TimeSpan.FromSeconds(0.1))
                       .AddFrame(5, TimeSpan.FromSeconds(0.1));
            });

            spriteSheet.DefineAnimation("idle", builder =>
            {
                builder.IsLooping(true)
                       .AddFrame(6, TimeSpan.FromSeconds(0.1))
                       .AddFrame(7, TimeSpan.FromSeconds(0.1))
                       .AddFrame(8, TimeSpan.FromSeconds(0.1))
                       .AddFrame(9, TimeSpan.FromSeconds(0.1));
            });

            spriteSheet.DefineAnimation("run", builder =>
            {
                builder.IsLooping(true)
                       .AddFrame(10, TimeSpan.FromSeconds(0.1))
                       .AddFrame(11, TimeSpan.FromSeconds(0.1))
                       .AddFrame(12, TimeSpan.FromSeconds(0.1))
                       .AddFrame(13, TimeSpan.FromSeconds(0.1))
                       .AddFrame(14, TimeSpan.FromSeconds(0.1))
                       .AddFrame(15, TimeSpan.FromSeconds(0.1));
            });

            _animatedSprite = new AnimatedSprite(spriteSheet, "idle");
            */
        }
    }
}
