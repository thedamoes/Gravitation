using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gravitation.Comms.Transmiters;
using Gravitation.Comms.Transevers;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using Gravitation.Comms.Messages;
using Gravitation.Comms.Messages.LobbyMessages;

namespace Gravitation.Comms.Lobby
{
    class GameLobbyComms
    {
        public event EventHandler<EventArgs.ChatRecevedEventArgs> chatReceved;
        public event EventHandler<EventArgs.PlayerConnectedEventArgs> playerConnected;

        private const int PORT = 50002;
        private AbstractTransever transever;
        protected AbstractTransever Transever
        {
            set
            { 
                this.transever = value;
                this.reconnectTransever();
            }
        }
        private Regex ipRegexp = new Regex("[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}");
        private String myName;

        public GameLobbyComms(String myName)
        {
            this.myName = myName;
        }

        public virtual bool tryCreateComms(string ip)
        {
            IPAddress address = this.parseStringToIP(ip);

            if (address == null)
            {
                MessageBox.Show("IP of invalid format");
                return false;
            }

            IPEndPoint endpoint = new IPEndPoint(address,GameLobbyComms.PORT);
            this.Transever = new TCPTransever(endpoint);
            this.transever.startListening();

            return this.transever.Connected;
        }
        public void sendChat(String message)
        {
            this.baseSend(new ChatMessage(message,myName));
        }
        
        public void reconnectTransever()
        {
            this.transever.onMessageRecieved += new EventHandler<OnMessageRecevedEventArgs>(transever_onMessageRecieved);
        }

        void transever_onMessageRecieved(object sender, OnMessageRecevedEventArgs e)
        {
           if(e.line is ChatMessage)
           {
               ChatMessage mes = e.line as ChatMessage;
               this.fire<EventArgs.ChatRecevedEventArgs>(this.chatReceved, new EventArgs.ChatRecevedEventArgs(mes.Message));
           }
        }

        private void baseSend(AbstractMessage message)
        {
            if (this.transever != null)
            {
                this.transever.sendMessage(message);
            }
        }

        protected void fireNewPlayerConnected(EventArgs.PlayerConnectedEventArgs a)
        {
            this.fire<EventArgs.PlayerConnectedEventArgs>(this.playerConnected, a);
        }

        public virtual void shutdown()
        {
            this.transever.stopListening();
        }

        protected void fire<A>(EventHandler<A> evnt, A args) where A : System.EventArgs
        {
            if (evnt != null)
                evnt(this, args);
        }
        private IPAddress parseStringToIP(String ip)
        {
            if (this.ipRegexp.IsMatch(ip))
            {
                string[] bits = ip.Split('.');
                byte[] address = {
                byte.Parse(bits[0]),
                byte.Parse(bits[1]),
                byte.Parse(bits[2]),
                byte.Parse(bits[3])};

                return new IPAddress(address);
            }
            return null;
        }
    }
}
