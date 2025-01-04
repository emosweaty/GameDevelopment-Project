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
        private Rectangle startButtonBounds;
        private bool startGame;
        public StartScreen(SpriteBatch spriteBatch, Texture2D backgroundTexture, Texture2D button, SpriteFont font)
            : base(spriteBatch, backgroundTexture, button, font)
        {
            int buttonWidth = 600;
            int buttonHeight = 200;
            int startX = (backgroundTexture.Width - buttonWidth) / 2;
            int startY = (backgroundTexture.Height - buttonHeight) / 2;

            startButtonBounds = new Rectangle(startX, startY, buttonWidth, buttonHeight);
            startGame = false;
        }

        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            if (startButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
            {
                startGame = true;
            }
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            spriteBatch.Draw(button, startButtonBounds, Color.White);
            spriteBatch.DrawString(font, "Start Game", new Vector2(startButtonBounds.X +250 , startButtonBounds.Y+100), Color.White);

        }
        public bool ShouldStartGame()
        {
            return startGame;
        }

    }
}

