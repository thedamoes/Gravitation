using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravitation.Comms.Messages
{
    public abstract class AbstractMessage
    {
        byte[] rawData;
        List<byte[]> dataElements = null;
        int offset = 0;


        public AbstractMessage()
        {
            dataElements = new List<byte[]>();
        }

        public AbstractMessage(byte[] data)
        {
            this.rawData = data;
        }
        protected void readNextVal(ref int value)
        {
            if (this.offset + sizeof(int) > rawData.Length)
            {
                throw new IndexOutOfRangeException("WOW FOOL we have ran outa data U ARE READING NOTHING!!");
            }

            value = BitConverter.ToInt32(this.rawData, this.offset);
            this.offset += sizeof(int);
        }
        protected void readNextVal(ref float value)
        {
            if (this.offset + sizeof(float) > rawData.Length)
            {
                throw new IndexOutOfRangeException("WOW FOOL we have ran outa data U ARE READING NOTHING!!");
            }

            value = BitConverter.ToSingle(this.rawData, this.offset);
            this.offset += sizeof(float);
        }


        public byte[] getMessageData()
        {
            if (this.dataElements != null)
            {
                int messageLength = 0;
                foreach (byte[] ba in this.dataElements)
                    messageLength += ba.Length;

                byte[] message = new byte[messageLength];
                int arrOffset = 0;
                foreach (byte[] ba in this.dataElements)
                {
                    Array.Copy(ba, 0, message, arrOffset, ba.Length);
                    arrOffset += ba.Length;
                }

                return message;

            }
            else
                return this.rawData;
        }
        protected void addToMessage(int val)
        {
            this.dataElements.Add(BitConverter.GetBytes(val));
        }
        protected void addToMessage(float val)
        {
            this.dataElements.Add(BitConverter.GetBytes(val));
        }

    }

}
