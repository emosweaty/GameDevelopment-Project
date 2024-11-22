using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Blocks
{
    internal class BlockFactory
    {
        public static Block CreateBlock(
             int x, int y, GraphicsDevice graphics)
        {
            //type nog toevoegen
            Block newBlock = null;
            newBlock = new Block(x, y, graphics);
            return newBlock;
        }
    }
}
