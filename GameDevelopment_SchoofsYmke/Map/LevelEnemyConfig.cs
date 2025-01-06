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
        public static Dictionary<string, List<(string texturePath, Vector2 position, float speed, float viewRange, string type)>> EnemyConfigurations =
            new Dictionary<string, List<(string, Vector2, float, float, string)>>()
            {
                {
                    "Level1", new List<(string, Vector2, float, float, string)>
                    {
                        ("Enemies/Loader", new Vector2(3800, 900), 80, 800, "loader"),
                        ("Enemies/Security", new Vector2(2100, 900), 80, 800, "security"),
                        ("Enemies/Operator", new Vector2(2500, 900), 80, 1000, "operator")
                    }
                }
            };

        public static List<(string texturePath, Vector2 position, float speed, float viewRange, string type)> GetConfig(string levelId)
        {
            return EnemyConfigurations.TryGetValue(levelId, out var config) ? config : new List<(string, Vector2, float, float, string)>();
        }
    }
}
