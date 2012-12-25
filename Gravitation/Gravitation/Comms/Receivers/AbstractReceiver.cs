using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Gravitation.Comms.Receivers;

namespace Gravitation.Comms
{
    public abstract class AbstractReceiver : IReceiver
    {
        public event EventHandler<OnMessageRecevedEventArgs> onMessageRecieved;
        protected IPEndPoint endpoint;

        protected bool stoped = true;

        public AbstractReceiver(IPEndPoint endpoint)
        {
            this.endpoint = endpoint;
        }

        public abstract void startListening();
        public void stopListening()
        {
            this.stoped = true;
        }

        protected void fireOnMessageReceved(Messages.AbstractMessage ss)
        {
            this.fire<OnMessageRecevedEventArgs>(this.onMessageRecieved, new OnMessageRecevedEventArgs(ss));
        }

        protected void fire<A>(EventHandler<A> evnt, A args) where A : System.EventArgs
        {
            if (evnt != null)
                evnt(this, args);
        }


        public IPEndPoint getEndpoint()
        {
            return this.endpoint;
        }
    }
}
