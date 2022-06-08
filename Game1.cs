using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Recoil
{
    public class Game1 : Game
    {
        public static Game1 Instance;

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
        CoinSpawner coinSpawner;
        AntiGravSpawner antiGravSpawner;

        Random r;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Instance = this;
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
            uiManager.ChangeUIState('m');

            float spawnYBoundary = collectableHeightRatio * screenHeight;
            ammoSpawner = new AmmoSpawner(Content, "ammo_crate", spawnYBoundary, screenWidth);
            coinSpawner = new CoinSpawner(Content, "coin", spawnYBoundary, screenWidth);
            antiGravSpawner = new AntiGravSpawner(Content, "antigrav_vial", spawnYBoundary, screenWidth);
        }

        void SpawnCollectables()
        {
            int spawnAmount = r.Next(1, maxSpawn);
            ammoSpawner.Spawn(GraphicsDevice, spawnAmount);
            timeSinceLastSpawn = 0f;

            int coinChance = r.Next(1, 3);
            int coinSpawnAmount = r.Next(1, coinSpawner.maxSpawn);
            if (coinChance != 3) coinSpawner.Spawn(GraphicsDevice, coinSpawnAmount);

            int antiGravChance = r.Next(1, 5);
            if (antiGravChance == 4) antiGravSpawner.Spawn(GraphicsDevice, 1);
        }

        protected override void Update(GameTime gameTime)
        {
            if (gamePlaying)
            { 
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                timeSinceLastSpawn += elapsedTime;
                timeSinceStart += elapsedTime;
                uiManager.UpdateTimer(timeSinceStart);

                if (timeSinceLastSpawn >= spawnDelay)
                {
                    SpawnCollectables();
                }

                //Update Entities
                player.Update(elapsedTime, uiManager);

                //Check Object Collisions
                ammoSpawner.CheckCollisions(player);
                coinSpawner.CheckCollisions(player, uiManager);
                antiGravSpawner.CheckCollisions(player);

                //Check if Player dead
                //Height limit
                if (player.y > screenHeight)
                {
                    gamePlaying = false;
                    player.canMove = false;
                    uiManager.ChangeUIState('d');
                    ammoSpawner.DestroyAll();
                    coinSpawner.DestroyAll();
                    antiGravSpawner.DestroyAll();
                }

                base.Update(gameTime);
            }
            else
            {
                KeyboardState kState = Keyboard.GetState();
                MouseState mState = Mouse.GetState();

                bool startButtonPress = uiManager.StartButtonPressed(mState);
                bool exitButtonPress = uiManager.ExitButtonPressed(mState);
                bool mainMenuButtonPress = uiManager.MainMenuButtonPressed(mState);

                if (startButtonPress)
                {
                    uiManager.ChangeUIState('g');

                    player.x = screenWidth / 2 - player.texture.Width / 2;
                    player.y = 0;
                    player.dY = 0;
                    player.dX = 0;
                    player.Ammo = player.MaxAmmo;
                    player.canMove = true;
                    player.cooldownTime = 0;
                    player.RemoveBuffs();

                    uiManager.ResetScore();

                    gamePlaying = true;
                    timeSinceStart = 0;
                }
                else if (exitButtonPress)
                {
                    Exit();
                }
                else if (mainMenuButtonPress)
                {
                    uiManager.ChangeUIState('m');
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (gamePlaying)
            {
                player.Draw(_spriteBatch);
                uiManager.Draw(_spriteBatch, screenHeight, screenWidth);
                ammoSpawner.Draw(_spriteBatch);
                coinSpawner.Draw(_spriteBatch);
                antiGravSpawner.Draw(_spriteBatch);
            }
            else uiManager.Draw(_spriteBatch, screenHeight, screenWidth); 

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
