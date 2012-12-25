using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Gravitation.Comms.Transevers
{
    class MessageRecevedStatestruct
    {
        public NetworkStream clientStream;
        public byte[] message;
    }

    class TCPTransever : AbstractTransever
    {
        TcpClient client;
        NetworkStream clientStream;
        ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

        byte[] recvBuffer = new byte[1024];

        public override bool Connected
        {
            get
            {
                if (this.client == null)
                    return false;
                else
                    return this.client.Connected;
            }
        }

        public TCPTransever(IPEndPoint endpt):base(endpt)
        {
            
        }

        public override void startListening()
        {
            try
            {
                if (!this.Connected)
                {
                    this.stoped = false;
                    this.client = new TcpClient();
                    this.client.Connect(this.endpoint);
                    this.clientStream = this.client.GetStream();

                    Thread workerThread = new Thread(this.messageListenerWorker);
                    workerThread.Start();
                }
            }
            catch (System.Net.Sockets.SocketException e)
            {
               
            }
            
        }

        public void startListening(TcpClient client)
        {
            if (!this.Connected)
            {
                this.stoped = false;
                this.client = client;
                this.clientStream = this.client.GetStream();

                Thread workerThread = new Thread(this.messageListenerWorker);
                workerThread.Start();
            }
        }

        public override void stopListening()
        {
            base.stopListening();
            this.tcpClientConnected.Set();
        }


        public void messageListenerWorker()
        {
            while (!this.stoped)
            {
                tcpClientConnected.Reset();
                MessageRecevedStatestruct state = new MessageRecevedStatestruct();
                state.clientStream = this.clientStream;
                state.message = this.recvBuffer;
                try { this.clientStream.BeginRead(recvBuffer, 0, recvBuffer.Length, new AsyncCallback(this.messageReceved), state); }
                catch (System.IO.IOException e) { break; }
                tcpClientConnected.WaitOne();
            }
        }


        private void messageReceved(IAsyncResult res)
        {
            MessageRecevedStatestruct message = res.AsyncState as MessageRecevedStatestruct;
            if (res.IsCompleted)
            {
                if (message != null)
                {
                    this.fireOnMessageReceved(MessageFactory.createMessage(message.message));
                }
            }
            try
            {
                message.clientStream.EndRead(res);
            }
            catch (System.IO.IOException e)
            {
            }
            tcpClientConnected.Set();
        }

        public override void sendMessage(Messages.AbstractMessage message)
        {
            byte[] messageData = message.getMessageData();
            this.clientStream.Write(messageData, 0, messageData.Length);
        }

    }
}
