using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gravitation.Comms.Transmiters;
using Gravitation.Comms.Receivers;
using System.Collections.Concurrent;

namespace Gravitation.Comms.Transevers
{
    abstract class AbstractServerTransever : AbstractTransever
    {
        public event EventHandler<EventArgs.ReceiverHolderEventArgs> newConnectionMade;

        protected BlockingCollection<AbstractTransever> connections = new BlockingCollection<AbstractTransever>();

        public AbstractServerTransever(int portNumber) : base(portNumber)
        {

        }

        protected void newConnection(AbstractTransever transever)
        {
            this.connections.Add(transever);
            this.fire<EventArgs.ReceiverHolderEventArgs>(this.newConnectionMade, new EventArgs.ReceiverHolderEventArgs(transever));    
        }

        public override void  stopListening()
        {
 	         base.stopListening();

            foreach(AbstractTransever transever in this.connections)
            {
                transever.stopListening();
            }
        }

        public abstract void startListeningForConnections();
        public abstract void stopListeningForConnections();
    }
}
