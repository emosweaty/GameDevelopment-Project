using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace GameDevelopment_SchoofsYmke.Display
{
    internal class WonScreen:Screen
    {
        private Rectangle startButtonBounds;
        private bool buttonClicked;

        public WonScreen(SpriteBatch spriteBatch, Texture2D backgroundTexture, Texture2D button, SpriteFont font)
            : base(spriteBatch, backgroundTexture, button, font)
        {
            int buttonWidth = 600;
            int buttonHeight = 200;
            int startX = (backgroundTexture.Width - buttonWidth) / 2;
            int startY = (backgroundTexture.Height / 2);

            startButtonBounds = new Rectangle(startX, startY, buttonWidth, buttonHeight);
            buttonClicked = false;
        }

        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();

            if (startButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
            {
                buttonClicked = true;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

            string winMessage = "You Won!";
            Vector2 winMessageSize = font.MeasureString(winMessage);
            Vector2 winMessagePosition = new Vector2((backgroundTexture.Width - winMessageSize.X) / 2, (backgroundTexture.Height / 4) - winMessageSize.Y);
            spriteBatch.DrawString(font, winMessage, winMessagePosition, Color.White);

            spriteBatch.Draw(button, startButtonBounds, Color.White);
            spriteBatch.DrawString(font, "Start Screen", new Vector2(startButtonBounds.X + (startButtonBounds.Width - font.MeasureString("Start Screen").X) / 2,
                startButtonBounds.Y + (startButtonBounds.Height - font.MeasureString("Start Screen").Y) / 2), Color.White);
        }

        public bool ShouldGoToStartScreen()
        {
            return buttonClicked;
        }
    }
}

