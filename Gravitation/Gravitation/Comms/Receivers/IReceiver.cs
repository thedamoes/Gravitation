using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Gravitation.Comms.Receivers
{
    public interface IReceiver
    {
        event EventHandler<OnMessageRecevedEventArgs> onMessageRecieved;

        void startListening();
        void stopListening();

        IPEndPoint getEndpoint();
    }
}
