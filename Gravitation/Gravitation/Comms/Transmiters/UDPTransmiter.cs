using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Gravitation.Comms.Transmiters
{
    public class UDPTransmiter : AbstractTransmitter
    {
        UdpClient client;

        public UDPTransmiter(IPEndPoint endpoint) : base (endpoint)
        {
            this.client = new UdpClient();
        }

        public override void sendMessage(Messages.AbstractMessage message)
        {
            byte[] messageBytes = message.getMessageData();
            this.client.Send(messageBytes, messageBytes.Length, base.endpoint);
        }
    }
}
