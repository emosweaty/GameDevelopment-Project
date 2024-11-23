using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Map
{
    internal class TileMap
    {
        private Dictionary<Vector2, int> tilemap;
        private Texture2D tilesheet;
        

        public TileMap()
        {
            tilemap = new Dictionary<Vector2, int>(); 
        }

        public void LoadContent(ContentManager content, string tilesheetPath) 
        {    
                tilesheet = content.Load<Texture2D>(tilesheetPath);  
        }



        public void LoadMap(string filepath)
        {
            tilemap = new Dictionary<Vector2, int>();
            using StreamReader reader = new(filepath);

            int y = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] item = line.Split(",");

                for (int x = 0; x < item.Length; x++)
                {
                    if (int.TryParse(item[x], out int value))
                    {
                        if (value > 0)
                        {
                            tilemap[new Vector2(x, y)] = value;
                        }
                    }
                }
                y++;
            }
            
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in tilemap)
            {
                Vector2 position = item.Key;
                int tileId = item.Value;

                int tilesPerRow = tilesheet.Width / 64;
                int tileX = (tileId - 1) % tilesPerRow;
                int tileY = (tileId - 1) / tilesPerRow;

                Rectangle source = new Rectangle(tileX * 64, tileY * 64, 64, 64);
                Rectangle destination = new Rectangle((int)position.X * 64, (int)position.Y * 64, 64, 64);

                spriteBatch.Draw(tilesheet, destination, source , Color.White);
            }

        }
    }
}
