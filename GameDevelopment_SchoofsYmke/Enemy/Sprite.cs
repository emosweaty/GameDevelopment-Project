using GameDevelopment_SchoofsYmke.Animation;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Enemy
{
    internal class Sprite
    {
        private Texture2D texture;
        private Animatie animation;

        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;

        public Sprite(Texture2D texture, Animatie animation)
        {
            this.texture = texture;
            this.animation = animation;
        }

        public void SetAnimationState(AnimationState state)
        {
            animation.SetAnimationState(state);
        }

        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 positionOffset = default)
        {
            spriteBatch.Draw(
                texture,
                position + positionOffset,
                animation.CurrentFrame.SourceRectangle,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffect,
                0f
            );
        }
    }
}
