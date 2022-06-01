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
        public Texture2D headUp { get; set; }
        public Texture2D headDown { get; set; }
        public Texture2D headRight { get; set; }
        public Texture2D headLeft { get; set; }

        public SpriteClass Gun { get; set; }
        public SpriteClass Arms { get; set; }
        public SpriteClass Head { get; set; }

        public int Ammo { get; set; }
        public int MaxAmmo { get; set; }
        public float shootForce { get; set; }
        public float gravityValue { get; set; }
        public float sHeight { get; set; }
        public float sWidth { get; set; }
        public float cooldownTime { get; set; }
        public float gunshotDelay { get; set; }
        public bool canMove { get; set; }

        public Player(GraphicsDevice gDevice, ContentManager Content, float scale, float gravityVal, float sHeight, float sWidth) : base(gDevice, Content, "body", scale)
        {
            MaxAmmo = 10;
            Ammo = MaxAmmo;

            Gun = new SpriteClass(gDevice, Content, "gun_placeholder", 1f);
            Gun.origin = new Vector2(0, Gun.texture.Height / 2);

            headUp = Content.Load<Texture2D>("head_up");
            headDown = Content.Load<Texture2D>("head_down");
            headRight = Content.Load<Texture2D>("head_right");
            headLeft = Content.Load<Texture2D>("head_left");
            Head = new SpriteClass(gDevice, headRight, 1f);

            Arms = new SpriteClass(gDevice, Content, "arms", 1f);
            Arms.origin = Vector2.Zero;

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

            //Update Body Parts
            Gun.x = x;
            Gun.y = y;

            Head.x = x;
            Head.y = (y - texture.Height / 2) - Head.texture.Height/2;

            Arms.x = x;
            Arms.y = y - texture.Height / 2;

            Gun.Update(elapsedTime);

            uiManager.UpdateAmmoCount(Ammo);
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Gun.Draw(spriteBatch);
            Head.Draw(spriteBatch);
            Arms.Draw(spriteBatch);
        }
    }
}
