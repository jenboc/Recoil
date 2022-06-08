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

        }

        public bool StartButtonPressed(MouseState mState)
        {
            return 
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
            

        }

        public void DrawMainMenu(SpriteBatch spriteBatch, float sHeight, float sWidth)
        {

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
