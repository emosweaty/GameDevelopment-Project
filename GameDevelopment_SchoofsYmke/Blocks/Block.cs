using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Blocks
{
    internal class Block : IGameObject
    {
        public Rectangle BoundingBox { get; set; }
        public bool Passable { get; set; }
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        

        public Block(int x, int y, GraphicsDevice graphics)
        {
            BoundingBox = new Rectangle(x, y, 10, 10);
            Passable = false;
            Color = Color.Pink;
            Texture = new Texture2D(graphics, 1, 1);
        }
        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(Texture, BoundingBox, Color);
        }

        public void Update(GameTime gameTime) { }
       
    }
}
