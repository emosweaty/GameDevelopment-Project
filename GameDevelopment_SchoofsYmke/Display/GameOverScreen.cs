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
    internal class GameOverScreen : Screen
    {
        private Rectangle restartButtonBounds;
        private Rectangle exitButtonBounds;
        private bool restartGame;
        private bool exitGame;

        public GameOverScreen(SpriteBatch spriteBatch, Texture2D backgroundTexture, Texture2D button, SpriteFont font)
            : base(spriteBatch, backgroundTexture, button, font)
        {
            int buttonWidth = 600;
            int buttonHeight = 200;

            // Centered position for the buttons
            int startX = (backgroundTexture.Width - buttonWidth) / 2;
            int startY = (backgroundTexture.Height / 3);

            restartButtonBounds = new Rectangle(startX, startY, buttonWidth, buttonHeight);
            exitButtonBounds = new Rectangle(startX, startY + buttonHeight + 10, buttonWidth, buttonHeight);

            restartGame = false;
            exitGame = false;
        }

        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();

            // Detect click on Restart button
            if (restartButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
            {
                restartGame = true;
            }

            // Detect click on Exit button
            if (exitButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
            {
                exitGame = true;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

            // Draw Restart button
            spriteBatch.Draw(button, restartButtonBounds, Color.White);
            spriteBatch.DrawString(font, "Restart", new Vector2(restartButtonBounds.X + (restartButtonBounds.Width - font.MeasureString("Restart").X) / 2,
                restartButtonBounds.Y + (restartButtonBounds.Height - font.MeasureString("Restart").Y) / 2), Color.White);

            // Draw Exit button
            spriteBatch.Draw(button, exitButtonBounds, Color.White);
            spriteBatch.DrawString(font, "Exit", new Vector2(exitButtonBounds.X + (exitButtonBounds.Width - font.MeasureString("Exit").X) / 2,
                exitButtonBounds.Y + (exitButtonBounds.Height - font.MeasureString("Exit").Y) / 2), Color.White);
        }

        public bool ShouldRestartGame()
        {
            return restartGame;
        }

        public bool ShouldExitGame()
        {
            return exitGame;
        }
    }
}
