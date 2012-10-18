using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravitation.Comms
{
    public class OnMessageRecevedEventArgs : EventArgs
    {
        public Messages.AbstractMessage line;

        public OnMessageRecevedEventArgs(Messages.AbstractMessage line)
        {
            this.line = line;
        }
    }
}
