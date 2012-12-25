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
        event EventHandler<DataClasses.GameSelectedEventArgs> gameSelected;

        void LoadContent(GraphicsDeviceManager dMan,ContentManager cm);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch sb, GameTime gameTime);
        void HandleKeyboard(KeyboardState curState, KeyboardState prevState);
        Matrix getView();

        void windowCloseing();
    }
}
