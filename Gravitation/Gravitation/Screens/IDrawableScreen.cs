using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Gravitation.Screens
{
    interface IDrawableScreen
    {
        void LoadContent(SpriteBatch sb, GraphicsDeviceManager dMan,ContentManager cm);
        DataClasses.GameConfiguration Update(GameTime gameTime);
        void Draw(SpriteBatch sb);
        void HandleKeyboard(KeyboardState curState, KeyboardState prevState);
        Matrix getView();
    }
}
