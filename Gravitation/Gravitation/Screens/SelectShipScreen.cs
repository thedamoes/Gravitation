using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaGUILib;

namespace Gravitation.Screens
{
    class SelectShipScreen: BaseScreen, IDrawableScreen
    {
        public SelectShipScreen(int screenHeight, int screenWidth)
            : base(screenHeight, screenWidth)
        {
            
        }

        public void LoadContent(Microsoft.Xna.Framework.GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            throw new NotImplementedException();
        }

        public DataClasses.GameConfiguration Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            throw new NotImplementedException();
        }

        public void HandleKeyboard(Microsoft.Xna.Framework.Input.KeyboardState curState, Microsoft.Xna.Framework.Input.KeyboardState prevState)
        {
            throw new NotImplementedException();
        }

        public Microsoft.Xna.Framework.Matrix getView()
        {
            throw new NotImplementedException();
        }


        DataClasses.IScreenExitData IDrawableScreen.Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
