using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Gravitation.Comms
{
    public abstract class AbstractReceiver
    {
        public event EventHandler<OnMessageRecevedEventArgs> onMessageRecieved;
        protected IPEndPoint endpoint;

        protected bool stoped = true;

        public AbstractReceiver(IPEndPoint endpoint)
        {
            this.endpoint = endpoint;
        }

        public abstract void startComms();
        public void shutdownComms()
        {
            this.stoped = true;
        }

        protected void fireOnMessageReceved(Messages.AbstractMessage ss)
        {
            this.fire<OnMessageRecevedEventArgs>(this.onMessageRecieved, new OnMessageRecevedEventArgs(ss));
        }

        protected void fire<A>(EventHandler<A> evnt, A args) where A : EventArgs
        {
            if (evnt != null)
                evnt(this, args);
        }
    }
}
