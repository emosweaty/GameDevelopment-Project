using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Interfaces
{
    internal interface ICollidable
    {
        Rectangle Bounds { get; }
        bool IsSolid { get; }
        
        bool CollidesWith(ICollidable other);
    }
}
