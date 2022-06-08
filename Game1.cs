using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Recoil.GameStates;

namespace Recoil
{
    public class Game1 : Game
    {
        public static Game1 Instance;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        float screenWidth;
        float screenHeight;

        GameStateManager StateManager;
        MainMenuState MainMenu;
        MainGameState MainGame;
        DeathScreenState DeathScreen;

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

            StateManager = new GameStateManager();
            MainMenu = new MainMenuState(GraphicsDevice, screenWidth, screenHeight);
            MainGame = new MainGameState(GraphicsDevice, screenWidth, screenHeight);
            DeathScreen = new DeathScreenState(GraphicsDevice, screenWidth, screenHeight);

            MainMenu.Initialise();
            MainGame.Initialise();
            DeathScreen.Initialise();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            MainMenu.LoadContent(Content);
            MainGame.LoadContent(Content);
            DeathScreen.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            StateManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            StateManager.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
