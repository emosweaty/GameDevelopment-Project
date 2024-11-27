using GameDevelopment_SchoofsYmke.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Blocks
{
    internal class SolidBlock : ICollidable
    {
        public Rectangle Bounds { get; set; }
        public bool IsSolid { get; set; }

        public SolidBlock(Rectangle bounds)
        {
            Bounds = bounds;
        }

        public bool CollidesWith(ICollidable other)
        {
            return Bounds.Intersects(other.Bounds);
        }
    }
}
