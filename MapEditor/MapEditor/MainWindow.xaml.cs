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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Editor.setOptionsPannel(this.pannel);
            this.pannel.setEditorWindow(this.Editor);

            XNAWindow.XNAHost xnaWindow = new XNAWindow.XNAHost();
            xnaWindow.Show();
            
            /*String rightWall = @"D:\programming\Gravitation\Gravitation\GravitationContent\Maps\map1\rightWall.png";
            String leftWall = @"D:\programming\Gravitation\Gravitation\GravitationContent\Maps\map1\leftWall.png";
            String bottomWall = @"D:\programming\Gravitation\Gravitation\GravitationContent\Maps\map1\bottomWall.png";
            String topWall = @"D:\programming\Gravitation\Gravitation\GravitationContent\Maps\map1\topWall.png";

            this.Editor.addLeftwall(leftWall, new System.Windows.Point(50, 54));
            this.Editor.addRightwall(rightWall, new System.Windows.Point(1700, 284));
            this.Editor.addBottomwall(bottomWall, new System.Windows.Point(53, 1144));
            this.Editor.addTopwall(topWall, new System.Windows.Point(442, 50));*/
        }
    }
}
