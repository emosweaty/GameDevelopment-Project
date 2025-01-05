using GameDevelopment_SchoofsYmke.Characters;
using GameDevelopment_SchoofsYmke.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Projectiles
{
    internal class ProjectileManager
    {
        private List<Projectile> projectiles = new List<Projectile>();
        private Texture2D projectileTexture;
        Hero hero;
        EnemyManager enemies;

        public void getEntities(Hero hero, EnemyManager enemies)
        {
            this.hero = hero;
            this.enemies = enemies;
        }

        public void AddProjectile(Vector2 startPosition, Vector2 initialVelocity, Texture2D texture, string ownerType)
        {
            projectiles.Add(new Projectile(startPosition, initialVelocity, texture, ownerType));
        }

        public void Update(GameTime gameTime ,Point mapSize, int screenHeight, CollisionManager collision)
        {
            foreach (var projectile in projectiles)
            {
                projectile.Update(gameTime, mapSize, screenHeight, collision);
                
                if (projectile.IsActive && projectile.OwnerType == "LoaderEnemy" && projectile.Bounds.Intersects(hero.Bounds))
                {
                    hero.TakeDamage(10);
                    projectile.IsActive = false;
                }

                // Check collision with enemies
                if (projectile.IsActive && projectile.OwnerType == "Hero")
                {
                    foreach (var enemy in enemies.Enemies)
                    {
                        if (projectile.Bounds.Intersects(enemy.Bounds))
                        {
                            enemy.TakeDamage(10);
                            projectile.IsActive = false;
                            break;
                        }
                    }


                }
            }

            projectiles.RemoveAll(p => !p.IsActive);
 
        }

        public void Draw(SpriteBatch sprite)
        {
            foreach (var projectile in projectiles)
            {
                projectile.Draw(sprite);

            }
        }
    }
}
