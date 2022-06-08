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

        private Dictionary<string, GameState> _screens = new Dictionary<string, GameState>();
        private string _activeState;

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

        public void AddScreen(string screenName, GameState screen)
        {
            _screens.Add(screenName, screen);
        }

        public void RemoveScreen(string screenName)
        {
            _screens.Remove(screenName);
        }

        public void ClearScreens()
        {
            _screens.Clear();
        }

        public void ChangeScreen(string screenName)
        {
            if (_screens.ContainsKey(screenName))
            {
                _activeState = screenName;

                var newScreen = _screens[_activeState];
                if (_activeState == "main_game") ((MainGameState)newScreen).Reset();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_activeState != null)
            {
                _screens[_activeState].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_activeState != null)
            {
                _screens[_activeState].Draw(spriteBatch);
            }
        }

        public void UnloadContent()
        {
            foreach (GameState state in _screens.Values)
            {
                state.UnloadContent();
            }
        }
    }
}
