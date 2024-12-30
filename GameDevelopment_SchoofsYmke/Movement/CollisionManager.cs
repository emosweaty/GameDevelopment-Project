using GameDevelopment_SchoofsYmke.Characters;
using GameDevelopment_SchoofsYmke.Enemy;
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

        public Vector2 CalculateNewPosition(ICollidable entity, Vector2 direction)
        {
            Rectangle dynamicBounds = entity.Bounds;
            Vector2 newPosition = new Vector2(dynamicBounds.X, dynamicBounds.Y) + direction;
            Vector2 spawnPoint = new Vector2(0, 900);

            Rectangle newBounds = new Rectangle(
                (int)(newPosition.X), (int)(newPosition.Y),
                entity.Bounds.Width, entity.Bounds.Height);


            foreach (var collidable in collidables)
            {
                if (!collidable.IsSolid)
                    continue;

                Rectangle collidableBounds = collidable.Bounds;

                if (newBounds.Intersects(collidable.Bounds))
                {
                    if (newBounds.Bottom >= collidableBounds.Top &&
                        entity.Bounds.Bottom <= collidableBounds.Top + 20 &&
                        newBounds.Right > collidableBounds.Left &&
                        newBounds.Left < collidableBounds.Right)
                    {
                        direction.Y = 0;
                        newPosition.Y = collidableBounds.Top - entity.Bounds.Height;
                    }

                    if (newBounds.Top < collidableBounds.Bottom &&
                        entity.Bounds.Top >= collidableBounds.Bottom &&
                        newBounds.Right > collidableBounds.Left &&
                        newBounds.Left < collidableBounds.Right)
                    {
                        direction.Y = 0;
                        newPosition.Y = collidableBounds.Bottom;
                    }

                    if (newBounds.Right > collidableBounds.Left &&
                        entity.Bounds.Right <= collidableBounds.Left &&
                        newBounds.Bottom > collidableBounds.Top + 28)
                    {
                        direction.X = 0;
                        newPosition.X = collidableBounds.Left - entity.Bounds.Width;
                    }
                    else if (newBounds.Left < collidableBounds.Right &&
                             entity.Bounds.Left >= collidableBounds.Right &&
                             newBounds.Bottom > collidableBounds.Top + 28)
                    {
                        direction.X = 0;
                        newPosition.X = collidableBounds.Right;
                        Debug.WriteLine($"Collision Detected: Entity={entity.GetType().Name}, " +
                    $"Bounds={newBounds}, Collidable={collidable.GetType().Name}, " +
                    $"Collidable Bounds={collidableBounds}");
                    }


                    if (newPosition.X < 0)
                    {
                        newPosition.X = 0;
                        direction.X = 0;
                    }
                    else if (newPosition.X + entity.Bounds.Width > screenWidth.X)
                    {
                        newPosition.X = screenWidth.X - entity.Bounds.Width; 
                        direction.X = 0;                                    
                    }

                    if (newPosition.Y + entity.Bounds.Height >= screenHeight)
                    {
                        newPosition = new Vector2(spawnPoint.X, spawnPoint.Y);
                        direction = Vector2.Zero;
                        IsDead = true;
                    }
                }
                
            }

            if (entity is Hero hero) hero.location = newPosition;
           // Debug.WriteLine($"Entity: {entity.GetType().Name}, NewPosition: {newPosition}, Direction: {direction}");

            return direction;
            }

        public bool IsOnGround(ICollidable entity)
        {
            var heroBounds = entity.Bounds;

            foreach (var obj in collidables)
            {
                if (!obj.IsSolid) continue;

                var objectBounds = obj.Bounds;

                if (heroBounds.Bottom >= objectBounds.Top - 5 &&
                heroBounds.Bottom <= objectBounds.Top + 5 &&
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

