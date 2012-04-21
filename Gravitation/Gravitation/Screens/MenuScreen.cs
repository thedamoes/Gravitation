using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Gravitation.Screens
{
    class MenuScreen : IDrawableScreen
    {
        private GraphicsDeviceManager dMan;
        private Microsoft.Xna.Framework.Content.ContentManager cm;

        private IDrawableScreen currentMenuScreen;


        public MenuScreen(int ScreenWidth, int ScreenHeight, SoundHandler player,
                           GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            currentMenuScreen = new MainMenu(ScreenWidth, ScreenHeight, player);

            this.dMan = dMan;
            this.cm = cm;
        }

        public void LoadContent(GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            currentMenuScreen.LoadContent(dMan, cm); // cm and dman atent needed to be passed in here anymore refactor later
        }

        public DataClasses.IScreenExitData Update(GameTime gameTime)
        {
            DataClasses.IScreenExitData screen = currentMenuScreen.Update(gameTime);

            if (screen == null)
                return null;

            if (screen.GetType().Equals(typeof(DataClasses.DisplayNewScreen)))
            {
                currentMenuScreen = ((DataClasses.DisplayNewScreen)screen).NewScreen;
                LoadContent(dMan, cm);

                return null;
            }
            else if (screen.GetType().Equals(typeof(DataClasses.GameConfiguration)))
            {
                return screen;
            }

            else
                return null;
                
        }

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            currentMenuScreen.Draw(sb, gameTime);
        }

        public void HandleKeyboard(KeyboardState curState, KeyboardState prevState)
        {
            currentMenuScreen.HandleKeyboard(curState, prevState);
        }

        public Matrix getView()
        {
            return currentMenuScreen.getView();
        }
    }
}
