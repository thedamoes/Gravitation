using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravitation.Comms.Messages
{
    public class ShipStateUpdate : AbstractMessage
    {

        int shipx;
        int shipy;

        int offset = 0;

        public ShipStateUpdate(byte[] data)
            : base(data)
        {
            this.shipx = this.readNextVal();
            this.shipy = this.readNextVal();
        }
    }
}
