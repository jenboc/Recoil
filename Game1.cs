using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Recoil
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        float screenWidth;
        float screenHeight;
        float collectableHeightRatio = 7f / 8f;
        float gravityValue = 50f;

        float spawnDelay = 5f;
        float timeSinceLastSpawn;
        float timeSinceStart;

        int maxSpawn = 4;

        bool gamePlaying; 

        Texture2D surfaceTexture;

        Player player;
        UIManager uiManager;
        AmmoSpawner ammoSpawner;

        Random r;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            _graphics.ToggleFullScreen();

            screenHeight = _graphics.PreferredBackBufferHeight;
            screenWidth = _graphics.PreferredBackBufferWidth;

            r = new Random();
            timeSinceLastSpawn = spawnDelay;
            gamePlaying = false;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            surfaceTexture = new Texture2D(GraphicsDevice, 1, 1);
            surfaceTexture.SetData(new Color[] { new Color(99, 99, 99) });

            player = new Player(GraphicsDevice, Content, 1f, gravityValue, screenHeight, screenWidth);

            uiManager = new UIManager(GraphicsDevice, Content);
            uiManager.ShowMenu(false);

            ammoSpawner = new AmmoSpawner(Content, "ammo_crate", collectableHeightRatio * screenHeight, screenWidth);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (gamePlaying)
            { 
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                timeSinceLastSpawn += elapsedTime;
                timeSinceStart += elapsedTime;
                uiManager.UpdateTimer(timeSinceStart);

                if (timeSinceLastSpawn >= spawnDelay)
                {
                    int spawnAmount = r.Next(1, maxSpawn);
                    ammoSpawner.Spawn(GraphicsDevice, spawnAmount);
                    timeSinceLastSpawn = 0f;
                }

                //Update Entities
                player.Update(elapsedTime, uiManager);

                //Check Object Collisions
                ammoSpawner.CheckCollisions(player);

                //Check if Player dead
                //Height limit
                if (player.y > screenHeight)
                {
                    gamePlaying = false;
                    player.canMove = false;
                    uiManager.ShowMenu(true);
                    ammoSpawner.DestroyAll();
                }

                base.Update(gameTime);
            }
            else
            {
                KeyboardState kState = Keyboard.GetState();

                if (kState.IsKeyDown(Keys.Enter))
                {
                    uiManager.ShowAmmo();

                    player.x = screenWidth / 2 - player.texture.Width / 2;
                    player.y = 0;
                    player.dY = 0;
                    player.dX = 0;
                    player.Ammo = player.MaxAmmo;
                    player.canMove = true; 

                    gamePlaying = true;
                    timeSinceStart = 0;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            uiManager.Draw(_spriteBatch, screenHeight, screenWidth);

            if (gamePlaying)
            {
                player.Draw(_spriteBatch);
                ammoSpawner.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
