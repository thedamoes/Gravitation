using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XnaGUILib;
using Gravitation.Screens.Menu.UserControls;
using Gravitation.Screens.Menu.UserControls.BaseUserControls;

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
        private IDrawableScreen mNextScreen = null;

        private SpriteObjects.Ship mShip = null;

        private MenuButton single_Player_bttn, multiplayer_bttn, option_bttn , 
            singlePlayer_Swarm, singlePlayer_Dogfight, singlePlayer_Race,
            multiplayer_internet, multiplayer_local,
            button_config, sounds;

        Microsoft.Xna.Framework.Content.ContentManager cm;

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

            this.mShip = new SpriteObjects.Ship(mPlayer,25,100);

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
            this.multiplayer_local.click += this.localPlayMultiplayerSelected;
            this.multiplayer_internet.click += this.multiplayerInternetSelected;
            this.singlePlayer_Race.click += this.singleplayerRaceSelected;

            this.single_Player_bttn.highlighted += buttonUnhighlighted;
            this.multiplayer_bttn.highlighted += buttonUnhighlighted; 
            this.option_bttn.highlighted += buttonUnhighlighted;
            this.singlePlayer_Race.highlighted += buttonUnhighlighted;
            this.singlePlayer_Swarm.highlighted += buttonUnhighlighted;
            this.singlePlayer_Dogfight.highlighted += buttonUnhighlighted;
            this.multiplayer_internet.highlighted += buttonUnhighlighted;
            this.multiplayer_local.highlighted += buttonUnhighlighted;
            this.button_config.highlighted += buttonUnhighlighted;


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

            mBackground.LoadContent(cm, "Menu/menuBG", "Menu/menuBG");

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

            this.cm = cm;
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.mNextScreen == null)
            {
                this.single_Player_bttn.Update(gameTime);
                this.multiplayer_bttn.Update(gameTime);
                this.option_bttn.Update(gameTime);
                this.detailedSelectionContainer.Update(gameTime);
            }
            else
                this.mNextScreen.Update(gameTime);

        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, GameTime gameTime)
        {
            if (this.mNextScreen == null)
            {
                mBackground.Draw(sb);
                this.single_Player_bttn.Draw(sb, gameTime);
                this.multiplayer_bttn.Draw(sb, gameTime);
                this.option_bttn.Draw(sb, gameTime);
                this.detailedSelectionContainer.Draw(sb, gameTime);

                sb.DrawString(mHeaderFont, Title, new Vector2((float)(mScreenWidth * 0.10 - mFont.MeasureString(Title).X / 2), 20), Color.White);
            }
            else
                this.mNextScreen.Draw(sb, gameTime);
        }

        public override void windowCloseing()
        {
            base.windowCloseing();
            if (this.mNextScreen != null)
                this.mNextScreen.windowCloseing();
        }

        public void HandleKeyboard(Microsoft.Xna.Framework.Input.KeyboardState curState, Microsoft.Xna.Framework.Input.KeyboardState prevState)
        {
            if(this.mNextScreen != null)
            this.mNextScreen.HandleKeyboard(curState, prevState);
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

        private void dogfightSelected(object senderm, EventArgs e)
        {

           // base.fire<DataClasses.GameSelectedEventArgs>(this.gameSelected, new DataClasses.GameSelectedEventArgs(new Screens.GameTypes.SinglePlayer(
             //                                                                           new DataClasses.GameConfiguration("../../../Maps/testLevel.xml", mShip, null))));
            this.mNextScreen = new SelectShipScreen(this.screenHeight, this.mScreenWidth, typeof(Screens.GameTypes.SinglePlayer), mPlayer, 2);
            this.mNextScreen.gameSelected += delegate(object sender, DataClasses.GameSelectedEventArgs args) { base.fire<DataClasses.GameSelectedEventArgs>(this.gameSelected, args); };
        }

        private void singleplayerRaceSelected(object senderm, EventArgs e)
        {
            this.mNextScreen = new SelectShipScreen(this.screenHeight, this.mScreenWidth,typeof(Screens.GameTypes.SinglePlayer),mPlayer,2);
            this.mNextScreen.gameSelected += delegate(object sender, DataClasses.GameSelectedEventArgs args) { base.fire<DataClasses.GameSelectedEventArgs>(this.gameSelected, args); };
        }

        private void localPlayMultiplayerSelected(object senderm, EventArgs e)
        {
            this.mNextScreen = new SelectShipScreen(this.screenHeight, this.mScreenWidth, typeof(Screens.GameTypes.DogFight), mPlayer, 2);
            this.mNextScreen.gameSelected += delegate(object sender, DataClasses.GameSelectedEventArgs args) { base.fire<DataClasses.GameSelectedEventArgs>(this.gameSelected, args); };
        }

        private void multiplayerInternetSelected(object senderm, EventArgs e)
        {
            this.mNextScreen = new NetworkMenuScreens.NetworkedGameStartScreen(this.screenHeight, this.mScreenWidth, this.cm);
            this.mNextScreen.gameSelected += delegate(object sender, DataClasses.GameSelectedEventArgs args) { base.fire<DataClasses.GameSelectedEventArgs>(this.gameSelected, args); };
        }
        #endregion

        private void buttonUnhighlighted(object sender, EventArgs e)
        {
            this.mPlayer.playSound(SoundHandler.Sounds.MOVE_MENU);
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
