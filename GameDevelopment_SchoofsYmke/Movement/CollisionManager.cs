using GameDevelopment_SchoofsYmke.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Movement
{
    internal class CollisionManager
    {
        public readonly List<ICollidable> collidables;
        public bool IsDead { get; set; } = false;

        private readonly int screenHeight;
        private readonly Point screenWidth;
        public CollisionManager(List<ICollidable> collidables, Point screenWidth, int screenHeight)
        {
            this.collidables = collidables ?? new List<ICollidable>();
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        public Vector2 CalculateNewPosition(Hero hero, Vector2 direction)
        {
            Vector2 newPosition = hero.location + direction;
            Vector2 spawnPoint = new Vector2(0,900);

            Rectangle newBounds = new Rectangle(
                (int)(newPosition.X), (int)(newPosition.Y),
                hero.Bounds.Width, hero.Bounds.Height);
             

            foreach (var collidable in collidables)
            {
                if (!collidable.IsSolid)
                    continue;

                Rectangle collidableBounds = collidable.Bounds;

                if (newBounds.Intersects(collidable.Bounds))
                {
                    if (newBounds.Bottom >= collidableBounds.Top &&
                        hero.Bounds.Bottom <= collidableBounds.Top + 20 && 
                        newBounds.Right > collidableBounds.Left &&
                        newBounds.Left < collidableBounds.Right)
                    {
                        direction.Y = 0;
                        newPosition.Y = collidableBounds.Top - hero.Bounds.Height;
                    }

                    if (newBounds.Top < collidableBounds.Bottom &&
                        hero.Bounds.Top >= collidableBounds.Bottom &&
                        newBounds.Right > collidableBounds.Left &&
                        newBounds.Left < collidableBounds.Right)
                    {
                        direction.Y = 0;
                        newPosition.Y = collidableBounds.Bottom;
                    }

                    if (newBounds.Right > collidableBounds.Left &&
                        hero.Bounds.Right <= collidableBounds.Left &&
                        newBounds.Bottom > collidableBounds.Top + 28)
                    {
                        direction.X = 0;
                        newPosition.X = collidableBounds.Left - hero.Bounds.Width;
                    }
                    else if (newBounds.Left < collidableBounds.Right &&
                             hero.Bounds.Left >= collidableBounds.Right &&
                             newBounds.Bottom > collidableBounds.Top + 20)
                    {
                        direction.X = 0;
                        newPosition.X = collidableBounds.Right;
                    }


                    if (newPosition.X < 0)
                    {
                        newPosition.X = 0;
                        direction.X = 0;
                    }
                    else if (newPosition.X + hero.Bounds.Width > screenWidth.X)
                    {
                        newPosition = new Vector2(spawnPoint.X, spawnPoint.Y);
                        direction.X = 0;
                    }

                    if (newPosition.Y + hero.Bounds.Height >= screenHeight)
                    {
                        newPosition = new Vector2(spawnPoint.X, spawnPoint.Y);
                        direction = Vector2.Zero;
                        IsDead = true;
                    }
                }
            }
            hero.location = newPosition;
            return direction;
        }

        public bool IsOnGround(Hero hero)
        {
            var heroBounds = hero.Bounds;

            foreach (var obj in collidables)
            {
                if (!obj.IsSolid) continue;

                var objectBounds = obj.Bounds;

                if (heroBounds.Bottom >= objectBounds.Top -5 &&
                heroBounds.Bottom <= objectBounds.Top +5 &&
                heroBounds.Right > objectBounds.Left &&  
                heroBounds.Left < objectBounds.Right)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
