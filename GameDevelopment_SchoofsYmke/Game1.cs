﻿using GameDevelopment_SchoofsYmke.Character;
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
        private StartScreen startScreen;
        private GameOverScreen gameOverScreen;
        private WonScreen wonScreen;

        private Texture2D screen;
        private Texture2D texture;
        private Texture2D projectileTexture;
        private Texture2D healthBgTexture;
        private Texture2D healthBarTexture;
        private Texture2D arrowTexture;
        private Texture2D button;

        private SpriteFont font;

        private Hero hero;
        private DisplayManager display;
        private LevelManager level;
        private CollisionManager movement;
        private CameraManager camera;
        private EnemyManager enemy;
        private ProjectileManager projectile;
        private HealthBar healthBar;

        private bool canHeroMove;
        private bool gameOverFlag;
        private bool WonFlag;
        private string currentLevel;

        //Voor debuggen (Bounds)
        //private Texture2D blokTexture;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            display = DisplayManager.GetInstance(_graphics);
            level = new LevelManager();
            camera = new CameraManager();
        }

        protected override void Initialize()
        {
            base.Initialize();

            display.Apply();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Voor debuggen (Bounds)
            //blokTexture = new Texture2D(GraphicsDevice, 1, 1);
            //blokTexture.SetData(new[] { Color.White });

            font = Content.Load<SpriteFont>("GUI/CyberFont");

            screen = Content.Load<Texture2D>("GUI/StartScreen");
  
            button = Content.Load<Texture2D>("GUI/Button");
            healthBgTexture = Content.Load<Texture2D>("GUI/LifeBar");
            healthBarTexture = Content.Load<Texture2D>("GUI/Health");

            texture = Content.Load<Texture2D>("HeroSprite");
            projectileTexture = Content.Load<Texture2D>("Enemies/Box");
            arrowTexture = Content.Load<Texture2D>("Hero/Arrow");


            startScreen = new StartScreen(_spriteBatch, screen, button, font);
            gameOverScreen = new GameOverScreen(_spriteBatch, screen, button, font);
            wonScreen = new WonScreen(_spriteBatch, screen, button, font);

            canHeroMove = false;
            gameOverFlag = false;
            WonFlag = false;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (startScreen != null)
            {
                startScreen.Update(gameTime);
                if (startScreen.ShouldStartLevel1())
                {
                    StartGame("Level1");
                }
                else if (startScreen.ShouldStartLevel2())
                {
                    StartGame("Level2");
                }
            }
            else
            {
                if (hero.Health <= 0 && !gameOverFlag || movement.IsDead)
                {
                    gameOverFlag = true;
                }

                if (enemy.AreAllEnemiesDead() && !WonFlag)
                {
                    WonFlag = true;
                }
                if (WonFlag)
                {
                    wonScreen.Update(gameTime);

                    if (wonScreen.ShouldGoToStartScreen())
                    {
                        GoToStartScreen();
                        RestartGame();
                    }
                }

                if (gameOverFlag)
                    {
                    gameOverScreen.Update(gameTime);

                    if (gameOverScreen.ShouldRestartGame())
                    {
                        RestartGame();
                    }
                    if (gameOverScreen.ShouldExitGame())
                    {
                        Exit();
                    }
                }
                else
                {
                    if (canHeroMove)
                    {
                        hero.Update(gameTime);
                    }

                    projectile.Update(gameTime, level.MapSize, display.ScreenHeight, movement);
                    enemy.Update(gameTime, hero, movement);
                    camera.CalculateTranslation(hero, display.ScreenWidth, display.ScreenHeight, level.MapSize);
                    healthBar.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(color);

            _spriteBatch.Begin(transformMatrix: camera.getTranslation());
            level.Currentlevel?.Draw(_spriteBatch);
            enemy?.Draw(_spriteBatch);
            projectile?.Draw(_spriteBatch);
            hero?.Draw(_spriteBatch);
            //Voor Debuggen (bounds)
            //Rectangle heroBounds = hero.Bounds;

            //foreach (var enemyBounds in enemy.GetEnemyBounds())
            //{
            //    _spriteBatch.Draw(blokTexture, enemyBounds, Color.Red * 0.5f);
            //}

            // _spriteBatch.Draw(blokTexture, enemyBounds, Color.Red * 0.5f);

            //if (movement.IsDead)
            //{
            //    _spriteBatch.Draw(screen, new Vector2(0, 0), Color.White);
            //}

            _spriteBatch.End();

            _spriteBatch.Begin();
            healthBar?.Draw(_spriteBatch);
            if (startScreen != null && !gameOverFlag)
            {
                startScreen.Draw(gameTime);
            }
            else if (gameOverFlag)
            {
                gameOverScreen.Draw(gameTime);
            }
           else if (WonFlag)
            {
                wonScreen.Draw(gameTime);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        private void StartGame(string levelName)
        {
            startScreen = null;
            canHeroMove = true;
            currentLevel = levelName;

            if (currentLevel == "Level1")
            {
                level.LoadLevel(Content, "Level1", "Content/Level1.txt", "Content/Level1-Deco.txt", "Tiles", "DecoTiles");
            }
            else if (currentLevel == "Level2")
            {
                level.LoadLevel(Content, "Level2", "Content/Level1.txt", "Content/Level1-Deco.txt", "Tiles", "DecoTiles");
            }

            var collidables = level.Currentlevel.GetCollidableObjects().ToList();

            healthBar = new HealthBar(healthBgTexture, healthBarTexture, font, new Vector2(20, 50), 100);
            projectile = new ProjectileManager();

            enemy = new EnemyManager(projectile, projectileTexture);
            hero = new Hero(texture, arrowTexture, projectile, healthBar, enemy, level.MapSize);

            projectile.getEntities(hero, enemy);

            movement = new CollisionManager(new List<ICollidable>(collidables) { hero, enemy }, level.MapSize, display.ScreenHeight);

            color = new Color(50, 25, 51, 255);

            hero.SetMovementManager(movement);

            var enemyConfigs = LevelEnemyConfig.GetConfig(currentLevel);
            enemy.InitializeEnemies(enemyConfigs.Select(config => {
                var configtexture = Content.Load<Texture2D>(config.texturePath);
                return (configtexture, config.position, config.speed, config.viewRange, config.type);
            }));
        }

        private void RestartGame()
        {
            enemy.Reset();
            hero.Reset();
            healthBar.Reset();
            movement.IsDead = false;
            canHeroMove = true;
            gameOverFlag = false;
            WonFlag = false;
            hero.location = new Vector2(20, 1200);
        }

        private void GoToStartScreen()
        {
            startScreen = new StartScreen(_spriteBatch, screen, button, font); 
            WonFlag = false;
            wonScreen = new WonScreen(_spriteBatch, screen, button, font);

        }

    }
}
