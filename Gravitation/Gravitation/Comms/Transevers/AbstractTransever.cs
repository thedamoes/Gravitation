using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Gravitation.Comms.Transmiters;
using Gravitation.Comms.Receivers;

namespace Gravitation.Comms.Transevers
{
    public abstract class AbstractTransever : ITransmitter, IReceiver
    {
        public event EventHandler<OnMessageRecevedEventArgs> onMessageRecieved;
        public abstract bool Connected
        {
            get;
        }

        protected IPEndPoint endpoint;

        protected bool stoped = true;

        public AbstractTransever(IPEndPoint endpoint)
        {
            this.endpoint = endpoint;
        }

        public AbstractTransever(int port)
        {
            this.endpoint = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), port);
        }

        public abstract void startListening();
        public virtual void stopListening()
        {
            this.stoped = true;
        }

        public abstract void sendMessage(Messages.AbstractMessage message);
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
