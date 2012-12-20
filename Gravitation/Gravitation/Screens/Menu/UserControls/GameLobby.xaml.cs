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
    /// Interaction logic for GameLobby.xaml
    /// </summary>
    public partial class GameLobby : UserControl
    {
        public event EventHandler<EventArgs> OnsendMessage;
        public String message
        {
            get { return this.textBox1.Text; }
        }

        public GameLobby()
        {
            InitializeComponent();
            this.sendBttn.Click += new RoutedEventHandler(sendBttn_Click);
        }

        void sendBttn_Click(object sender, RoutedEventArgs e)
        {
            this.fire<EventArgs>(this.OnsendMessage, e);
            this.textBox1.Text = "";
        }

        public void addChatMessage(string message)
        {
            this.textBlock1.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
          new Action(
            delegate()
            {
                this.textBlock1.Text += message + "\n\r";
            }));
            
        }

        public void addPlayer(string name)
        {

            this.listBox1.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                new Action(
                delegate()
                {
                    this.listBox1.Items.Add(name);
                }));

            
        }

        public void handleKeyboard(Microsoft.Xna.Framework.Input.KeyboardState curState)
        {
            this.textBox1.handleKeyboard(curState);
        }

        private void fire<A>(EventHandler<A> evnt, A args) where A : EventArgs
        {
            if (evnt != null)
                evnt(this, args);
        }
    }
}
