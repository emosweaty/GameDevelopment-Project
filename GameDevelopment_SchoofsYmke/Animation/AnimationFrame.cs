using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Animation
{
    public class AnimationFrame
    {
        public Rectangle SourceRectangle { get; set; }
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;

        public AnimationFrame(Rectangle sourceRectangle)
        {
            SourceRectangle = sourceRectangle;
        }
    }
}
