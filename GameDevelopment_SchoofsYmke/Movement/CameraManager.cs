using GameDevelopment_SchoofsYmke.Display;
using GameDevelopment_SchoofsYmke.Character;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Movement
{
    internal class CameraManager
    {
        private Matrix translation;
        private DisplayManager display;
        
        public void CalculateTranslation(Hero hero,int screenWidth, int screenHeight, Point mapSize)
        {
            var dx = (screenWidth / 2) - hero.location.X;
            dx = MathHelper.Clamp(dx, -mapSize.X + screenWidth + (64 / 2), 64 / 2);
            var dy = (screenHeight / 2) - hero.location.Y;
            dy = MathHelper.Clamp(dy, -mapSize.Y + screenHeight + (64 / 2), 64 / 2);
            translation = Matrix.CreateTranslation(dx, dy + 350, 0f);
        }

       public Matrix getTranslation()
        {
            return translation;
        }
    }
}
