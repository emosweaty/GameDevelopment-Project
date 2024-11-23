using GameDevelopment_SchoofsYmke.Map;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Content
{
    internal class ContentLoader
    {
        private readonly ContentManager content;

        public ContentLoader(ContentManager content)
        {
            this.content = content;
        }

        public Texture2D LoadTexture(string assetName)
        {
            return content.Load<Texture2D>(assetName);
        }

        public TileMap LoadTileMap(string mapPath, string tilesheetPath)
        {
            var tileMap = new TileMap();
            tileMap.LoadMap(mapPath);
            tileMap.LoadContent(content, tilesheetPath);
            return tileMap;
        }
    }
}
