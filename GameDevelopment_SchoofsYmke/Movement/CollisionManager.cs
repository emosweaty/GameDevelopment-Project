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
        private readonly List<ICollidable> collidables;

        public CollisionManager(List<ICollidable> collidables)
        {
            this.collidables = collidables ?? new List<ICollidable>();
        }

        public Vector2 CalculateNewPosition(Vector2 currentPosition, Vector2 direction, ICollidable movingObject)
        {
            Vector2 newPosition = currentPosition + direction;

            Rectangle newBounds = new Rectangle(
                (int)(newPosition.X), (int)(newPosition.Y),
                movingObject.Bounds.Width, movingObject.Bounds.Height);  


            foreach (var collidable in collidables)
            {
                if (newBounds.Intersects(collidable.Bounds))
                {
                    if (newBounds.Right > collidable.Bounds.Left && currentPosition.X < collidable.Bounds.Left)
                    {
                        direction.X = 0;
                    }
                    if (newBounds.Left < collidable.Bounds.Right && currentPosition.X > collidable.Bounds.Right)
                    {
                        direction.X = 0;
                    }
                    if (newBounds.Bottom > collidable.Bounds.Top && currentPosition.Y < collidable.Bounds.Top)
                    {
                        direction.Y = 0;
                    }
                    if (newBounds.Top < collidable.Bounds.Bottom && currentPosition.Y > collidable.Bounds.Bottom)
                    {
                        direction.Y = 0;
                    }
                }
            }
            return direction;
        }
    }
}
