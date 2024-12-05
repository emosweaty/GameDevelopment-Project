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

        public CollisionManager(List<ICollidable> collidables)
        {
            this.collidables = collidables ?? new List<ICollidable>();
        }

        public Vector2 CalculateNewPosition(Hero hero, Vector2 direction)
        {
            Vector2 newPosition = hero.location + direction;

            Rectangle newBounds = new Rectangle(
                (int)(newPosition.X), (int)(newPosition.Y),
                hero.Bounds.Width, hero.Bounds.Height);


            foreach (var collidable in collidables)
            {
                if (!collidable.IsSolid)
                    continue;

                if (newBounds.Intersects(collidable.Bounds))
                {
                    if (newBounds.Bottom > collidable.Bounds.Top && hero.Bounds.Bottom <= collidable.Bounds.Top)
                    {
                        direction.Y = 0;
                        newPosition.Y = collidable.Bounds.Top - hero.Bounds.Height;
                    }
                    else if (newBounds.Top < collidable.Bounds.Bottom && hero.Bounds.Top >= collidable.Bounds.Bottom)
                    {
                        direction.Y = 0;
                        newPosition.Y = collidable.Bounds.Bottom;
                    }

                    if (newBounds.Right > collidable.Bounds.Left && hero.Bounds.Right <= collidable.Bounds.Left)
                    {
                        direction.X = 0; 
                        newPosition.X = collidable.Bounds.Left - hero.Bounds.Width;
                    }
                    else if (newBounds.Left < collidable.Bounds.Right && hero.Bounds.Left >= collidable.Bounds.Right)
                    {
                        direction.X = 0;
                        newPosition.X = collidable.Bounds.Right;
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

                if (heroBounds.Bottom >= objectBounds.Top &&
                heroBounds.Bottom <= objectBounds.Top &&
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
