using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravitation.Comms.Messages
{
    public abstract class AbstractMessage
    {
        byte[] rawData;
        int offset = 0;

        public AbstractMessage(byte[] data)
        {
            this.rawData = data;
        }

        protected int readNextVal()
        {
            if (this.offset + sizeof(int) > rawData.Length)
            {
                throw new IndexOutOfRangeException("WOW FOOL we have ran outa data U ARE READING NOTHING!!");
            }

            return BitConverter.ToInt32(this.rawData, this.offset);
        }

    }

}
