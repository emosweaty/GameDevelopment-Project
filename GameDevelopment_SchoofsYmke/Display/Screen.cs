using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Display
{
    abstract class Screen
    {
        protected Texture2D backgroundTexture;
        protected SpriteFont font;
        protected SpriteBatch spriteBatch;
        protected Texture2D button;

        public Screen(SpriteBatch spriteBatch, Texture2D backgroundTexture, Texture2D button, SpriteFont font)
        {
            this.spriteBatch = spriteBatch;
            this.backgroundTexture = backgroundTexture;
            this.font = font;
            this.button = button;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
