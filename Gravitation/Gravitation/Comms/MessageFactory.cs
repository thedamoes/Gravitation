﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravitation.Comms
{
    /*
     * 101 = Ship status
     */
    static class MessageFactory
    {
        public static Messages.AbstractMessage createMessage(byte[] data)
        {
            if (data.Length <= 2) // not a valid message because not even big enough for header
                return null;

            byte[] messageData = new byte[data.Length-2];
            Array.Copy(data,2,messageData,0,data.Length-2);

            switch (BitConverter.ToUInt16(data, 0))
            {
                case 101:
                    return new Messages.ShipStateUpdate(messageData);
                    break;
                default: // throw away unknowmn messages
                    return null;
                    break;
            }



        }
    }
}
