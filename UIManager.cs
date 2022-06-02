using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Recoil
{
    class UIManager
    {
        Texture2D SplashPixel;

        SpriteFont AmmoFont;
        SpriteFont TimeFont;
        SpriteFont MenuFont;

        bool MenuVisible;
        bool AmmoVisible;

        int AmmoCount = 0;
        float score;

        bool gameOver;

        public UIManager(GraphicsDevice gDevice, ContentManager Content)
        {
            AmmoFont = Content.Load<SpriteFont>("ammo_font");
            MenuFont = Content.Load<SpriteFont>("menu_font");
            TimeFont = Content.Load<SpriteFont>("time_font");

            SplashPixel = new Texture2D(gDevice, 1, 1);
            SplashPixel.SetData(new Color[] { new Color(99, 99, 99) });

            score = 0;
        }

        public void ShowMenu(bool playerDied)
        {
            gameOver = playerDied;
            AmmoVisible = false;
            MenuVisible = true;
        }

        public void ShowAmmo()
        {
            MenuVisible = false;
            AmmoVisible = true;            
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

        public void Draw(SpriteBatch spriteBatch, float sHeight, float sWidth)
        {
            if (AmmoVisible)
            {
                string ammoString = AmmoCount.ToString();
                Vector2 ammoSize = AmmoFont.MeasureString(ammoString);

                string timeString = "Score: " + ((int)score).ToString();
                Vector2 timeSize = TimeFont.MeasureString(timeString);

                spriteBatch.DrawString(AmmoFont, ammoString, new Vector2((sWidth / 2) - (ammoSize.X/2), (sHeight / 2) - ammoSize.Y), Color.White);
                spriteBatch.DrawString(TimeFont, timeString, new Vector2(0, 0), Color.White);
            }

            if (MenuVisible)
            {
                spriteBatch.Draw(SplashPixel, new Rectangle(0, 0, (int)sWidth, (int)sHeight), Color.White);

                string nameString = "GAME NAME";
                Vector2 nameSize = MenuFont.MeasureString(nameString);
                spriteBatch.DrawString(MenuFont, nameString, new Vector2((sWidth / 2) - (nameSize.X / 2), (sHeight / 3) - nameSize.Y), Color.White);

                if (gameOver)
                {
                    string diedString = "You Died";
                    Vector2 diedSize = MenuFont.MeasureString(diedString);

                    string scoreString = "Score: " + ((int)score).ToString();
                    Vector2 scoreSize = MenuFont.MeasureString(scoreString);

                    string instString = "Press ENTER to Try Again";
                    Vector2 instSize = MenuFont.MeasureString(instString);

                    spriteBatch.DrawString(MenuFont, diedString, new Vector2((sWidth / 2) - (diedSize.X / 2), (sHeight / 2) - diedSize.Y), Color.White);
                    spriteBatch.DrawString(MenuFont, scoreString, new Vector2((sWidth / 2) - (scoreSize.X / 2), (sHeight * 2 / 3) - scoreSize.Y), Color.White);
                    spriteBatch.DrawString(MenuFont, instString, new Vector2((sWidth / 2) - (instSize.X / 2), (sHeight * 5 / 6) - instSize.Y), Color.White);
                }
                else
                {
                    string instString = "Press ENTER to Play";
                    Vector2 instSize = MenuFont.MeasureString(instString);
                    spriteBatch.DrawString(MenuFont, instString, new Vector2((sWidth / 2) - (instSize.X / 2), (sHeight / 2) - instSize.Y), Color.White);
                }
            }
        }
    }
}
