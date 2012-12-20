using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Gravitation.Screens.Menu
{
    class MenuScreen : IDrawableScreen
    {
        public event EventHandler<DataClasses.GameSelectedEventArgs> gameSelected;
        private GraphicsDeviceManager dMan;
        private Microsoft.Xna.Framework.Content.ContentManager cm;

        private int mScreenHeight;
        private int mScreenWidth;
        private SoundHandler mPlayer;

        private IDrawableScreen currentMenuScreen;


        public MenuScreen(int ScreenWidth, int ScreenHeight, SoundHandler player,
                           GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            currentMenuScreen = new MainMenu(ScreenWidth, ScreenHeight, player);
            currentMenuScreen.gameSelected += gameSelectedHandler;

            this.dMan = dMan;
            this.cm = cm;

            this.mPlayer = player;
            this.mScreenHeight = ScreenHeight;
            this.mScreenWidth = ScreenWidth;
        }

        public void LoadContent(GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            currentMenuScreen.LoadContent(dMan, cm);
        }

        public void Update(GameTime gameTime)
        {
            currentMenuScreen.Update(gameTime); 
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

        private void gameSelectedHandler(object sender, DataClasses.GameSelectedEventArgs e)
        {
            if (this.gameSelected != null)
                this.gameSelected(sender, e);
        }


        public void windowCloseing()
        {
            if (this.currentMenuScreen != null)
                this.currentMenuScreen.windowCloseing();
        }
    }
}
