using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gravitation.Comms;
using System.Net;
using System.Threading;

namespace Gravitation.ControllerAgents
{
    public class NetworkedAgent : IControllerInterface
    {
        AbstractReceiver receiver;
        SpriteObjects.Ship ship;

        public NetworkedAgent(System.Net.IPEndPoint endpoint, SoundHandler handler)
        {
#if DEBUG
            this.receiver = new UDPReceiver(new System.Net.IPEndPoint(IPAddress.Loopback, 45001)); // localhost on port 45001
#else
            this.receiver = new UDPReceiver(endpoint);
#endif
            this.receiver.onMessageRecieved += this.playerUpdateReceived;
            this.ship = new SpriteObjects.Ship(handler, 0, 0);
        }

        ~NetworkedAgent()
        {
            receiver.shutdownComms();
        }

        public void applyMovement()
        {
           // ignore this because we wont use it
        }

        public void loadShip(Microsoft.Xna.Framework.Content.ContentManager cm, Microsoft.Xna.Framework.GraphicsDeviceManager graphics)
        {
            // may need to load a ship will think about it later

            Thread commsThread = new Thread(new ThreadStart(receiver.startComms));
            commsThread.Start();
        }


        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sBatch)
        {
            this.ship.Draw(sBatch);
        }

        private void playerUpdateReceived(object sender, OnMessageRecevedEventArgs e)
        {
            
        }
    }
}
