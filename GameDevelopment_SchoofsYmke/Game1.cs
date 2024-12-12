using GameDevelopment_SchoofsYmke.Characters;
using GameDevelopment_SchoofsYmke.Display;
using GameDevelopment_SchoofsYmke.Interfaces;
using GameDevelopment_SchoofsYmke.Map;
using GameDevelopment_SchoofsYmke.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace GameDevelopment_SchoofsYmke
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D texture;
        private Texture2D enemyTexture;
        private Texture2D screen;
        private Color color;

        private Hero hero;
        private Enemy enemy;
        
        private DisplayManager display;
        private LevelManager level;
        private CollisionManager movement;
        private CameraManager camera;

        //Voor debuggen (Bounds)
        //private Texture2D blokTexture;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            display = new DisplayManager(_graphics);
            level = new LevelManager();
            camera = new CameraManager();

           // bgm.AddLayer(new(Content.Load<Texture2D>("1"), 0, 0f, 0.0f));
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

            screen = Content.Load<Texture2D>("DeadScreen");
            texture = Content.Load<Texture2D>("HeroSprite");
            enemyTexture = Content.Load<Texture2D>("Spider");
            level.LoadLevel(Content, "Level1", "Content/Level1.txt", "Content/Level1-Deco.txt", "Tiles", "DecoTiles");

            var collidables = level.Currentlevel.GetCollidableObjects().ToList();

            hero = new Hero(texture);
            enemy = new Enemy(enemyTexture, new Vector2(300, 400), 100);

            int screenWidth = display.ScreenWidth;
            int screenHeight = display.ScreenHeight;
            movement = new CollisionManager(new List<ICollidable>(collidables){ hero }, level.MapSize, display.ScreenHeight);

            color = new Color(50, 25, 51, 255);

            hero.SetMovementManager(movement);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (movement.IsDead)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.E))
                {
                    movement.IsDead = false;
                }
            }
            
            hero.Update(gameTime);
            enemy.Update(gameTime);
            camera.CalculateTranslation(hero, display.ScreenWidth, display.ScreenHeight, level.MapSize);
            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(color);

            _spriteBatch.Begin(transformMatrix: camera.getTranslation());
            level.Currentlevel?.Draw(_spriteBatch);
            hero.Draw(_spriteBatch);
            enemy.Draw(_spriteBatch);

            //Voor Debuggen (bounds)
            //Rectangle heroBounds = hero.Bounds;
            //_spriteBatch.Draw(blokTexture, heroBounds, Color.Red * 0.5f);

            if (movement.IsDead)
            {
                _spriteBatch.Draw(screen, new Vector2(0, 0), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        
    }
}
