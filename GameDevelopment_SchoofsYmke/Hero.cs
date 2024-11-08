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
            animation.AddFrame(new AnimationFrame(new Rectangle(0, 0, 180, 240)));
            animation.AddFrame(new AnimationFrame(new Rectangle(180, 0, 180, 240)));
            animation.AddFrame(new AnimationFrame(new Rectangle(360, 0, 180, 240)));
            animation.AddFrame(new AnimationFrame(new Rectangle(540, 0, 180, 240)));
            animation.AddFrame(new AnimationFrame(new Rectangle(720, 0, 180, 240)));
            animation.AddFrame(new AnimationFrame(new Rectangle(900, 0, 180, 240)));
            animation.AddFrame(new AnimationFrame(new Rectangle(1080, 0, 180, 240)));
            animation.AddFrame(new AnimationFrame(new Rectangle(1260, 0, 180, 240)));

        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(texture, new Vector2(0, 0), animation.CurrentFrame.SourceRectangle, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            animation.Update();
        }
    }
}
