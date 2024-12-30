using GameDevelopment_SchoofsYmke.Characters;
using GameDevelopment_SchoofsYmke.Display;
using GameDevelopment_SchoofsYmke.Interfaces;
using GameDevelopment_SchoofsYmke.Map;
using GameDevelopment_SchoofsYmke.Movement;
using GameDevelopment_SchoofsYmke.Projectiles;
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
        private Color color;

        private Texture2D screen;
        private Texture2D texture;
        private Texture2D enemyTexture;
        private Texture2D projectileTexture;
        private Texture2D healthBarTexture;

        private Hero hero;
        private DisplayManager display;
        private LevelManager level;
        private CollisionManager movement;
        private CameraManager camera;
        private EnemyManager enemy;
        private ProjectileManager projectile;

        //Voor debuggen (Bounds)
        private Texture2D blokTexture;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            display = new DisplayManager(_graphics);
            level = new LevelManager();
            camera = new CameraManager();
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
            blokTexture = new Texture2D(GraphicsDevice, 1, 1);
            blokTexture.SetData(new[] { Color.White });

            screen = Content.Load<Texture2D>("DeadScreen");
            healthBarTexture = Content.Load<Texture2D>("GUI/LifeBar");
            texture = Content.Load<Texture2D>("HeroSprite");
            enemyTexture = Content.Load<Texture2D>("Enemies/Loader");
            projectileTexture = Content.Load<Texture2D>("Enemies/Box");

            level.LoadLevel(Content, "Level1", "Content/Level1.txt", "Content/Level1-Deco.txt", "Tiles", "DecoTiles");

            var collidables = level.Currentlevel.GetCollidableObjects().ToList();

            hero = new Hero(texture);
            projectile = new ProjectileManager(projectileTexture);
            enemy = new EnemyManager(projectile);
            
            movement = new CollisionManager(new List<ICollidable>(collidables){ hero, enemy }, level.MapSize, display.ScreenHeight);

            color = new Color(50, 25, 51, 255);

            hero.SetMovementManager(movement);

            var enemyConfigs = LevelEnemyConfig.GetConfig("Level1");
            enemy.InitializeEnemies(enemyConfigs.Select(config =>
                (enemyTexture, config.position, config.speed, config.viewRange)));

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
            projectile.Update(gameTime, hero,level.MapSize, display.ScreenHeight, movement );
            enemy.Update(gameTime, hero, movement);
            camera.CalculateTranslation(hero, display.ScreenWidth, display.ScreenHeight, level.MapSize);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(color);

            _spriteBatch.Begin(transformMatrix: camera.getTranslation());
            level.Currentlevel?.Draw(_spriteBatch);
            enemy.Draw(_spriteBatch);
            projectile.Draw(_spriteBatch, projectileTexture);
            hero.Draw(_spriteBatch);
            //Voor Debuggen (bounds)
            //Rectangle heroBounds = hero.Bounds;

            foreach (var enemyBounds in enemy.GetEnemyBounds())
            {
                _spriteBatch.Draw(blokTexture, enemyBounds, Color.Red * 0.5f); // Draw translucent red rectangle
            }

            // _spriteBatch.Draw(blokTexture, enemyBounds, Color.Red * 0.5f);

            if (movement.IsDead)
            {
                _spriteBatch.Draw(screen, new Vector2(0, 0), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        
    }
}
