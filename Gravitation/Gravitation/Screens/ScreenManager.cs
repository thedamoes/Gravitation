using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gravitation.SpriteObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gravitation.Screens
{
    public class ScreenManager
    {
        private static ScreenManager _screenManager;
        private Screens.IDrawableScreen _currentScreen;
        private Sprite _screenOverlay;

        public static ScreenManager GetScreenManager
        {
            get 
            {
                if (_screenManager == null)
                    _screenManager = new ScreenManager();

                return _screenManager;
            }
        }

        public Screens.IDrawableScreen CurrentScreen
        {
            set { _currentScreen = value; }
            get { return _currentScreen; }
        }

        public Sprite ScreenOverlay
        {
            set { _screenOverlay = value; }
        }



        private ScreenManager()
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_currentScreen != null)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _currentScreen.getView());
                _currentScreen.Draw(spriteBatch, gameTime);
                spriteBatch.End();
            }
            if (_screenOverlay != null)
            {
                spriteBatch.Begin();
                _screenOverlay.Draw(spriteBatch);
                spriteBatch.End();
            }
        }

        


    }
}
