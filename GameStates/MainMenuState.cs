using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recoil.GameStates
{
    class MainMenuState : GameState
    {
        private float sWidth;
        private float sHeight;

        private GraphicsDevice Graphics;
        private SpriteFont MenuFont;
        private Texture2D Logo;
        private Texture2D CharacterImg;
        private Texture2D SplashPixel;
        private SpriteClass PlayButton;
        private SpriteClass ExitButton;

        public MainMenuState(GraphicsDevice graphicsDevice, float sWidth, float sHeight) : base(graphicsDevice)
        {
            this.sWidth = sWidth;
            this.sHeight = sHeight;
            Graphics = graphicsDevice;
        }

        public override void Initialise()
        {
        }

        public override void LoadContent(ContentManager Content)
        {
            MenuFont = Content.Load<SpriteFont>("menu_font");

            Logo = Content.Load<Texture2D>("logo");
            CharacterImg = Content.Load<Texture2D>("character_img");

            PlayButton = new SpriteClass(Graphics, Content, "button", 1f);
            ExitButton = new SpriteClass(Graphics, Content, "button", 1f);

            SplashPixel = new Texture2D(Graphics, 1, 1);
            SplashPixel.SetData(new Color[] { new Color(64, 64, 64) });
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();
            bool clicking = mState.LeftButton == ButtonState.Pressed;

            if (clicking && PlayButton.RectangleCollision(mState.X, mState.Y))
            {
                GameStateManager.Instance.ChangeScreen("main_game");
            }
            else if (clicking && ExitButton.RectangleCollision(mState.X, mState.Y))
            {
                Game1.Instance.Exit();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Move and Draw Assets
            PlayButton.x = sWidth / 2;
            PlayButton.y = sHeight / 2;

            ExitButton.x = sWidth / 2;
            ExitButton.y = PlayButton.y + ExitButton.texture.Height + 20;

            spriteBatch.Draw(SplashPixel, new Rectangle(0, 0, (int)sWidth, (int)sHeight), Color.White);
            spriteBatch.Draw(Logo, new Rectangle((int)((sWidth / 2) - (Logo.Width / 2)), (int)((PlayButton.y - PlayButton.texture.Height) - 375), Logo.Width, Logo.Height), Color.White);
            spriteBatch.Draw(CharacterImg, new Rectangle((int)(PlayButton.x - PlayButton.texture.Width / 2) / 4, (int)PlayButton.y - PlayButton.texture.Height / 2, CharacterImg.Width, CharacterImg.Height), Color.White);
            PlayButton.Draw(spriteBatch);
            ExitButton.Draw(spriteBatch);

            //Draw Text
            string b1Text = "Play";
            Vector2 b1TextSize = MenuFont.MeasureString(b1Text);
            string b2Text = "Exit";
            Vector2 b2TextSize = MenuFont.MeasureString(b2Text);

            spriteBatch.DrawString(MenuFont, b1Text, new Vector2(PlayButton.x - b1TextSize.X / 2, PlayButton.y - b1TextSize.Y * 3 / 7), Color.Black);
            spriteBatch.DrawString(MenuFont, b2Text, new Vector2(ExitButton.x - b2TextSize.X / 2, ExitButton.y - b2TextSize.Y * 3 / 7), Color.Black);
        }
    }
}
