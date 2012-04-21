using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaGUILib;
using Microsoft.Xna.Framework;

namespace Gravitation.Screens
{
    class SelectShipScreen: BaseScreen, IDrawableScreen
    {
        ShipSettings settingsWindow;

        public SelectShipScreen(int screenHeight, int screenWidth)
            : base(screenHeight, screenWidth)
        {
            settingsWindow = new ShipSettings(screenWidth, screenWidth);
        }

        public void LoadContent(Microsoft.Xna.Framework.GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
        }

        public DataClasses.IScreenExitData Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            XnaGUIManager.Update(gameTime);

            settingsWindow.Update(gameTime);

            DataClasses.ShipConfiguration ship = settingsWindow.getShip();
            if (ship != null)
                return ship;

            return null;
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, GameTime gameTime)
        {

            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            XnaGUIManager.Draw(frameTime);
        }

        public void HandleKeyboard(Microsoft.Xna.Framework.Input.KeyboardState curState, Microsoft.Xna.Framework.Input.KeyboardState prevState)
        {
            
        }

        public Microsoft.Xna.Framework.Matrix getView()
        {
            return base._view;
        }

    }
}
