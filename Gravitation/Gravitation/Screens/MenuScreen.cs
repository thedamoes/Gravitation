using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Gravitation.Screens
{
    class MenuScreen : BaseScreen, IDrawableScreen
    {
        private List<string> MenuItems;
        private int iterator;
        public string InfoText { get; set; }
        public string Title { get; set; }

        private SpriteFont mFont;
        private int mScreenWidth;

        private DataClasses.GameConfiguration gameConfig = null;

        public int Iterator
        {
            get
            {
                return iterator;
            }
            set
            {
                iterator = value;
                if (iterator > MenuItems.Count - 1) iterator = MenuItems.Count - 1;
                if (iterator < 0) iterator = 0;
            }
        }

        public MenuScreen(int ScreenWidth, int ScreenHeight) :base(ScreenWidth, ScreenHeight)
        {
            this.mScreenWidth = ScreenWidth;
            Title = "Gravitation";
            MenuItems = new List<string>();
            MenuItems.Add("Race");
            MenuItems.Add("Swarm");
            MenuItems.Add("Verses");
            MenuItems.Add("Verses Networked");
            MenuItems.Add("Ship");
            MenuItems.Add("Exit Game");
            Iterator = 0;
            InfoText = string.Empty;
        }

        public void LoadContent(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Microsoft.Xna.Framework.GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            mFont = cm.Load<SpriteFont>("font");
        }

        public DataClasses.GameConfiguration Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            return gameConfig;
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            sb.DrawString(mFont, Title, new Vector2(mScreenWidth / 2 - mFont.MeasureString(Title).X / 2, 20), Color.White);
            int yPos = 100;
            for (int i = 0; i < GetNumberOfOptions(); i++)
            {
                Color colour = Color.White;
                if (i == Iterator)
                {
                    colour = Color.Gray;
                }
                sb.DrawString(mFont, GetItem(i), new Vector2(mScreenWidth / 2 - mFont.MeasureString(GetItem(i)).X / 2, yPos), colour);
                yPos += 50;
            }
        }

        public void HandleKeyboard(Microsoft.Xna.Framework.Input.KeyboardState curState, Microsoft.Xna.Framework.Input.KeyboardState prevState)
        {
            if (curState.IsKeyDown(Keys.Down) && !prevState.IsKeyDown(Keys.Down))
                Iterator++;

            if (curState.IsKeyDown(Keys.Up) && !prevState.IsKeyDown(Keys.Up))
                Iterator--;

            if (curState.IsKeyDown(Keys.Enter) && !prevState.IsKeyDown(Keys.Enter))
            {
                gameConfig = new DataClasses.GameConfiguration("../../../Maps/firstLevel.xml", new SpriteObjects.Ship());
            }
        }

        public Microsoft.Xna.Framework.Matrix getView()
        {
            return base._view;
        }

        public int GetNumberOfOptions()
        {
            return MenuItems.Count;
        }

        public string GetItem(int index)
        {
            return MenuItems[index];
        }
    }
}
