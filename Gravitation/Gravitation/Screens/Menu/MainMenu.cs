﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XnaGUILib;

namespace Gravitation.Screens.Menu
{
    class MainMenu: BaseScreen, IDrawableScreen
    {
        public event EventHandler<DataClasses.GameSelectedEventArgs> gameSelected;
        public string Title { get; set; }

        private SpriteFont mFont;
        private SpriteFont mHeaderFont;

        private SpriteObjects.Sprite mBackground;

        private int mScreenWidth;

        private DataClasses.GameConfiguration mGameConfig = null;
        private DataClasses.DisplayNewScreen mNextScreen = null;

        private SpriteObjects.Ship mShip = null;

        private MenuButton single_Player_bttn, multiplayer_bttn, option_bttn , 
            singlePlayer_Swarm, singlePlayer_Dogfight, singlePlayer_Race,
            multiplayer_internet, multiplayer_local,
            button_config, sounds;

        private MenuL2Selection detailedSelectionContainer;
        private MenuButton[] singlePlayerL2Buttons = new MenuButton[3];
        private MenuButton[] multiPlayerL2Buttons = new MenuButton[2];
        private MenuButton[] optionsL2Buttons = new MenuButton[2];

        private SoundHandler mPlayer;

        public MainMenu(int ScreenWidth, int ScreenHeight, SoundHandler player)
            : base(ScreenWidth, ScreenHeight)
        {
            this.mPlayer = player;
            this.mScreenWidth = ScreenWidth;
            Title = "Gravitation";

            createBackground(ref mBackground, 0.5f, 0.5f);

            this.mShip = new SpriteObjects.Ship(mPlayer);

            this.createMenuButtons();
            this.detailedSelectionContainer = new MenuL2Selection();
            
        }

        public MainMenu(int ScreenWidth, int ScreenHeight, SoundHandler player, SpriteObjects.Ship selectedShip)
            : base(ScreenWidth, ScreenHeight)
        {
            this.mPlayer = player;
            this.mScreenWidth = ScreenWidth;
            Title = "Gravitation";

            createBackground(ref mBackground, 0.5f, 0.5f);

            this.mShip = selectedShip;

            this.createMenuButtons();
            this.detailedSelectionContainer = new MenuL2Selection();

        }

        private void createMenuButtons()
        {
            float buttonScale = 0.4f;
            single_Player_bttn = new MenuButton("single Player", new Vector2(100, 100), buttonScale, buttonScale);
            multiplayer_bttn = new MenuButton("Multi Player", new Vector2(100, 200), buttonScale, buttonScale);
            option_bttn = new MenuButton("Options", new Vector2(100, 300), buttonScale, buttonScale);

            // will do this in a proper panel later.. cbs
            this.singlePlayer_Dogfight = new MenuButton("Dogfight", new Vector2(400, 100), buttonScale, buttonScale);
            this.singlePlayer_Race = new MenuButton("Race", new Vector2(400, 150), buttonScale, buttonScale);
            this.singlePlayer_Swarm = new MenuButton("Swarm", new Vector2(400, 200), buttonScale, buttonScale);

            this.multiplayer_internet = new MenuButton("Online", new Vector2(400, 200), buttonScale, buttonScale);
            this.multiplayer_local = new MenuButton("Local", new Vector2(400, 250), buttonScale, buttonScale);

            this.button_config = new MenuButton("Button Configuration", new Vector2(400, 300), buttonScale, buttonScale);
            this.sounds = new MenuButton("sound", new Vector2(400, 350), buttonScale, buttonScale);

            this.single_Player_bttn.click += this.SinglePlayerSelected;
            this.multiplayer_bttn.click += this.multiplayerSelected;
            this.option_bttn.click += this.optionsSelected;
            this.singlePlayer_Dogfight.click += this.dogfightSelected;

            this.singlePlayerL2Buttons[0] = this.singlePlayer_Dogfight;
            this.singlePlayerL2Buttons[1] = this.singlePlayer_Race;
            this.singlePlayerL2Buttons[2] = this.singlePlayer_Swarm;

            this.multiPlayerL2Buttons[0] = this.multiplayer_internet;
            this.multiPlayerL2Buttons[1] = this.multiplayer_local;

            this.optionsL2Buttons[0] = this.button_config;
            this.optionsL2Buttons[1] = this.sounds;
        }

        public void LoadContent(Microsoft.Xna.Framework.GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            mFont = cm.Load<SpriteFont>("font");
            mHeaderFont = cm.Load<SpriteFont>("Header");

            mBackground.LoadContent(cm,"Menu/menuBG");

            // main buttons
            this.single_Player_bttn.LoadContent(cm, "Menu/SelectedBackground");
            this.multiplayer_bttn.LoadContent(cm, "Menu/SelectedBackground");
            this.option_bttn.LoadContent(cm, "Menu/SelectedBackground");

            //L2 buttons

            this.singlePlayer_Dogfight.LoadContent(cm, "Menu/SelectedBackground");
            this.singlePlayer_Race.LoadContent(cm, "Menu/SelectedBackground");
            this.singlePlayer_Swarm.LoadContent(cm, "Menu/SelectedBackground");
            this.multiplayer_internet.LoadContent(cm, "Menu/SelectedBackground");
            this.multiplayer_local.LoadContent(cm, "Menu/SelectedBackground");
            this.sounds.LoadContent(cm, "Menu/SelectedBackground");
            this.button_config.LoadContent(cm, "Menu/SelectedBackground");
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            this.single_Player_bttn.Update(gameTime);
            this.multiplayer_bttn.Update(gameTime);
            this.option_bttn.Update(gameTime);
            this.detailedSelectionContainer.Update(gameTime);

        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, GameTime gameTime)
        {
            mBackground.Draw(sb);
            this.single_Player_bttn.Draw(sb,gameTime);
            this.multiplayer_bttn.Draw(sb, gameTime);
            this.option_bttn.Draw(sb, gameTime);
            this.detailedSelectionContainer.Draw(sb,gameTime);

            sb.DrawString(mHeaderFont, Title, new Vector2((float)(mScreenWidth * 0.10 - mFont.MeasureString(Title).X / 2), 20), Color.White);
        }

        public void HandleKeyboard(Microsoft.Xna.Framework.Input.KeyboardState curState, Microsoft.Xna.Framework.Input.KeyboardState prevState)
        {

            /*if (curState.IsKeyDown(Keys.Down) && !prevState.IsKeyDown(Keys.Down))
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
                                                                                        new DataClasses.GameConfiguration("../../../Maps/level1.xml", mShip, new SpriteObjects.Ship(mPlayer))));
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
                            mNextScreen = new DataClasses.DisplayNewScreen(new Screens.Menu.SelectShipScreen(screenHeight, mScreenWidth));
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
            }*/
        }

        public Microsoft.Xna.Framework.Matrix getView()
        {
            return base._view;
        }

        #region Button Click Handlers
        private void SinglePlayerSelected(object sender, EventArgs e)
        {
            this.detailedSelectionContainer.setView(this.singlePlayerL2Buttons);
        }

        private void multiplayerSelected(object sender, EventArgs e)
        {
            this.detailedSelectionContainer.setView(this.multiPlayerL2Buttons);
        }

        private void optionsSelected(object sender, EventArgs e)
        {
            this.detailedSelectionContainer.setView(this.optionsL2Buttons);
        }

        private void dogfightSelected(object sender, EventArgs e)
        {
            base.fire<DataClasses.GameSelectedEventArgs>(this.gameSelected, new DataClasses.GameSelectedEventArgs(new Screens.GameTypes.SinglePlayer(
                                                                                        new DataClasses.GameConfiguration("../../../Maps/level1.xml", mShip, null))));
        }
        #endregion

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
