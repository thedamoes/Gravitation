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
using System.Xml.Serialization;
using System.IO;

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for OptionsPannel.xaml
    /// </summary>
    public partial class OptionsPannel : UserControl
    {

        List<string> wallTypes = new List<string>();
        enum WALL_TYPES { LEFT = 0, RIGHT = 1, BOTTOM = 2, TOP = 3};
        EditorWindow editor;
        


        public OptionsPannel()
        {
            InitializeComponent();
            initaliseGUI();
        }

        public void setEditorWindow(EditorWindow ew)
        {
            this.editor = ew;
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

        private void Wall_add_btn_Click(object sender, RoutedEventArgs e)
        {
            switch (this.Walls_type_select.SelectedIndex)
            {
                case (int)WALL_TYPES.LEFT:
                    editor.addLeftwall(this.Walls_selected_texture.Text, new Point(0, 0));
                    break;

                case (int)WALL_TYPES.RIGHT:
                    editor.addRightwall(this.Walls_selected_texture.Text, new Point(0, 0));
                    break;

                case (int)WALL_TYPES.BOTTOM:
                    editor.addBottomwall(this.Walls_selected_texture.Text, new Point(0, 0));
                    break;

                case (int)WALL_TYPES.TOP:
                    editor.addTopwall(this.Walls_selected_texture.Text, new Point(0, 0));
                    break;
                default:
                       MessageBox.Show("u gotta select a wall type FOOL");
                       break;
                    
            }
        }

        private void saveBttn_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "xml documents (.xml)|*.xml";
            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;
                Map newMap = new Map();

                // create map dimentions
                MapMapDimentions dimentions = new MapMapDimentions();
                dimentions.Height = 1;
                dimentions.Width = 1;
                newMap.MapDimentions = dimentions;


                // create surfaces
                MapSurfaces surfaces = new MapSurfaces();

                // create background
                MapSurfacesBackgoundPicture backgroundPic = new MapSurfacesBackgoundPicture();
                MapSurfacesBackgoundPictureAsset backgroundAsset = new MapSurfacesBackgoundPictureAsset();
                MapSurfacesBackgoundPictureAssetScale backScale = new MapSurfacesBackgoundPictureAssetScale();
                MapSurfacesBackgoundPictureAssetPosition backgroundPos = new MapSurfacesBackgoundPictureAssetPosition();

                backgroundAsset.name = "smudgystars";
                
                backScale.X = "1";
                backScale.Y = "1";
             
                backgroundPos.X = "0";
                backgroundPos.Y = "0";

                backgroundPic.Asset = backgroundAsset;
                surfaces.BackgoundPicture = backgroundPic;
                backgroundAsset.Scale = backScale;
                backgroundAsset.Position = backgroundPos;

                // create some walls
                MapSurfacesWall[] walls = new MapSurfacesWall[4];
                createWall(ref walls, 0, "left", editor.leftWallPosition, editor.leftWall);
                createWall(ref walls, 1, "right", editor.rightWallPosition, editor.rightWall);
                createWall(ref walls, 2, "bottom", editor.bottomWallPosition, editor.bottomWall);
                createWall(ref walls, 3, "top", editor.topWallPosition, editor.topWall);

                MapSurfacesAsset[] obsticals = new MapSurfacesAsset[0];

                surfaces.Obsticals = obsticals;

                surfaces.MapWalls = walls;
                newMap.Surfaces = surfaces;

                // create spawns

                MapSpawnPoint spawns = new MapSpawnPoint();
                MapSpawnPointPlayer1 p1Spawn = new MapSpawnPointPlayer1();

                p1Spawn.X = "0";
                p1Spawn.Y = "100";

                spawns.Player1 = p1Spawn;
                newMap.SpawnPoint = spawns;


                XmlSerializer serialiser = new XmlSerializer(typeof(Map));
                serialiser.Serialize(new StreamWriter(filename), newMap);
            }

        }

        private void createWall(ref MapSurfacesWall[] wallArr,
                                int index,
                                String wallType,
                                Point Wallposition,
                                EditorWindow.Asset editorWall)
        {
            MapSurfacesWall wall = new MapSurfacesWall();
            wall.walltype = wallType;
            MapSurfacesWallAsset wallAsset = new MapSurfacesWallAsset();

            MapSurfacesWallAssetScale scale = new MapSurfacesWallAssetScale();
            scale.X = 1M;
            scale.Y = 1M;// no idea wat M is ?? fix it later

            wallAsset.Scale = scale;
            wallAsset.name = "Maps/Map" + editorWall.mapNo + "/" + editorWall.fileName;

            wallAsset.Height = "1";
            wallAsset.Width = "1";

            MapSurfacesWallAssetPosition position = new MapSurfacesWallAssetPosition();
            position.X = ((int)Wallposition.X).ToString();
            position.Y = ((int)Wallposition.Y).ToString();

            wallAsset.Position = position;
            wall.Asset = wallAsset;

            wallArr[index] = wall;

        }
    }
}
