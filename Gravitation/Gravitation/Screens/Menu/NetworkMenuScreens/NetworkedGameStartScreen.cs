using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Integration;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Gravitation.Screens.Menu.UserControls.BaseUserControls;
using Microsoft.Xna.Framework;

namespace Gravitation.Screens.Menu.NetworkMenuScreens
{
    class NetworkedGameStartScreen : BaseScreen, IDrawableScreen
    {
        UserControls.IPSelection IP;

        private IDrawableScreen nextScreen = null;
        private DockableControlHolder ipControl;

        public event EventHandler<DataClasses.GameSelectedEventArgs> gameSelected;

        public NetworkedGameStartScreen(int screenHeight, int screenWidth, Microsoft.Xna.Framework.Content.ContentManager cm)
            : base(screenHeight, screenWidth)
        {
            this.IP = new UserControls.IPSelection();
            ipControl = new DockableControlHolder(this.IP, screenWidth, screenHeight,400,300);
            ipControl.createControl();

            this.IP.connect_Click += this.connect_Clicked;
            this.IP.host_Click += this.hostGame_Clicked;

        }

        public void LoadContent(Microsoft.Xna.Framework.GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Microsoft.Xna.Framework.GameTime gameTime)
        {
            sb.GraphicsDevice.Clear(Color.FromNonPremultiplied(40,40,40,255));
            
        }

        public void HandleKeyboard(Microsoft.Xna.Framework.Input.KeyboardState curState, Microsoft.Xna.Framework.Input.KeyboardState prevState)
        {
            if (this.nextScreen != null)
                this.nextScreen.HandleKeyboard(curState, prevState);
            else
                this.IP.handleKeyboard(curState);

        }

        public Microsoft.Xna.Framework.Matrix getView()
        {
            return base._view;
        }


        private void connect_Clicked(object sender, EventArgs e)
        {
            Comms.Lobby.GameLobbyComms gameLobbyComms = new Comms.Lobby.GameLobbyComms("non boss man (pleb)");

            if (gameLobbyComms.tryCreateComms(this.IP.SelectedIP))
            {
                this.ipControl.destroyControl();
                this.nextScreen = new GameLobbyScreen(base.screenHeight, base.screenWidth, gameLobbyComms);
            }
            else
            {
                MessageBox.Show("Failed To Connect...");
            }
        }

        public override void windowCloseing()
        {
            base.windowCloseing();
            if (this.nextScreen != null)
                this.nextScreen.windowCloseing();
        }

        private void hostGame_Clicked(object sender, EventArgs e)
        {
            this.ipControl.destroyControl();
            this.nextScreen = new GameLobbyScreen(base.screenHeight, base.screenWidth);
            
        }
    }
}
