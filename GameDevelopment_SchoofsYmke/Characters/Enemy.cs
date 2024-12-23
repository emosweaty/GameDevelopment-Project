using GameDevelopment_SchoofsYmke.Animation;
using GameDevelopment_SchoofsYmke.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace GameDevelopment_SchoofsYmke.Characters
{
    internal class Enemy : ICollidable
    {
        private Animatie animation;
        private Texture2D texture;
        private Vector2 position;
        private Vector2 direction;
        private float viewRange;
        private float speed;
        private float lastDirectionX = 1;
        private double elapsedTimeCounter = 0; 
        private double attackDuration = 50;

        public Rectangle Bounds => new Rectangle((int)position.X + 60, (int)position.Y+80, 120, 140);

        public bool IsSolid => true;

        public Enemy(Texture2D texture, Vector2 initialPosition, float speed, float viewRange)
        {
            this.texture = texture;
            position = initialPosition;
            this.speed = speed;
            this.viewRange = viewRange;

            animation = new Animatie();
            animation.GetFramesFromTexture(texture.Width, texture.Height, 6, 9, "loader");
            animation.SetAnimationState(AnimationState.Idle);
        }

        public void Update(GameTime gameTime, Vector2 heroPosition, Hero hero)
        {
            float distanceToHero = Vector2.Distance(heroPosition, position);
            elapsedTimeCounter--;

            if (distanceToHero <= viewRange)
            {
                var target = heroPosition.X;
                direction.X = Math.Sign(heroPosition.X - position.X);
                
                if (Bounds.Intersects(hero.Bounds))
                {

                    direction.X = 0;

                    if(elapsedTimeCounter <= 0)
                    {
                        elapsedTimeCounter = attackDuration;

                    }
                    else if (elapsedTimeCounter <= 20 )
                    {
                        animation.SetAnimationState(AnimationState.Idle);

                    }
                    else if (elapsedTimeCounter > 20)
                    {
                        animation.SetAnimationState(AnimationState.Attack1);
                    }

                }
                else
                {
                    animation.SetAnimationState(AnimationState.Moving);
                    position.X = MathHelper.Lerp(position.X, target, 0.015f);

                }
            }
            else
            {
                animation.SetAnimationState(AnimationState.Idle);

            }
            animation.Update(gameTime);

            if (direction.X != 0)
            {
                lastDirectionX = direction.X;
            }

            if (lastDirectionX > 0){animation.CurrentFrame.SpriteEffect = SpriteEffects.None;}
            else if (lastDirectionX < 0){animation.CurrentFrame.SpriteEffect = SpriteEffects.FlipHorizontally;}

        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(texture, position, animation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f, animation.CurrentFrame.SpriteEffect, 0f);
        }

        public bool CollidesWith(ICollidable other)
        {
            return Bounds.Intersects(other.Bounds);
        }
    }

}
