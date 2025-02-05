﻿using GameDevelopment_SchoofsYmke.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Blocks
{
    internal class DecoTile : ICollidable
    {
        public int TileId { get; set; }
        private Rectangle bounds;
        private bool isSolid;

        public DecoTile(int tileId, Rectangle bounds, bool isSolid)
        {
            TileId = tileId;
            this.bounds = bounds;
            this.isSolid = isSolid;
        }

        public Rectangle Bounds => bounds;

        public bool IsSolid => isSolid;

        public bool CollidesWith(ICollidable other)
        {
            return bounds.Intersects(other.Bounds);
        }
    }
}
