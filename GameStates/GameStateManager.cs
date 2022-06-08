using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Recoil.GameStates
{
    class GameStateManager
    {
        private static GameStateManager _instance;
        private ContentManager _content;

        private Stack<GameState> _screens = new Stack<GameState>();

        public static GameStateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameStateManager();
                }
                return _instance;
            }
        }

        public void AddScreen(GameState screen)
        {
            _screens.Push(screen);
            _screens.Peek().Initialise();

            if (_content != null)
            {
                _screens.Peek().LoadContent(_content);
            }
        }

        public void RemoveScreen()
        {
            if (_screens.Count > 0)
            {
                GameState screen = _screens.Peek();
                _screens.Pop();
            }
        }

        public void ClearScreens()
        {
            while (_screens.Count > 0)
            {
                _screens.Pop();
            }
        }

        public void ChangeScreen(GameState screen)
        {
            ClearScreens();
            AddScreen(screen);
        }

        public void Update(GameTime gameTime)
        {
            if (_screens.Count > 0)
            {
                _screens.Peek().Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_screens.Count > 0)
            {
                _screens.Peek().Draw(spriteBatch);
            }
        }

        public void UnloadContent()
        {
            foreach (GameState state in _screens)
            {
                state.UnloadContent();
            }
        }
    }
}
