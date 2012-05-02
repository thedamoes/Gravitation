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
using System.Windows.Media.Media3D;

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : UserControl
    {
        List<Image> objectList = new List<Image>();
        Image activeObject = null;
        OptionsPannel pannel;

        public double canvasZoom
        {
            get { return (double)GetValue(EditorWindow.canvasZoomProperty); }
            set { SetValue(EditorWindow.canvasZoomProperty, value); }
        }
        public static readonly DependencyProperty canvasZoomProperty = DependencyProperty.Register("canvasZoom", typeof(double), typeof(EditorWindow), new PropertyMetadata(0.0));

        Point lastMouseClick;

        public EditorWindow()
        {
            InitializeComponent();
            this.canvasZoom = 0.5;    
        }

        public void setOptionsPannel(OptionsPannel op)
        {
            this.pannel = op;
        }

        public void addObject(string uri, Point position)
        {
           
            Image pic = new Image();

            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(uri, UriKind.Absolute);
            bi3.EndInit();

            pic.Source = bi3;


            objectList.Add(pic);
            this.mainCanvas.Children.Add(pic);
            Canvas.SetLeft(pic, position.X);
            Canvas.SetBottom(pic, position.Y);


        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Retrieve the coordinate of the mouse position.
            lastMouseClick = e.GetPosition((UIElement)sender);

            // Perform the hit test against a given portion of the visual object tree.
            HitTestResult result = VisualTreeHelper.HitTest(this.bla, lastMouseClick);

            if (result != null)
            {
                if (activeObject == result.VisualHit)
                    activeObject = null;
                else
                    activeObject = (Image)result.VisualHit;
            }
        }

        private void bla_MouseMove(object sender, MouseEventArgs e)
        {
            if (activeObject == null)
                return;

            Point test = e.GetPosition(this.mainCanvas);

            Canvas.SetLeft(activeObject, test.X - (activeObject.ActualWidth/2));
            Canvas.SetTop(activeObject, test.Y - (activeObject.ActualHeight / 2));
        }

        private void bla_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                this.canvasZoom += 0.01;
            else
                this.canvasZoom -= 0.01;
        }

        
    }
}
