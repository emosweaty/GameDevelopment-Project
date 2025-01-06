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
        private static DisplayManager instance;

        private GraphicsDeviceManager graphics;

        private DisplayManager(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }

        public static DisplayManager GetInstance(GraphicsDeviceManager graphics)
        {
            if (instance == null)
            {
                instance = new DisplayManager(graphics);
            }
            return instance;
        }

        public void Apply()
        {
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        public int ScreenWidth => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public int ScreenHeight => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
    }
}
