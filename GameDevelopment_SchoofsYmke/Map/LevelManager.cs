using GameDevelopment_SchoofsYmke.Interfaces;
using Microsoft.Xna.Framework;
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

        public Point MapSize => currentLevel.MapSize;

        public void LoadLevel(ContentManager content, 
            string levelId, string mapFilePath,string decoFilePath, string tilesheetPath, string decosheetPath)
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
        }
    }
}
