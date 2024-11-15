using GameDevelopment_SchoofsYmke.Animation;
using GameDevelopment_SchoofsYmke.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace GameDevelopment_SchoofsYmke
{
    internal class Hero : IGameObject
    {
        private Texture2D texture;
        Animatie animation;
        private Vector2 location;
        private Vector2 speed;
        private Vector2 speedup;
        private IInputReader inputReader;


        public Hero(Texture2D texture, IInputReader inputReader)
        {
            this.texture = texture;
            this.inputReader = inputReader;
            animation = new Animatie();
            animation.GetFramesFromTexture(texture.Width, texture.Height,8,2);

            location = new Vector2(10, 10);
            speed = new Vector2(5, 1);
            speedup = new Vector2(0.1f, 0.1f);
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(texture, location, animation.CurrentFrame.SourceRectangle, Color.White);

        
        }

        public void Update(GameTime gameTime)
        {         
            Move(); 
            animation.Update(gameTime);
        }

        private Vector2 Limit(Vector2 v, float max)
        {
            if (v.Length() > max)
            {
                var ratio = max / v.Length();
                v.X *= ratio;
                v.Y *= ratio;
            }
            return v;
        }

        private void Move()
        {
            var direction = inputReader.ReadInput();
            direction *= speed;
            location += direction;

        }

    }
}
