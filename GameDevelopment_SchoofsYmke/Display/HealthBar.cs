using GameDevelopment_SchoofsYmke.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Display
{
    internal class HealthBar
    {
        private Texture2D background;
        private Texture2D bar;
        private SpriteFont font;
        private Vector2 position;
        private int maxHealth;
        private int currenHealth;
        private Animatie animation;

        private float flashTimer;
        private float flashDuration;

        public HealthBar(Texture2D background, Texture2D bar, SpriteFont font, Vector2 position, int maxHealth)
        {
            this.background = background;
            this.bar = bar;
            this.font = font;
            this.position = position;
            this.maxHealth = maxHealth;
            currenHealth = maxHealth;

            flashDuration = 1.0f;

            animation = new Animatie();
            animation.GetFramesFromTexture(background.Width, background.Height, 4, 1, "healthbar");
            animation.SetAnimationState(AnimationState.Idle);
        }

        public void TakeDamage(int damage)
        {
            currenHealth = MathHelper.Clamp(currenHealth - damage, 0, maxHealth);
            flashTimer = flashDuration;
        }

        public void Update(GameTime gameTime)
        {
            if (flashTimer > 0)
            {
                flashTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                animation.SetAnimationState(AnimationState.Hit);
            }
            else
            {
                animation.SetAnimationState(AnimationState.Idle);
            }
        }

        public void Draw(SpriteBatch sprite)
        {
            float healthPercentage = (float)currenHealth / maxHealth;
            int filledSegments = (int)(healthPercentage * 7); 

            for (int i = 0; i < 7; i++)
            {
                if (i < filledSegments)
                {
                    var extra = 30 * i;
                    sprite.Draw(bar, position + new Vector2(205 + extra, 80), Color.White);
                }
            }
            sprite.Draw(background, position, animation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            string healthText = $"{(int)(healthPercentage * 100)}%";
            Vector2 textPosition = new Vector2(position.X + background.Width / 15, position.Y + background.Height / 2);
            Vector2 textOrigin = font.MeasureString(healthText) / 2;

            sprite.DrawString(font, healthText, textPosition, Color.White, 0, textOrigin, 2f, SpriteEffects.None, 0);
        }
    }
}
