using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Recoil
{
    class Player : SpriteClass
    {
        public SpriteClass Gun { get; set; }
        public int Ammo { get; set; }
        public int MaxAmmo { get; set; }
        public float shootForce { get; set; }
        public float gravityValue { get; set; }
        public float sHeight { get; set; }
        public float sWidth { get; set; }
        public float cooldownTime { get; set; }
        public float gunshotDelay { get; set; }
        public bool canMove { get; set; }

        public Player(GraphicsDevice gDevice, ContentManager Content, string textureName, float scale, float gravityVal, float sHeight, float sWidth) : base(gDevice, Content, textureName, scale)
        {
            MaxAmmo = 10;
            Ammo = MaxAmmo;

            Gun = new SpriteClass(gDevice, Content, "gun_placeholder", 1f);
            Gun.origin = new Vector2(0, Gun.texture.Height / 2);

            x = 1000;
            y = 0;

            shootForce = 1500;
            gravityValue = gravityVal;

            this.sWidth = sWidth;
            this.sHeight = sHeight;

            gunshotDelay = 0.5f;
            cooldownTime = gunshotDelay;
            canMove = false;
        }

        public void AddAmmo(int amount)
        {
            Ammo += amount;
            if (Ammo > MaxAmmo)
            {
                Ammo = MaxAmmo;
            }
        }
        public void Shoot(float mX, float mY)
        {
            if (Ammo > 0 && cooldownTime >= gunshotDelay) { 
                Vector2 mouseVector = new Vector2(mX - x, mY - y);
                mouseVector.Normalize();
                Vector2 shootVector = (mouseVector * -1) * shootForce;

                dX = shootVector.X;
                dY = shootVector.Y;
                Ammo--;
                cooldownTime = 0;
            }

        }

        public void HandleKeyInput()
        {
            MouseState mouseState = Mouse.GetState();

            //Gun Angle 
            //Angle measured Anti-clockwise in radians
            float theta;
            float cX = mouseState.X - x;
            float cY = mouseState.Y - y;
            theta = MathF.Atan(cY / cX);

            if (mouseState.X < x) theta += MathF.PI;

            Gun.angle = theta;
    
            //Get Shoot
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Shoot(mouseState.X, mouseState.Y);
            }
        }

        public void Update(float elapsedTime, UIManager uiManager)
        {
            if (!canMove) return; 

            cooldownTime += elapsedTime;
            //Change according to gravity + shooting
            HandleKeyInput();
            dY += gravityValue;
            base.Update(elapsedTime);

            //Offscreen Left
            if (x + texture.Width/2 < 0)
            {
                x = sWidth;
            }
            //Offscreen Right
            else if (x - texture.Width/2 > sWidth)
            {
                x = texture.Width / 2;
            }

            //Update Gun
            Gun.x = x;
            Gun.y = y;

            Gun.Update(elapsedTime);

            uiManager.UpdateAmmoCount(Ammo);
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Gun.Draw(spriteBatch);
        }
    }
}
