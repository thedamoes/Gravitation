using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gravitation.Comms.Transmiters;

namespace Gravitation.Comms
{
    public class Server
    {
        UDPReceiver udpReciver;
        UDPTransmiter udpTransmitter;

        public Server(System.Net.IPEndPoint endpoint)
        {
            this.udpTransmitter = new UDPTransmiter(endpoint);
            this.udpReciver = new UDPReceiver(endpoint);
        }

        public void sendGamestaeUpdate(GameStates.TwoPlayerGameState state, TimeSpan atTime)
        {
            Messages.ShipStateUpdate shipStateMessage = new Messages.ShipStateUpdate((int)state.playerstate.position.X,
                                                                                     (int)state.playerstate.position.Y,
                                                                                     (int)state.player2State.position.X,
                                                                                     (int)state.player2State.position.Y,
                                                                                     state.playerstate.rotation,
                                                                                     state.player2State.rotation, atTime);

            this.udpTransmitter.sendMessage(shipStateMessage);
        }


    }
}
