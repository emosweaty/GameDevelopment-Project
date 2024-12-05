using GameDevelopment_SchoofsYmke.Interfaces;
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
        private  List<Tile> tiles;
        private Texture2D tilesheet;

        public TileMap()
        {
            tiles = new List<Tile>();
        }

        public void LoadContent(ContentManager content, string tilesheetPath)
        {
            tilesheet = content.Load<Texture2D>(tilesheetPath);
        }

        public void LoadMap(string filepath)
        {
            tiles.Clear();

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
                            
                            var tileBounds = new Rectangle(x * 400, y * 400, 200, 200);
                            bool isSolid = value == 2;
                            tiles.Add(new Tile(value, tileBounds, isSolid));
                        }
                    }
                }     
                y++;
            }
            
        }

        public IEnumerable<ICollidable> GetCollidableObjects()
        {
            return tiles.Where(tiles => tiles.IsSolid)
                .Cast<ICollidable>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in tiles)
            { 
                int tilesPerRow = tilesheet.Width / 200;
                int tileX = (tile.TileId - 1) % tilesPerRow;
                int tileY = (tile.TileId - 1) / tilesPerRow;

                Rectangle source = new Rectangle(tileX * 200, tileY * 400, 200, 200);
                spriteBatch.Draw(tilesheet, tile.Bounds, source, Color.White);
            }

        }
    }
}
