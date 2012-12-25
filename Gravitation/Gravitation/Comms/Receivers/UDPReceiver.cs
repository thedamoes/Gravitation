using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Gravitation.Comms
{
    public class UDPReceiver: AbstractReceiver
    {
        UdpClient client;

        public UDPReceiver(IPEndPoint endpt) :
            base(endpt)
        {
            this.client = new UdpClient(base.endpoint);
        }

        public override void startListening()
        {
            base.stoped = false;

            // update to use asink later
            while (!base.stoped)
            {
                if (this.client.Available > 0)
                {
                    byte[] bytes = this.client.Receive(ref base.endpoint);
                    this.fireOnMessageReceved(MessageFactory.createMessage(bytes));
                }

                Thread.Sleep(1);

            }
        }

    }
}
