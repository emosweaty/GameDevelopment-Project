using GameDevelopment_SchoofsYmke.Interfaces;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Map
{
    internal class LevelManager
    {
        private readonly Dictionary<string, TileMap> levels;
        private TileMap currentLevel;
        private List<ICollidable> collidables;

        public LevelManager()
        {
            levels = new Dictionary<string, TileMap>();
            collidables = new List<ICollidable>();
        }

        public TileMap Currentlevel => currentLevel;
        public IEnumerable<ICollidable> Collidables => collidables;

        public void LoadLevel(ContentManager content, 
            string levelId, string mapFilePath,string tilesheetPath)
        {
            if (!levels.ContainsKey(levelId))
            {
                var tileMap = new TileMap();
                tileMap.LoadContent(content, tilesheetPath);
                tileMap.LoadMap(mapFilePath);
                levels[levelId] = tileMap;
            }
            currentLevel = levels[levelId];
            collidables = currentLevel.GetCollidableObjects().ToList();
        }
    }
}
