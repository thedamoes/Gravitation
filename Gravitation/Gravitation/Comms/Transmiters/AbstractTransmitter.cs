using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Gravitation.Comms.Transmiters
{
    public abstract class AbstractTransmitter : ITransmitter
    {
        protected IPEndPoint endpoint;
        protected bool stoped = true;

        public AbstractTransmitter(IPEndPoint endpoint)
        {
            this.endpoint = endpoint;
        }

        public abstract void sendMessage(Messages.AbstractMessage message);
    }
}
