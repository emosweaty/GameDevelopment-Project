using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Display
{
    internal class StartScreen : Screen
    {
        private Rectangle level1ButtonBounds;
        private Rectangle level2ButtonBounds;
        private bool level1Selected;
        private bool level2Selected;

        public StartScreen(SpriteBatch spriteBatch, Texture2D backgroundTexture, Texture2D button, SpriteFont font)
            : base(spriteBatch, backgroundTexture, button, font)
        {
            int buttonWidth = 600;
            int buttonHeight = 200;

            int startX = (backgroundTexture.Width - buttonWidth) / 2;

            int level1Y = (backgroundTexture.Height - buttonHeight) / 2;
            level1ButtonBounds = new Rectangle(startX, level1Y, buttonWidth, buttonHeight);

            int level2Y = level1Y + buttonHeight + 20; 
            level2ButtonBounds = new Rectangle(startX, level2Y, buttonWidth, buttonHeight);

            level1Selected = false;
            level2Selected = false;
        }

        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();

            if (level1ButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
            {
                level1Selected = true;
            }

            if (level2ButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
            {
                level2Selected = true;
            }
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

            spriteBatch.Draw(button, level1ButtonBounds, Color.White);
            spriteBatch.DrawString(font, "Level 1", new Vector2(level1ButtonBounds.X + (level1ButtonBounds.Width - font.MeasureString("Level 1").X) / 2,
                level1ButtonBounds.Y + (level1ButtonBounds.Height - font.MeasureString("Level 1").Y) / 2), Color.White);

            spriteBatch.Draw(button, level2ButtonBounds, Color.White);
            spriteBatch.DrawString(font, "Level 2", new Vector2(level2ButtonBounds.X + (level2ButtonBounds.Width - font.MeasureString("Level 2").X) / 2,
                level2ButtonBounds.Y + (level2ButtonBounds.Height - font.MeasureString("Level 2").Y) / 2), Color.White);

        }
        public bool ShouldStartLevel1()
        {
            return level1Selected;
        }

        public bool ShouldStartLevel2()
        {
            return level2Selected;
        }

    }
}

