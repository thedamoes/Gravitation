using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravitation.Comms.Messages
{
    public class ShipStateUpdate : AbstractMessage
    {

        TimeSpan timestamp;

        int ship1x;
        int ship1y;
        float ship1rotateion;

        int ship2x;
        int ship2y;
        float ship2rotateion;

        int offset = 0;

        public ShipStateUpdate(int shipx, int shipy, int ship2x, int ship2y, float ship1Rot, float ship2Rot,TimeSpan timestamp)
        {
            this.addToMessage(timestamp.Hours);
            this.addToMessage(timestamp.Minutes);
            this.addToMessage(timestamp.Seconds);
            this.addToMessage(timestamp.Milliseconds);

            this.addToMessage(shipx);
            this.addToMessage(shipy);
            this.addToMessage(ship2x);
            this.addToMessage(ship2y);

            this.addToMessage(ship1Rot);
            this.addToMessage(ship2Rot);
        }

        public ShipStateUpdate(byte[] data)
            : base(data)
        {
            int hours=0, min=0,sec=0,miliSec =0;

            this.readNextVal(ref hours);
            this.readNextVal(ref min);
            this.readNextVal(ref sec);
            this.readNextVal(ref miliSec);

            this.timestamp = new TimeSpan(0, hours, min, sec, miliSec);

            this.readNextVal(ref this.ship1x);
            this.readNextVal(ref this.ship1y);

            this.readNextVal(ref this.ship2x);
            this.readNextVal(ref this.ship2y);

            this.readNextVal(ref this.ship1rotateion);
            this.readNextVal(ref this.ship2rotateion);
        }

        protected override char getMessageHeader()
        {
            return (char)101;
        }
    }
}
