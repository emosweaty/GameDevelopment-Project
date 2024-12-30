using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameDevelopment_SchoofsYmke.Map
{
    internal class LevelEnemyConfig
    {
        public static Dictionary<string, List<(string texturePath, Vector2 position, float speed, float viewRange)>> EnemyConfigurations =
            new Dictionary<string, List<(string, Vector2, float, float)>>()
            {
                {
                    "Level1", new List<(string, Vector2, float, float)>
                    {
                        ("Enemies/Loader", new Vector2(3800, 900), 100, 800)
                    }
                }
            };

        public static List<(string texturePath, Vector2 position, float speed, float viewRange)> GetConfig(string levelId)
        {
            return EnemyConfigurations.TryGetValue(levelId, out var config) ? config : new List<(string, Vector2, float, float)>();
        }
    }
}
