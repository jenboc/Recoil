using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Recoil
{
    class UIManager
    {
        Texture2D SplashPixel;

        SpriteFont AmmoFont;
        SpriteFont TimeFont;
        SpriteFont MenuFont;

        Texture2D Logo;
        Texture2D CharacterImg;

        SpriteClass Button1;
        SpriteClass Button2;

        //States: 'm': Main Menu
        //        'g': Game UI
        //        'd': Death Screen
        public char state;

        int AmmoCount = 0;
        float score;

        public UIManager(GraphicsDevice gDevice, ContentManager Content)
        {
            AmmoFont = Content.Load<SpriteFont>("ammo_font");
            MenuFont = Content.Load<SpriteFont>("menu_font");
            TimeFont = Content.Load<SpriteFont>("time_font");

            Logo = Content.Load<Texture2D>("logo");
            CharacterImg = Content.Load<Texture2D>("character_img");

            Button1 = new SpriteClass(gDevice, Content, "button", 1f);
            Button2 = new SpriteClass(gDevice, Content, "button", 1f);

            SplashPixel = new Texture2D(gDevice, 1, 1);
            SplashPixel.SetData(new Color[] { new Color(64, 64, 64) });

            score = 0;
        }

        public bool StartButtonPressed(MouseState mState)
        {
            return Button1.RectangleCollision(mState.X, mState.Y) && mState.LeftButton == ButtonState.Pressed;
        }
        
        public bool ExitButtonPressed(MouseState mState)
        {
            return Button2.RectangleCollision(mState.X, mState.Y) && mState.LeftButton == ButtonState.Pressed && state == 'm'; 
        }

        public bool MainMenuButtonPressed(MouseState mState)
        {
            return Button2.RectangleCollision(mState.X, mState.Y) && mState.LeftButton == ButtonState.Pressed && state == 'd';
        }

        public void ChangeUIState(char state)
        {
            this.state = state;
        }

        public void UpdateAmmoCount(int ammoCount)
        {
            AmmoCount = ammoCount;
        }

        public void UpdateTimer(float time)
        {
            float increment = time / 100;
            score += increment;
        }

        public void AddScore(int amount)
        {
            score += amount;
        }

        public void ResetScore()
        {
            score = 0;
        }

        public void DrawGameUI(SpriteBatch spriteBatch, float sHeight, float sWidth)
        {
            string ammoString = AmmoCount.ToString();
            Vector2 ammoSize = AmmoFont.MeasureString(ammoString);

            string timeString = "Score: " + ((int)score).ToString();

            spriteBatch.DrawString(AmmoFont, ammoString, new Vector2((sWidth / 2) - (ammoSize.X / 2), (sHeight / 2) - ammoSize.Y), Color.White);
            spriteBatch.DrawString(TimeFont, timeString, new Vector2(0, 0), Color.White);
        }

        public void DrawDeathScreen(SpriteBatch spriteBatch, float sHeight, float sWidth)
        {
            //Move and Draw Assets            
            Button1.x = sWidth / 2 - Button1.texture.Width / 2;
            Button1.y = sHeight / 2 + Button1.texture.Height;

            Button2.x = sWidth / 2 + Button2.texture.Width / 2;
            Button2.y = Button1.y;

            spriteBatch.Draw(SplashPixel, new Rectangle(0, 0, (int)sWidth, (int)sHeight), Color.White);
            Button1.Draw(spriteBatch);
            Button2.Draw(spriteBatch);

            //Draw Text
            string diedString = "You Died";
            Vector2 diedSize = MenuFont.MeasureString(diedString);
            string scoreString = "Score: " + ((int)score).ToString();
            Vector2 scoreSize = MenuFont.MeasureString(scoreString);

            string b1Text = "Play";
            Vector2 b1TextSize = MenuFont.MeasureString(b1Text);
            string b2Text = "Menu";
            Vector2 b2TextSize = MenuFont.MeasureString(b2Text);

            spriteBatch.DrawString(MenuFont, diedString, new Vector2(sWidth/2 - diedSize.X/2, sHeight/6), Color.White);
            spriteBatch.DrawString(MenuFont, scoreString, new Vector2(sWidth/2 - scoreSize.X/2, sHeight/3), Color.White);
            spriteBatch.DrawString(MenuFont, b1Text, new Vector2(Button1.x - b1TextSize.X / 2, Button1.y - b1TextSize.Y * 3 / 7), Color.Black);
            spriteBatch.DrawString(MenuFont, b2Text, new Vector2(Button2.x - b2TextSize.X / 2, Button2.y - b2TextSize.Y * 3 / 7), Color.Black);

        }

        public void DrawMainMenu(SpriteBatch spriteBatch, float sHeight, float sWidth)
        {
            //Move and Draw Assets
            Button1.x = sWidth / 2;
            Button1.y = sHeight / 2;

            Button2.x = sWidth / 2;
            Button2.y = Button1.y + Button2.texture.Height + 20;

            spriteBatch.Draw(SplashPixel, new Rectangle(0, 0, (int)sWidth, (int)sHeight), Color.White);
            spriteBatch.Draw(Logo, new Rectangle((int)((sWidth/2) - (Logo.Width / 2)), (int)((Button1.y - Button1.texture.Height) - 375), Logo.Width, Logo.Height), Color.White);
            spriteBatch.Draw(CharacterImg, new Rectangle((int)(Button1.x-Button1.texture.Width/2)/4, (int)Button1.y - Button1.texture.Height/2, CharacterImg.Width, CharacterImg.Height), Color.White);
            Button1.Draw(spriteBatch);
            Button2.Draw(spriteBatch);

            //Draw Text
            string b1Text = "Play";
            Vector2 b1TextSize = MenuFont.MeasureString(b1Text);
            string b2Text = "Exit";
            Vector2 b2TextSize = MenuFont.MeasureString(b2Text);

            spriteBatch.DrawString(MenuFont, b1Text, new Vector2(Button1.x - b1TextSize.X/2, Button1.y - b1TextSize.Y * 3/7), Color.Black);
            spriteBatch.DrawString(MenuFont, b2Text, new Vector2(Button2.x - b2TextSize.X/2, Button2.y - b2TextSize.Y * 3/7), Color.Black);
        }

        public void Draw(SpriteBatch spriteBatch, float sHeight, float sWidth)
        {
            switch (state)
            {
                case 'm':
                    DrawMainMenu(spriteBatch, sHeight, sWidth);
                    break;
                case 'g':
                    DrawGameUI(spriteBatch, sHeight, sWidth);
                    break;
                case 'd':
                    DrawDeathScreen(spriteBatch, sHeight, sWidth);
                    break;
            }
        }
    }
}
