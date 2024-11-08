using GameDevelopment_SchoofsYmke.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameDevelopment_SchoofsYmke
{
    internal class Hero : IGameObject
    {
        private Texture2D texture;
        Animatie animation;

        public Hero(Texture2D texture)
        {
            this.texture = texture;
            animation = new Animatie();
            animation.GetFramesFromTexturePropeties(texture.Width, texture.Height,8,1);
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(texture, new Vector2(0, 0), animation.CurrentFrame.SourceRectangle, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }
    }
}
