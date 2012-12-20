using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gravitation.Comms.Transevers;

namespace Gravitation.Comms.Lobby.EventArgs
{
    public class PlayerConnectedEventArgs : System.EventArgs
    {
        public String PlayerName;

        public PlayerConnectedEventArgs(string playerName)
        {
            this.PlayerName = playerName;
        }
    }
}
