using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace Gravitation.Comms.Transevers
{
    class TCPServerTransever : AbstractServerTransever
    {
        public override bool Connected
        {
            get { return true; }
        }

        private TcpListener listener;
        private ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

        private bool stoptListeningForConnections = true;

        public TCPServerTransever( int portNumber) : base(portNumber)
        {
            this.listener = new TcpListener(IPAddress.Any, portNumber);
        }

        public override void startListeningForConnections()
        {
            if (this.stoptListeningForConnections)
            {
                this.stoptListeningForConnections = false;

                Thread workerThread = new Thread(this.connectionListenerWorker);
                workerThread.Start();
            }
        }

        public override void stopListeningForConnections()
        {
            this.stoptListeningForConnections = true;
            tcpClientConnected.Set();
        }

        public override void stopListening()
        {
            base.stopListening();
            this.tcpClientConnected.Set();
        }

        public override void startListening()
        {
            
        }

        public override void sendMessage(Messages.AbstractMessage message)
        {
            foreach (AbstractTransever trans in base.connections)
            {
                trans.sendMessage(message);
            }
        }

        public void connectionListenerWorker()
        {
            this.listener.Start();
            while (!this.stoptListeningForConnections)
            {
                tcpClientConnected.Reset();
                listener.BeginAcceptTcpClient(new AsyncCallback(this.DoAcceptTcpClientCallback), listener);
                tcpClientConnected.WaitOne();
            }
            this.listener.Stop();
        }

        public void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            try
            {
                TcpListener listener = (TcpListener)ar.AsyncState;
                TcpClient client = listener.EndAcceptTcpClient(ar);

                TCPTransever transever = new TCPTransever(new System.Net.IPEndPoint(0, 0));
                transever.startListening(client);
                transever.onMessageRecieved += new EventHandler<OnMessageRecevedEventArgs>(transever_onMessageRecieved);
                base.newConnection(transever);

                tcpClientConnected.Set();
            }
            catch (System.ObjectDisposedException)
            {
                if (this.stoptListeningForConnections)
                    return;
                else
                    throw;
            }

           
        }

        void transever_onMessageRecieved(object sender, OnMessageRecevedEventArgs e)
        {
            this.fireOnMessageReceved(e.line);
        }

       
    }
}
