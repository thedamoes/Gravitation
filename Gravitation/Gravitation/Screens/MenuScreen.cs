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
        private SpriteFont mHeaderFont;

        private SpriteObjects.Sprite mBackground;
        private SpriteObjects.Sprite mSelectedBackground;

        private int mScreenWidth;

        private DataClasses.GameConfiguration gameConfig = null;
        private SoundHandler mPlayer;

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
                mPlayer.playSound(SoundHandler.Sounds.MOVE_MENU);

            }
        }

        public MenuScreen(int ScreenWidth, int ScreenHeight, SoundHandler player) :base(ScreenWidth, ScreenHeight)
        {
            this.mPlayer = player;
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

            createBackground(ref mBackground, 0.5f, 0.5f);
            createBackground(ref mSelectedBackground, 0.4f, 0.3f);
        }

        public void LoadContent(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Microsoft.Xna.Framework.GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            mFont = cm.Load<SpriteFont>("font");
            mHeaderFont = cm.Load<SpriteFont>("Header");

            mBackground.LoadContent(cm,"Menu/menuBG");
            mSelectedBackground.LoadContent(cm, "Menu/SelectedBackground");
        }

        public DataClasses.GameConfiguration Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            return gameConfig;
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            mBackground.Draw(sb);
            sb.DrawString(mHeaderFont, Title, new Vector2((float)(mScreenWidth * 0.10 - mFont.MeasureString(Title).X / 2), 20), Color.White);
            int yPos = 100;
            for (int i = 0; i < GetNumberOfOptions(); i++)
            {
                Color colour = Color.White;
                if (i == Iterator)
                {
                    mSelectedBackground.Draw(sb);
                    mSelectedBackground.spriteOrigin = new Vector2(-(float)(mScreenWidth + 200f), -(float)(yPos * 3.3));
                }
                sb.DrawString(mFont, GetItem(i), new Vector2((float)(mScreenWidth / 1.5 - mFont.MeasureString(GetItem(i)).X / 2), yPos), colour);
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

        private void createBackground(ref SpriteObjects.Sprite back, float scaleX, float scaleY)
        {
            Vector2 backPos = new Vector2( 0, 0);

            float backScaleX = scaleX;
            float backScaleY = scaleY;

            float spriteRotation = 0f;

            back = new SpriteObjects.Sprite(backPos, spriteRotation);
            back.WidthScale = backScaleX;
            back.HeightScale = backScaleY;
        }
    }
}
