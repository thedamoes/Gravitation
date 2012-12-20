using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravitation.Comms.Messages.LobbyMessages
{
    class NewPlayerConnected : AbstractMessage
    {
        private string playerName;

        #region Properties
        public string PlayerName
        {
            get { return this.playerName; }
        }
        #endregion

        public NewPlayerConnected(string playerName)
        {
            base.addToMessage(playerName);
        }

        public NewPlayerConnected(byte[] data) : base(data)
        {
            base.readNextVal(ref playerName);
        }

        protected override char getMessageHeader()
        {
            return (char)103;
        }
    }
}
