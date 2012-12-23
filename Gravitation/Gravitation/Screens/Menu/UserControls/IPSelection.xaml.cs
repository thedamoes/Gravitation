using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gravitation.Screens.Menu.UserControls
{
    /// <summary>
    /// Interaction logic for IPSelection.xaml
    /// </summary>
    public partial class IPSelection : UserControl
    {
        public event EventHandler<EventArgs> connect_Click, host_Click;

        public String SelectedIP
        {
            get { return this.textBox1.Text; }
        }

        public IPSelection()
        {
            InitializeComponent();
            this.label1.Content = "Enter an IP: ";

            this.button1.Click += this.connClick;
            this.buttonHost.Click += this.hostClick;
            this.textBox1.DisableCharacters = true;
        }

        public void handleKeyboard(Microsoft.Xna.Framework.Input.KeyboardState curState)
        {
            this.textBox1.handleKeyboard(curState);
        }

        #region event Relays

        private void connClick(object sender, EventArgs even)
        {
            this.fire<EventArgs>(this.connect_Click, even);
        }

        private void hostClick(object sender, EventArgs e)
        {
            this.fire<EventArgs>(this.host_Click, e);
        }

        #endregion


        private void fire<A>(EventHandler<A> evnt, A args) where A : EventArgs
        {
            if(evnt != null)
                evnt(this,args);
        }

    }
}
