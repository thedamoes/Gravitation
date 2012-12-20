using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gravitation.Comms.Receivers;

namespace Gravitation.Comms.EventArgs
{
    public class ReceiverHolderEventArgs : System.EventArgs
    {
        public IReceiver rec;

        public ReceiverHolderEventArgs(IReceiver rec)
        {
            this.rec = rec;
        }
    }
}
