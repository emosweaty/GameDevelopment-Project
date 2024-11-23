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
        private readonly List<Rectangle> solidTiles;

        public TileMap()
        {
            tilemap = new Dictionary<Vector2, int>();
            solidTiles = new List<Rectangle>();
        }

        public void LoadContent(ContentManager content, string tilesheetPath) 
        {    
                tilesheet = content.Load<Texture2D>(tilesheetPath);  
        }

        public void LoadMap(string filepath)
        {
            tilemap.Clear();
            solidTiles.Clear();

            using StreamReader reader = new(filepath);
            int y = 0;
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                string[] item = line.Split(",");

                for (int x = 0; x < item.Length; x++)
                {
                    if (int.TryParse(item[x], out int tileId))
                    {
                        if (tileId > 0)
                        {
                            tilemap[new Vector2(x, y)] = tileId;

                            if (IsSolid(tileId))
                            {
                                solidTiles.Add(new Rectangle(x * 128, y * 128, 128, 128));
                            }
                        }
                    }
                }
                y++;
            }
            
        }

        private bool IsSolid(int titleId)
        {
            return titleId == 2;
        }

        public bool IsTitleSolid(Rectangle boundingBox)
        {
            foreach (var solidTile in solidTiles)
            {
                if (solidTile.Intersects(boundingBox))
                {
                    return true;
                }
            }
                return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in tilemap)
            {
                Vector2 position = item.Key;
                int tileId = item.Value;

                int tilesPerRow = tilesheet.Width / 128;
                int tileX = (tileId - 1) % tilesPerRow;
                int tileY = (tileId - 1) / tilesPerRow;

                Rectangle source = new Rectangle(tileX * 128, tileY * 128, 128, 128);
                Rectangle destination = new Rectangle((int)position.X * 400, (int)position.Y * 400, 128, 128);

                spriteBatch.Draw(tilesheet, destination, source , Color.White);
            }

        }
    }
}
