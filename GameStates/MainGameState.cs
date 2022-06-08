using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recoil.GameStates
{
    class MainGameState : GameState
    {
        private GraphicsDevice Graphics;

        private const float CollectableHeightRatio = 7f / 8f;
        private const float GravityValue = 50f;

        private float screenHeight;
        private float screenWidth;

        private const float spawnDelay = 5f;
        private float timeSinceLastSpawn;

        private Random r;

        private float Score;

        //Game Classes
        private Player player;
        private AmmoSpawner ammoSpawner;
        private CoinSpawner coinSpawner;
        private AntiGravSpawner antiGravSpawner;

        //UI Classes
        private SpriteFont AmmoFont;
        private SpriteFont TimeFont;

        public MainGameState(GraphicsDevice graphicsDevice, float sWidth, float sHeight) : base(graphicsDevice)
        {
            Graphics = graphicsDevice;
            screenHeight = sHeight;
            screenWidth = sWidth;
        }

        public override void Initialise()
        {
            timeSinceLastSpawn = spawnDelay;
            r = new Random();
        }

        public override void LoadContent(ContentManager Content)
        {
            player = new Player(Graphics, Content, 1f, GravityValue, screenHeight, screenWidth);

            float spawnYBoundary = CollectableHeightRatio * screenHeight;
            ammoSpawner = new AmmoSpawner(Content, "ammo_crate", spawnYBoundary, screenWidth);
            coinSpawner = new CoinSpawner(Content, "coin", spawnYBoundary, screenWidth);
            antiGravSpawner = new AntiGravSpawner(Content, "antigrav_vial", spawnYBoundary, screenWidth);

            AmmoFont = Content.Load<SpriteFont>("ammo_font");
            TimeFont = Content.Load<SpriteFont>("time_font");
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Score += elapsedTime / 100;
            
            if (timeSinceLastSpawn >= spawnDelay)
            {
                SpawnCollectables();
            }

            //Update Entities
            player.Update(elapsedTime);

            //Check Object Collisions
            ammoSpawner.CheckCollisions(player);
            coinSpawner.CheckCollisions(player, ref Score);
            antiGravSpawner.CheckCollisions(player);

            //Check if Player dead
            //Height limit
            if (player.y > screenHeight)
            {
                player.canMove = false;
                DestroyCollectables();
                ShowDeathScreen();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawUI(spriteBatch);
            DrawCollectables(spriteBatch);
            player.Draw(spriteBatch);
        }

        private void DrawUI(SpriteBatch spriteBatch)
        {
            string ammoString = player.Ammo.ToString();
            Vector2 ammoSize = AmmoFont.MeasureString(ammoString);

            string timeString = "Score: " + ((int)Score).ToString();

            spriteBatch.DrawString(AmmoFont, ammoString, new Vector2((screenWidth / 2) - (ammoSize.X / 2), (screenHeight / 2) - ammoSize.Y), Color.White);
            spriteBatch.DrawString(TimeFont, timeString, new Vector2(0, 0), Color.White);
        }
           
        private void DrawCollectables(SpriteBatch spriteBatch)
        {
            ammoSpawner.Draw(spriteBatch);
            coinSpawner.Draw(spriteBatch);
            antiGravSpawner.Draw(spriteBatch);
        }
        private void DestroyCollectables()
        {
            ammoSpawner.DestroyAll();
            coinSpawner.DestroyAll();
            antiGravSpawner.DestroyAll();
        }

        private void ShowDeathScreen()
        {
        }

        private void SpawnCollectables()
        {
            int spawnAmount = r.Next(1, ammoSpawner.maxSpawn);
            ammoSpawner.Spawn(Graphics, spawnAmount);
            
            int coinChance = r.Next(1, 3);
            int coinSpawnAmount = r.Next(1, coinSpawner.maxSpawn);
            if (coinChance != 3) coinSpawner.Spawn(Graphics, coinSpawnAmount);

            int antiGravChance = r.Next(1, 5);
            if (antiGravChance == 4) antiGravSpawner.Spawn(Graphics, 1);

            timeSinceLastSpawn = 0f;
        }
    }
}