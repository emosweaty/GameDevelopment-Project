﻿using GameDevelopment_SchoofsYmke.Interfaces;
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
        private readonly int screenWidth;
        public CollisionManager(List<ICollidable> collidables, int screenWidth, int screenHeight)
        {
            this.collidables = collidables ?? new List<ICollidable>();
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        public Vector2 CalculateNewPosition(Hero hero, Vector2 direction)
        {
            Vector2 newPosition = hero.location + direction;
            Vector2 spawnPoint = new Vector2(0,890);

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


                    if (newPosition.X < 0)
                    {
                        newPosition.X = 0;
                        direction.X = 0;
                    }
                    else if (newPosition.X + hero.Bounds.Width > screenWidth)
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
