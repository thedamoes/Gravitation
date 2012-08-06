using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaGUILib;
using Microsoft.Xna.Framework;

namespace Gravitation.Screens.Menu
{
    class SelectShipScreen: BaseScreen, IDrawableScreen
    {
        Type gameType;
        ShipSettings settingsWindow;
        public event EventHandler<DataClasses.GameSelectedEventArgs> gameSelected;
        public SelectShipScreen(int screenHeight, int screenWidth, Type gameType, SoundHandler handler)
            : base(screenHeight, screenWidth)
        {
            if (gameType.BaseType != typeof(BaseGame))
            {
                throw new ArgumentException("Awww shit Nigga  What the fuck u playing at gaim type aint a base game");
            }
            this.gameType = gameType;
            settingsWindow = new ShipSettings(screenWidth, screenWidth, handler);
            settingsWindow.okClicked += new EventHandler<EventArgs>(settingsWindow_okClicked);

        }

        void settingsWindow_okClicked(object sender, EventArgs e)
        {
            object[] constructoArgs = new object[]{(object)new DataClasses.GameConfiguration("../../../Maps/level1.xml", settingsWindow.getShip().Ship, null)};

            System.Reflection.ConstructorInfo constructorInfo = this.gameType.GetConstructor(new Type[]{typeof(DataClasses.GameConfiguration)});
                
             BaseGame game =(BaseGame)constructorInfo.Invoke(constructoArgs);

            base.fire<DataClasses.GameSelectedEventArgs>(this.gameSelected, new DataClasses.GameSelectedEventArgs(game));
        }

        public void LoadContent(Microsoft.Xna.Framework.GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {

        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            XnaGUIManager.Update(gameTime);

            settingsWindow.Update(gameTime);
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
