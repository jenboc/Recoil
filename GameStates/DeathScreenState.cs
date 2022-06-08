using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recoil.GameStates
{
    class DeathScreenState : GameState
    {
        private float sWidth;
        private float sHeight;

        private GraphicsDevice Graphics;
        private SpriteFont MenuFont;
        private Texture2D Logo;
        private Texture2D CharacterImg;
        private Texture2D SplashPixel;
        private SpriteClass PlayButton;
        private SpriteClass MenuButton;

        public DeathScreenState(GraphicsDevice graphicsDevice, float sWidth, float sHeight) : base(graphicsDevice)
        {
            this.sWidth = sWidth;
            this.sHeight = sHeight;
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
            MenuButton = new SpriteClass(Graphics, Content, "button", 1f);

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
                //Change to Main Game State
            }
            else if (clicking && MenuButton.RectangleCollision(mState.X, mState.Y))
            {
                //Change to Main Menu
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Move and Draw Assets            
            PlayButton.x = sWidth / 2 - PlayButton.texture.Width / 2;
            PlayButton.y = sHeight / 2 + PlayButton.texture.Height;

            MenuButton.x = sWidth / 2 + MenuButton.texture.Width / 2;
            MenuButton.y = PlayButton.y;

            spriteBatch.Draw(SplashPixel, new Rectangle(0, 0, (int)sWidth, (int)sHeight), Color.White);
            PlayButton.Draw(spriteBatch);
            MenuButton.Draw(spriteBatch);

            //Draw Text
            string diedString = "You Died";
            Vector2 diedSize = MenuFont.MeasureString(diedString);
            string scoreString = "Score: " + ((int)score).ToString();
            Vector2 scoreSize = MenuFont.MeasureString(scoreString);

            string b1Text = "Play";
            Vector2 b1TextSize = MenuFont.MeasureString(b1Text);
            string b2Text = "Menu";
            Vector2 b2TextSize = MenuFont.MeasureString(b2Text);

            spriteBatch.DrawString(MenuFont, diedString, new Vector2(sWidth / 2 - diedSize.X / 2, sHeight / 6), Color.White);
            spriteBatch.DrawString(MenuFont, scoreString, new Vector2(sWidth / 2 - scoreSize.X / 2, sHeight / 3), Color.White);
            spriteBatch.DrawString(MenuFont, b1Text, new Vector2(PlayButton.x - b1TextSize.X / 2, PlayButton.y - b1TextSize.Y * 3 / 7), Color.Black);
            spriteBatch.DrawString(MenuFont, b2Text, new Vector2(MenuButton.x - b2TextSize.X / 2, MenuButton.y - b2TextSize.Y * 3 / 7), Color.Black);
        }
    }
}
