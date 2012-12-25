using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gravitation.Comms.Lobby;
using Gravitation.Screens.Menu.UserControls.BaseUserControls;
using Microsoft.Xna.Framework;

namespace Gravitation.Screens.Menu.NetworkMenuScreens
{
    class GameLobbyScreen : BaseScreen, IDrawableScreen
    {
        public event EventHandler<DataClasses.GameSelectedEventArgs> gameSelected;

        private GameLobbyComms comms;
        private UserControls.GameLobby lobbyControl;

        public GameLobbyScreen(int screenHeight, int screenWidth):base(screenHeight, screenWidth)
        {
            this.comms = new ServerGameLobbyComms("THE BOSSS (server man)");
            this.comms.tryCreateComms("127.0.0.1");
            initaliseGUI();
        }

        public GameLobbyScreen(int screenHeight, int screenWidth, GameLobbyComms comms)
            : base(screenHeight, screenWidth)
        {
            this.comms = comms;
            initaliseGUI();
        }

        private void initaliseGUI()
        {
            this.lobbyControl = new UserControls.GameLobby();
            DockableControlHolder ipControl = new DockableControlHolder(this.lobbyControl, screenWidth, screenHeight);
            ipControl.createControl();
            this.lobbyControl.OnsendMessage += new EventHandler<EventArgs>(lobbyControl_OnsendMessage);
            this.comms.chatReceved += new EventHandler<Comms.Lobby.EventArgs.ChatRecevedEventArgs>(comms_chatReceved);
            this.comms.playerConnected += new EventHandler<Comms.Lobby.EventArgs.PlayerConnectedEventArgs>(comms_playerConnected);
        }

        void comms_playerConnected(object sender, Comms.Lobby.EventArgs.PlayerConnectedEventArgs e)
        {
            this.lobbyControl.addPlayer(e.PlayerName);
        }

        void comms_chatReceved(object sender, Comms.Lobby.EventArgs.ChatRecevedEventArgs e)
        {
            this.lobbyControl.addChatMessage(e.line);
        }

        void lobbyControl_OnsendMessage(object sender, EventArgs e)
        {
            this.lobbyControl.addChatMessage(this.lobbyControl.message);
            this.comms.sendChat(this.lobbyControl.message);
        }

        public override void windowCloseing()
        {
            base.windowCloseing();

            this.comms.shutdown();
        }


        #region IDrawableScreen Methods

        void IDrawableScreen.LoadContent(Microsoft.Xna.Framework.GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            
        }

        void IDrawableScreen.Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }

        void IDrawableScreen.Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Microsoft.Xna.Framework.GameTime gameTime)
        {
            sb.GraphicsDevice.Clear(Color.FromNonPremultiplied(40, 40, 40, 255));
        }

        void IDrawableScreen.HandleKeyboard(Microsoft.Xna.Framework.Input.KeyboardState curState, Microsoft.Xna.Framework.Input.KeyboardState prevState)
        {
            this.lobbyControl.handleKeyboard(curState);
        }

        Microsoft.Xna.Framework.Matrix IDrawableScreen.getView()
        {
            return base._view;
        }

        #endregion

    }
}
