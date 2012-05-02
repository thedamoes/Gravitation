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

        public EditorWindow()
        {
            InitializeComponent();

            Viewport2DVisual3D newImg = new Viewport2DVisual3D();
            MeshGeometry3D mesh = new MeshGeometry3D();
             
            
            Point3DCollection positionCollection = new Point3DCollection();
            positionCollection.Add(new Point3D(-1,1,0));
            positionCollection.Add(new Point3D(-1,-1,0));
            positionCollection.Add(new Point3D(1,-1,0));
            positionCollection.Add(new Point3D(1,1,0));

            PointCollection textureCollection = new PointCollection();
            textureCollection.Add(new Point(0,0));
            textureCollection.Add(new Point(0,1));
            textureCollection.Add(new Point(1,1));
            textureCollection.Add(new Point(1,0));

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

            Image pic = new Image();

            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(@"D:\programming\Gravitation\Gravitation\GravitationContent\floor.png", UriKind.Absolute);
            bi3.EndInit();

            pic.Source = bi3;
            newImg.Visual = pic;

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
                MessageBox.Show("HIT");
            }
        }
    }
}
