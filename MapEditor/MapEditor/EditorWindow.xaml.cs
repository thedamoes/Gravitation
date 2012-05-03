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

        //walls
        public Asset leftWall = new Asset(null, null, 0); // yes.. public = bad w/e ... cba
        public Asset rightWall = new Asset(null, null, 0);
        public Asset topWall = new Asset(null, null, 0);
        public Asset bottomWall = new Asset(null, null, 0);

        public Point leftWallPosition
        {
            get
            {
                Point pos = new Point();

                pos.Y =  Canvas.GetBottom(leftWall.image);
                pos.X = Canvas.GetLeft(leftWall.image);

                return pos;
            }
        }
        public Point rightWallPosition
        {
            get
            {
                Point pos = new Point();

                pos.Y = Canvas.GetBottom(rightWall.image);
                pos.X = Canvas.GetLeft(rightWall.image);

                return pos;
            }
        }
        public Point bottomWallPosition
        {
            get
            {
                Point pos = new Point();

                pos.Y = Canvas.GetBottom(bottomWall.image);
                pos.X = Canvas.GetLeft(bottomWall.image);

                return pos;
            }
        }
        public Point topWallPosition
        {
            get
            {
                Point pos = new Point();

                pos.Y = Canvas.GetBottom(topWall.image);
                pos.X = Canvas.GetLeft(topWall.image);

                return pos;
            }
        }


        public class Asset
        {
            public Image image = null;
            public String fileName;
            public int mapNo;

            public Asset(Image image,
                         String fileName,
                         int mapNo)
            {
                this.image = image;
                this.fileName = fileName;
                this.mapNo = mapNo;
            }
        }

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

        public Image addObject(string uri, Point position)
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

            return pic;

        }

        public void addRightwall(string url, Point position)
        {
            if (this.rightWall.image != null)
                this.mainCanvas.Children.Remove(this.rightWall.image);

            this.rightWall.image = addObject(url, position);
            this.rightWall.fileName = System.IO.Path.GetFileNameWithoutExtension(url);
            this.rightWall.mapNo = extractMapNo(url);
        }

        public void addLeftwall(string url, Point position)
        {
            if (this.leftWall.image != null)
                this.mainCanvas.Children.Remove(this.leftWall.image);

            this.leftWall.image = addObject(url, position);
            this.leftWall.fileName = System.IO.Path.GetFileNameWithoutExtension(url);
            this.leftWall.mapNo = extractMapNo(url);
        }

        public void addTopwall(string url, Point position)
        {
            if (this.topWall.image != null)
                this.mainCanvas.Children.Remove(this.topWall.image);

            this.topWall.image = addObject(url, position);
            this.topWall.fileName = System.IO.Path.GetFileNameWithoutExtension(url);
            this.topWall.mapNo = extractMapNo(url);
        }

        public void addBottomwall(string url, Point position)
        {
            if (this.bottomWall.image != null)
                this.mainCanvas.Children.Remove(this.bottomWall.image);

            this.bottomWall.image = addObject(url, position);
            this.bottomWall.fileName = System.IO.Path.GetFileNameWithoutExtension(url);
            this.bottomWall.mapNo = extractMapNo(url);
        }

        private int extractMapNo(String url)
        {
            String mapNum = "";
            for (int i = url.LastIndexOf('\\')-1; i > 0; i--)
            {
                if (Char.IsNumber(url[i]))
                    mapNum += url[i];
                else
                    break;
            }

            return Int32.Parse(mapNum);
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
