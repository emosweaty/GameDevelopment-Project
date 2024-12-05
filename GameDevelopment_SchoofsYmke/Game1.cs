using GameDevelopment_SchoofsYmke.Blocks;
using GameDevelopment_SchoofsYmke.Display;
using GameDevelopment_SchoofsYmke.Interfaces;
using GameDevelopment_SchoofsYmke.Map;
using GameDevelopment_SchoofsYmke.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GameDevelopment_SchoofsYmke
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D texture;
        private Hero hero;

        private DisplayManager display;
        private LevelManager level;

        //Voor debuggen (Bounds)
        //private Texture2D blokTexture;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            display = new DisplayManager(_graphics);
            level = new LevelManager();
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            //hero = new Hero(texture, null);

            display.Apply();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Voor debuggen (Bounds)
            //blokTexture = new Texture2D(GraphicsDevice, 1, 1);
            //blokTexture.SetData(new[] { Color.White });


            texture = Content.Load<Texture2D>("Sprite_CharacterBIG");
            level.LoadLevel(Content, "Level1", "Content/Map.txt", "TileSheet");

            var collidables = level.Currentlevel.GetCollidableObjects().ToList();

            hero = new Hero(texture);
            var movement = new CollisionManager(new List<ICollidable>(collidables){ hero });

            hero.SetMovementManager(movement);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            hero.Update(gameTime);
            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            level.Currentlevel?.Draw(_spriteBatch);
            hero.Draw(_spriteBatch);
            
            //Voor Debuggen (bounds)
            //Rectangle heroBounds = hero.Bounds;
            //_spriteBatch.Draw(blokTexture, heroBounds, Color.Red * 0.5f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        
    }
}
