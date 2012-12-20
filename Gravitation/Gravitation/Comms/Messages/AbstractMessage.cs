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
        int offset =0;


        public AbstractMessage()
        {
            dataElements = new List<byte[]>();
        }

        public AbstractMessage(byte[] data)
        {
            this.rawData = data;
        }

        #region value getters
        protected void readNextVal(ref int value)
        {
            if (this.offset + sizeof(char) > rawData.Length)
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
        protected void readNextVal(ref char value)
        {
            if (this.offset + sizeof(char) > rawData.Length)
            {
                throw new IndexOutOfRangeException("WOW FOOL we have ran outa data U ARE READING NOTHING!!");
            }

            value = BitConverter.ToChar(this.rawData, this.offset);
            this.offset += sizeof(char);
        }
        protected void readNextVal(ref string value)
        {
            char c = ' ';

            while(true)
            {
                this.readNextVal(ref c);
                if (c == '\0')
                    break;
                value += c;
            }
            this.offset += sizeof(char);
        }
        #endregion

        public byte[] getMessageData()
        {
            if (this.dataElements != null)
            {
                int messageLength = 0;
                foreach (byte[] ba in this.dataElements)
                    messageLength += ba.Length;

                byte[] message = new byte[messageLength + sizeof(int)]; // + sizeof(int) because of the header
                byte[] headerBytes = BitConverter.GetBytes(this.getMessageHeader());
                Array.Copy(headerBytes, 0, message, 0, headerBytes.Length);
                int arrOffset = sizeof(char);
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

        #region Message Adderrs
        protected void addToMessage(int val)
        {
            this.dataElements.Add(BitConverter.GetBytes(val));
        }
        protected void addToMessage(float val)
        {
            this.dataElements.Add(BitConverter.GetBytes(val));
        }
        protected void addToMessage(string val)
        {
            char[] valarr = val.ToCharArray();
            foreach(char c in valarr)
            {
                this.dataElements.Add(BitConverter.GetBytes(c));
            }
        }
        #endregion

        protected abstract char getMessageHeader();

    }

}
