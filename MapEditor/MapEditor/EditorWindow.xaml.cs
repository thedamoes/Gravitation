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
        List<Viewport2DVisual3D> objectList = new List<Viewport2DVisual3D>();
        Viewport2DVisual3D activeObject = null;

        public EditorWindow()
        {
            InitializeComponent();

            addObject(@"D:\programming\Gravitation\Gravitation\GravitationContent\ship.png", new Point(-1,0));

            addObject(@"D:\programming\Gravitation\Gravitation\GravitationContent\floor.png", new Point(1, 0));

        }

        private void addObject(string uri, Point position)
        {
            Viewport2DVisual3D newImg = new Viewport2DVisual3D();
            MeshGeometry3D mesh = new MeshGeometry3D();


            Point3DCollection positionCollection = new Point3DCollection();
            positionCollection.Add(new Point3D(-1, 1, 0));
            positionCollection.Add(new Point3D(-1, -1, 0));
            positionCollection.Add(new Point3D(1, -1, 0));
            positionCollection.Add(new Point3D(1, 1, 0));

            PointCollection textureCollection = new PointCollection();
            textureCollection.Add(new Point(0, 0));
            textureCollection.Add(new Point(0, 1));
            textureCollection.Add(new Point(1, 1));
            textureCollection.Add(new Point(1, 0));

            Int32Collection triangleIdecesCollection = new Int32Collection();
            triangleIdecesCollection.Add(0);
            triangleIdecesCollection.Add(1);
            triangleIdecesCollection.Add(2);
            triangleIdecesCollection.Add(0);
            triangleIdecesCollection.Add(2);
            triangleIdecesCollection.Add(3);


            mesh.Positions = positionCollection;
            mesh.TextureCoordinates = textureCollection;
            mesh.TriangleIndices = triangleIdecesCollection;

            newImg.Geometry = mesh;

            DiffuseMaterial material = new DiffuseMaterial(Brushes.White);
            Viewport2DVisual3D.SetIsVisualHostMaterial(material, true);
            newImg.Material = material;


            TranslateTransform3D transform = new TranslateTransform3D(position.X, position.Y, 0);


            newImg.Transform = transform;

            Image pic = new Image();

            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(uri, UriKind.Absolute);
            bi3.EndInit();

            pic.Source = bi3;
            newImg.Visual = pic;


            objectList.Add(newImg);
            this.viewPort.Children.Add(newImg);

        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Retrieve the coordinate of the mouse position.
            Point pt = e.GetPosition((UIElement)sender);

            // Perform the hit test against a given portion of the visual object tree.
            HitTestResult result = VisualTreeHelper.HitTest(this.bla, pt);

            if (result != null)
            {
                foreach (Viewport2DVisual3D worldItem in objectList)
                {
                    if (worldItem.Visual == result.VisualHit)
                    {
                        activeObject = worldItem;
                        break;
                    }
                }
            }
        }

        private void bla_MouseMove(object sender, MouseEventArgs e)
        {
            if (activeObject == null)
                return;

            Point test = e.GetPosition(this.viewPort);

            activeObject.Transform = new TranslateTransform3D(0.02, 0.04,0);
        }
    }
}
