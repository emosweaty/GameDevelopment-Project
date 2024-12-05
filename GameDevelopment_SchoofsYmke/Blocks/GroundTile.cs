using GameDevelopment_SchoofsYmke.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Blocks
{
    internal class GroundTile : ICollidable
    {
        public Rectangle Bounds { get; private set; }
        public bool IsSolid => true;

        public bool CollidesWith(ICollidable other)
        {
            return Bounds.Intersects(other.Bounds);
        }
    }
}
