using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravitation.Comms.Transmiters
{
    interface ITransmitter
    {
        void sendMessage(Messages.AbstractMessage message);
    }
}
