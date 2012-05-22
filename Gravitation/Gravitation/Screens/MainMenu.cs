using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XnaGUILib;

namespace Gravitation.Screens
{
    class MainMenu: BaseScreen, IDrawableScreen
    {

        private List<string> MenuItems;
        private enum listItems { RACE = 0, SWARM = 1, VERSES = 2, NETWORKED = 3, SHIP = 4,  EXIT = 5 };
        private int iterator;
        public string InfoText { get; set; }
        public string Title { get; set; }

        private SpriteFont mFont;
        private SpriteFont mHeaderFont;

        private SpriteObjects.Sprite mBackground;
        private SpriteObjects.Sprite mSelectedBackground;

        private int mScreenWidth;

        private DataClasses.GameConfiguration mGameConfig = null;
        private DataClasses.DisplayNewScreen mNextScreen = null;

        private SpriteObjects.Ship mShip = null;

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

        public MainMenu(int ScreenWidth, int ScreenHeight, SoundHandler player)
            : base(ScreenWidth, ScreenHeight)
        {
            this.mPlayer = player;
            this.mScreenWidth = ScreenWidth;
            Title = "Gravitation";
            MenuItems = new List<string>();
            MenuItems.Add("Race");
            MenuItems.Add("Swarm");
            MenuItems.Add("Verses");
            MenuItems.Add("Networked");
            MenuItems.Add("Ship");
            MenuItems.Add("Exit Game");
            Iterator = 0;
            InfoText = string.Empty;

            createBackground(ref mBackground, 0.5f, 0.5f);
            createBackground(ref mSelectedBackground, 0.4f, 0.3f);

            this.mShip = new SpriteObjects.Ship(mPlayer);
            
        }

        public MainMenu(int ScreenWidth, int ScreenHeight, SoundHandler player, SpriteObjects.Ship selectedShip)
            : base(ScreenWidth, ScreenHeight)
        {
            this.mPlayer = player;
            this.mScreenWidth = ScreenWidth;
            Title = "Gravitation";
            MenuItems = new List<string>();
            MenuItems.Add("Race");
            MenuItems.Add("Swarm");
            MenuItems.Add("Verses");
            MenuItems.Add("Networked");
            MenuItems.Add("Ship");
            MenuItems.Add("Exit Game");
            Iterator = 0;
            InfoText = string.Empty;

            createBackground(ref mBackground, 0.5f, 0.5f);
            createBackground(ref mSelectedBackground, 0.4f, 0.3f);

            this.mShip = selectedShip;

        }

        public void LoadContent(Microsoft.Xna.Framework.GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            mFont = cm.Load<SpriteFont>("font");
            mHeaderFont = cm.Load<SpriteFont>("Header");

            mBackground.LoadContent(cm,"Menu/menuBG");
            mSelectedBackground.LoadContent(cm, "Menu/SelectedBackground");
        }

        public DataClasses.IScreenExitData Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (mNextScreen != null)
                return mNextScreen;
            else if (mGameConfig != null)
                return mGameConfig;

            else return null;
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, GameTime gameTime)
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

            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void HandleKeyboard(Microsoft.Xna.Framework.Input.KeyboardState curState, Microsoft.Xna.Framework.Input.KeyboardState prevState)
        {

            if (curState.IsKeyDown(Keys.Down) && !prevState.IsKeyDown(Keys.Down))
                Iterator++;

            if (curState.IsKeyDown(Keys.Up) && !prevState.IsKeyDown(Keys.Up))
                Iterator--;

            if (curState.IsKeyDown(Keys.Enter) && !prevState.IsKeyDown(Keys.Enter))
            {
                switch(Iterator)
                {
                    case (int)listItems.RACE:
                        {
                            mNextScreen = new DataClasses.DisplayNewScreen(new Screens.GameTypes.SinglePlayer(
                                                                                        new DataClasses.GameConfiguration("../../../Maps/level1.xml", mShip,null)));
                        }
                        break;
                    case (int)listItems.SWARM:
                        {
                            mNextScreen = new DataClasses.DisplayNewScreen(new Screens.GameTypes.SinglePlayer(
                                                                                        new DataClasses.GameConfiguration("../../../Maps/test.xml", mShip,null)));
                        }
                        break;
                    case (int)listItems.VERSES:
                        {
                            mNextScreen = new DataClasses.DisplayNewScreen(new Screens.GameTypes.DogFight(
                                                                                        new DataClasses.GameConfiguration("../../../Maps/firstLevel.xml", mShip,new SpriteObjects.Ship(mPlayer))));
                        }
                        break;
                    case (int)listItems.NETWORKED:
                        {
                            mNextScreen = new DataClasses.DisplayNewScreen(new Screens.GameTypes.SinglePlayer(
                                                                                         new DataClasses.GameConfiguration("../../../Maps/firstLevel.xml", mShip,null)));
                        }
                        break;
                    case (int)listItems.SHIP:
                        {
                            mNextScreen = new DataClasses.DisplayNewScreen(new Screens.SelectShipScreen(screenHeight, mScreenWidth));
                        }
                        break;
                    case (int)listItems.EXIT:
                        {
                            //Exit(); need to find some way of doing this 
                        }
                        break;
                    default: { }
                        break;
                }
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
