using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravitation.Comms.Lobby.EventArgs
{
    class ChatRecevedEventArgs : System.EventArgs
    {
        public String line;
        public ChatRecevedEventArgs(String line)
        {
            this.line = line;
        }
    }
}
