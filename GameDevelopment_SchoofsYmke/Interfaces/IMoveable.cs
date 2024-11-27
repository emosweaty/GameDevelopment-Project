using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Interfaces
{
    public interface IMoveable
    {
        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }
        public Vector2 Gravity { get; set; }
        public Vector2 Jump { get; set; }
        public Vector2 Velocity { get; set; }
        public bool OnGround { get; set; }
        public IInputReader InputReader { get; set; }
    }
}
