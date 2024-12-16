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
    internal class EnemyManager 
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

        public void Update(GameTime gameTime, Vector2 heroposition)
        {
            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime, heroposition);
            }
        }

        public void Draw(SpriteBatch sprite)
        {
            foreach (var enemy in enemies)
            {
                enemy.Draw(sprite);
            }
        }


    }
}
