using GameDevelopment_SchoofsYmke.Characters;
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

        public ProjectileManager(Texture2D texture) 
        {
            projectileTexture = texture;
        }

        public void AddProjectile(Vector2 position, Vector2 direction, float speed)
        {
            projectiles.Add(new Projectile(position, direction * speed, projectileTexture));
        }

        public void Update(GameTime gameTime, Hero hero, Point mapSize, int screenHeight)
        {
            foreach (var projectile in projectiles)
            {
                projectile.Update(gameTime, mapSize, screenHeight);

                if (projectile.IsActive && projectile.Bounds.Intersects(hero.Bounds))
                {
                    hero.TakeDamage(10);
                    projectile.IsActive = false;
                }
            }

            projectiles.RemoveAll(p => !p.IsActive);
 
        }

        public void Draw(SpriteBatch sprite, Texture2D texture)
        {
            foreach (var projectile in projectiles)
            {
                projectile.Draw(sprite);
            }
        }
    }
}
