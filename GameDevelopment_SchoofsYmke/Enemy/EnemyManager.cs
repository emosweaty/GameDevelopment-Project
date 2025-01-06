using GameDevelopment_SchoofsYmke.Blocks;
using GameDevelopment_SchoofsYmke.Character;
using GameDevelopment_SchoofsYmke.Enemy;
using GameDevelopment_SchoofsYmke.Interfaces;
using GameDevelopment_SchoofsYmke.Movement;
using GameDevelopment_SchoofsYmke.Projectiles;
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
        private List<BaseEnemy> enemies;
        private ProjectileManager projectile;
        private Texture2D projectileTexture;

        public IEnumerable<BaseEnemy> Enemies => enemies;

        public EnemyManager(ProjectileManager projectile, Texture2D projectileTexture)
        {
            enemies = new List<BaseEnemy>();
            this.projectile = projectile;
            this.projectileTexture = projectileTexture;
        }

        public void InitializeEnemies(IEnumerable<(Texture2D texture, Vector2 position, float speed, float viewRange, string type)> enemyConfig)
        {
            enemies.Clear();
            foreach (var config in enemyConfig)
            {
                BaseEnemy enemy;
                switch (config.type)
                {
                    case "loader":
                        enemy = new LoaderEnemy(config.texture, projectileTexture, config.position, config.speed, config.viewRange, projectile);
                        break;
                    case "security":
                        enemy = new SecurityEnemy(config.texture, projectileTexture, config.position, config.speed, config.viewRange, projectile);
                        break;
                    case "operator":
                        enemy = new OperatorEnemy(config.texture ,projectileTexture, config.position, config.speed, config.viewRange, projectile);
                        break;
                    default:
                        throw new ArgumentException($"Unknown enemy type: {config.type}");
                }
                enemies.Add(enemy);
            }
        }
        public void Reset()
        {
            foreach (var enemy in enemies)
            {
                enemy.Reset();
            }
        }
        public void Update(GameTime gameTime, Hero hero, CollisionManager collision)
        {
            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime, hero, collision, projectile);
            }
        }

        public void Draw(SpriteBatch sprite)
        {
            foreach (var enemy in enemies)
            {
                enemy.Draw(sprite);
            }
        }

        public bool AreAllEnemiesDead()
        {
            return enemies.All(e => !e.isAlive);
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

        public bool IsSolid
        {
            get
            {
                foreach (var enemy in enemies)
                {
                    return enemy.IsSolid;
                }
                return false;
            }
        }
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
