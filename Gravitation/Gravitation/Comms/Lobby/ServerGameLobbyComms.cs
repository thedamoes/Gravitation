using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gravitation.Comms.Messages;
using Gravitation.Comms.Transevers;
using Gravitation.Comms.Transmiters;

namespace Gravitation.Comms.Lobby
{
    class ServerGameLobbyComms : GameLobbyComms
    {
        private TCPServerTransever serverTransever;

        public ServerGameLobbyComms(String myName)
            : base(myName)
        {
           
        }

        public override bool tryCreateComms(string ip)
        {
            this.serverTransever = new TCPServerTransever(50002);
            this.serverTransever.startListeningForConnections();

            this.serverTransever.newConnectionMade += new EventHandler<Comms.EventArgs.ReceiverHolderEventArgs>(serverTransever_newConnectionMade);
            base.Transever = this.serverTransever;
            return true;
        }

        public override void shutdown()
        {
            base.shutdown();
            serverTransever.stopListeningForConnections();
        }

        void serverTransever_newConnectionMade(object sender, Comms.EventArgs.ReceiverHolderEventArgs e)
        {
            String playerName = e.rec.getEndpoint().Address.ToString();
            this.serverTransever.sendMessage(new Messages.LobbyMessages.NewPlayerConnected(playerName));
            base.fireNewPlayerConnected(new EventArgs.PlayerConnectedEventArgs(playerName));
        }


    }
}
