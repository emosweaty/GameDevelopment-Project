using GameDevelopment_SchoofsYmke.Characters;
using GameDevelopment_SchoofsYmke.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Map
{
    internal class LevelManager
    {
        private readonly Dictionary<string, TileMap> levels;
        private readonly Dictionary<string, List<(string texturePath, float depth, float moveScale)>> levelBackgroundConfigs;

        private TileMap currentLevel;
        private List<ICollidable> collidables;
        private EnemyManager enemy;

        public LevelManager()
        {
            levels = new Dictionary<string, TileMap>();
            enemy = new EnemyManager();
            collidables = new List<ICollidable>();
        }

        public TileMap Currentlevel => currentLevel;
        public IEnumerable<ICollidable> Collidables => collidables;
        public Point MapSize => currentLevel.MapSize;


        public void LoadLevel(ContentManager content,
            string levelId, string mapFilePath, string decoFilePath, string tilesheetPath, string decosheetPath)
        {
            if (!levels.ContainsKey(levelId))
            {
                var tileMap = new TileMap();
                tileMap.LoadContent(content, tilesheetPath, decosheetPath);
                tileMap.LoadMap(mapFilePath, decoFilePath);
                levels[levelId] = tileMap;
            }
            currentLevel = levels[levelId];
            collidables = currentLevel.GetCollidableObjects().ToList();

            var enemyConfig = LevelEnemyConfig.GetConfig(levelId);
            var initializedEnemies = new List<(Texture2D, Vector2, float, float)>();

            foreach (var (texturePath, position, speed, viewRange) in enemyConfig)
            {
                Texture2D texture = content.Load<Texture2D>(texturePath);
                initializedEnemies.Add((texture, position, speed, viewRange));
            }

            enemy.InitializeEnemies(initializedEnemies);
        }

    }
}
