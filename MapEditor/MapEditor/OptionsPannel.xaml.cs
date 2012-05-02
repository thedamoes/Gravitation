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

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for OptionsPannel.xaml
    /// </summary>
    public partial class OptionsPannel : UserControl
    {

        List<string> wallTypes = new List<string>();

        


        public OptionsPannel()
        {
            InitializeComponent();
            initaliseGUI();
        }

        private void initaliseGUI()
        {
            wallTypes.Add("Left");
            wallTypes.Add("Right");
            wallTypes.Add("Bottom");
            wallTypes.Add("Top");

            this.Walls_type_select.ItemsSource = wallTypes;
        }

        private void wall_texture_bttn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                this.Walls_selected_texture.Text = filename;
            }
        }
    }
}
