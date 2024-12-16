using GameDevelopment_SchoofsYmke.Animation;
using GameDevelopment_SchoofsYmke.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Characters
{
    internal class Enemy 
    {
        private Animatie animation;
        private Texture2D texture;
        private Vector2 position;
        private Vector2 direction;
        private float viewRange;
        private float speed;
        private int attackCooldown= 20; 
        public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, 64, 64);

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

        public void Update(GameTime gameTime, Vector2 heroPosition)
        {
            float distanceToHero = Vector2.Distance(heroPosition, position);


            if (distanceToHero <= viewRange)
            {
                direction.X = Math.Sign(heroPosition.X - position.X);

                if (direction.X > 0) { animation.CurrentFrame.SpriteEffect = SpriteEffects.None; }
                else if(direction.X < 0) { animation.CurrentFrame.SpriteEffect = SpriteEffects.FlipHorizontally; }

                if (distanceToHero > 200)
                {

                    animation.SetAnimationState(AnimationState.Moving);
                    position.X += direction.X * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    direction.X = 0;

                    if (attackCooldown ==  20 )
                    {
                        animation.SetAnimationState(AnimationState.Attack1);
                        attackCooldown = 0;
                    }
                    //else
                    //{
                    //    animation.SetAnimationState(AnimationState.Idle);
                    //}
                    



                }
            }   
            else
            {
                animation.SetAnimationState(AnimationState.Idle);
            }

            //if (attackCooldown > 5)
            //{
            //    attackCooldown += 1;
            //    Debug.WriteLine($"Attack Cooldown: {attackCooldown:F2} seconds");
            //}

            while (attackCooldown > 20)
            {
                attackCooldown++;
                    Debug.WriteLine($"Attack Cooldown: {attackCooldown:F2} seconds");
                
            }

            animation.Update(gameTime);
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(texture, position, animation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f, animation.CurrentFrame.SpriteEffect, 0f);
        }


        //private void UpdateCooldown(GameTime gameTime)
        //{
        //    if (attackCooldown > 0.0)
        //    {
        //        attackCooldown -= gameTime.ElapsedGameTime.TotalSeconds; // Count down
        //    }
        //}

        private void counter()
        {
            while (attackCooldown > 5)
            {
                attackCooldown++;
                Debug.WriteLine($"Attack Cooldown: {attackCooldown:F2} seconds");
            }
         }

            //private bool CanAttack()
            //{
            //    if (attackCooldown <= 0.0)
            //    {
            //        return true; // Attack is ready
            //    }

            //    return false; // Cooldown in progress
            //}
        }
}
