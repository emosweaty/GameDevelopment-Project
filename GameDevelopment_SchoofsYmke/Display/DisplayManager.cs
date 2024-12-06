using GameDevelopment_SchoofsYmke.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Display
{
    internal class DisplayManager
    {
        private GraphicsDeviceManager graphics;

        public DisplayManager(GraphicsDeviceManager graphics) 
        {
            this.graphics = graphics;
        }

        public void Apply()
        {
            graphics.PreferredBackBufferWidth = 2550;
            graphics.PreferredBackBufferHeight = 1500;

            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }

        public int ScreenWidth => 2550;
        public int ScreenHeight => 1500;
    }
}
