using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Recoil
{
    interface IGameState
    {
        //Initialise Game Settigns 
        void Initialise();
        //Load Content here
        void LoadContent(ContentManager Content);
        //Unload Content here
        void UnloadContent();
        //Update Game
        void Update(GameTime gameTime);
        //Draw Game
        void Draw(SpriteBatch spriteBatch);
    }
}
