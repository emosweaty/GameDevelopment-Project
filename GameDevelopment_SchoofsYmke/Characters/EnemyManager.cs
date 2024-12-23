using GameDevelopment_SchoofsYmke.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Characters
{
    internal class EnemyManager : ICollidable
    {
        private List<Enemy> enemies;

        public EnemyManager()
        {
            enemies = new List<Enemy>();
        }

        public void InitializeEnemies(IEnumerable<(Texture2D texture, Vector2 position, float speed, float viewRange)> enemyConfig)
        {
            enemies.Clear();
            foreach (var config in enemyConfig)
            {
                var enemy = new Enemy(config.texture, config.position, config.speed, config.viewRange);
                enemies.Add(enemy);
            }
        }

        public void Update(GameTime gameTime, Vector2 heroposition, Hero hero)
        {
            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime, heroposition, hero);
            }
        }

        public void Draw(SpriteBatch sprite)
        {
            foreach (var enemy in enemies)
            {
                enemy.Draw(sprite);
            }
        }

        public IEnumerable<Rectangle> GetEnemyBounds()
        {
            return enemies.Select(enemy => enemy.Bounds);
        }

        public Rectangle Bounds
        {
            get
            {
                if (enemies.Count > 0)
                {
                    return enemies[0].Bounds;
                }
                return Rectangle.Empty;
            }
        }

        public bool IsSolid => true;

        public bool CollidesWith(ICollidable other)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.CollidesWith(other))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
