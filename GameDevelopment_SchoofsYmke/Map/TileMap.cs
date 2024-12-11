using GameDevelopment_SchoofsYmke.Blocks;
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
        private List<Tile> tiles;
        private List<DecoTile> decoTiles;
        private Texture2D tilesheet;
        private Texture2D decosheet;
        private readonly HashSet<int> solidTileValues = new() { 0, 1, 2, 3, 4, 6, 7, 8, 9, 10, 12, 16, 17, 18, 19, 21, 24, 25, 26, 28, 29, 35, 49};
        private readonly HashSet<int> solidDecoTileValues = new() { 2, 3, 4, 5, 6, 7, 21, 22, 23, 24, 25};
        public Point MapSize { get; private set; }

        public TileMap()
        {
            tiles = new List<Tile>();
            decoTiles = new List<DecoTile>();
        }

        public void LoadContent(ContentManager content, string tilesheetPath, string decosheetPath)
        {
            tilesheet = content.Load<Texture2D>(tilesheetPath);
            decosheet = content.Load<Texture2D>(decosheetPath);
        }

        public void LoadMap(string filepath, string decopath)
        {
            tiles.Clear();

            int maxWidth = 0;

            using StreamReader reader = new(filepath);
            int y = 0;
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                string[] item = line.Split(",");

                maxWidth = Math.Max(maxWidth, item.Length);

                for (int x = 0; x < item.Length; x++)
                {
                    if (int.TryParse(item[x], out int value))
                    {
                        if (value > -1)
                        {   
                            
                            var tileBounds = new Rectangle(x * 64, y * 64, 64, 64);
                            bool isSolid = solidTileValues.Contains(value);
                            tiles.Add(new Tile(value, tileBounds, isSolid));
                        }
                    }
                }     
                y++;
            }

            using StreamReader decoReader = new(decopath);
            y = 0;

            while ((line = decoReader.ReadLine()) != null)
            {
                string[] item = line.Split(",");

                for (int x = 0; x < item.Length; x++)
                {
                    if (int.TryParse(item[x], out int value))
                    {
                        if (value > -1) // Only add decorative tiles with valid values
                        {
                            var tileBounds = new Rectangle(x * 64, y * 64, 64, 64);
                            bool isSolid = solidDecoTileValues.Contains(value);
                            decoTiles.Add(new DecoTile(value, tileBounds, isSolid));
                        }
                    }
                }
                y++;

                MapSize = new Point(maxWidth * 64);
            }
        }

        public IEnumerable<ICollidable> GetCollidableObjects()
        {
            var collidables = tiles.Where(t => t.IsSolid).Cast<ICollidable>().ToList();
            collidables.AddRange(decoTiles.Where(d => d.IsSolid).Cast<ICollidable>());
            return collidables;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in tiles)
            { 
                int tilesPerRow = tilesheet.Width / 64;
                int tileX = (tile.TileId) % tilesPerRow;
                int tileY = (tile.TileId) / tilesPerRow;

                Rectangle source = new Rectangle(tileX * 64, tileY * 64, 64, 64);
                spriteBatch.Draw(tilesheet, tile.Bounds, source, Color.White);

                //Texture2D debugTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                //debugTexture.SetData(new[] { Color.Red });
                //spriteBatch.Draw(debugTexture, tile.Bounds, Color.Red * 0.5f);
            }
            
            foreach (var decoTile in decoTiles)
            {
                int tilesPerRow = decosheet.Width / 64;
                int tileX = (decoTile.TileId) % tilesPerRow;
                int tileY = (decoTile.TileId) / tilesPerRow;

                Rectangle source = new Rectangle(tileX * 64, tileY * 64, 64, 64);
                spriteBatch.Draw(decosheet, decoTile.Bounds, source, Color.White);
            }
        }
    }
}
