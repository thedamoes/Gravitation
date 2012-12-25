using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravitation.Comms.Messages.LobbyMessages
{
    class ChatMessage : AbstractMessage
    {
        private String message;
        private String from;

        #region properties

        public String Message { get { return this.message; } }
        public String From { get { return this.from; } }

        #endregion

        public ChatMessage(String message, String from)
        {
            base.addToMessage(from);
            base.addToMessage('\0');
            base.addToMessage(message);
        }
        public ChatMessage(byte[] data) : base(data)
        {
            base.readNextVal(ref from);
            base.readNextVal(ref message);
        }

        protected override char getMessageHeader()
        {
            return (char)102;
        }
    }
}
