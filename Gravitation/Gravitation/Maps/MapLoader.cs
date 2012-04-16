using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gravitation.Maps
{
    class MapLoader
    {
        Map currentmap;
        SpriteObjects.Wall wall1;
        SpriteObjects.Wall wall2;
        SpriteObjects.Wall wall3;
        SpriteObjects.Wall wall4;

        World mWorld;

        public MapLoader(String fileName, World world)
        {
            this.mWorld = world;
            Stream mapXml= new FileStream(fileName,FileMode.Open);
            XmlSerializer serialiser = new XmlSerializer(typeof(Map));
            currentmap = (Map)serialiser.Deserialize(mapXml);

            //loadWalls
            createWall(ref wall1, currentmap.Surfaces.MapWalls[0]);
            createWall(ref wall2, currentmap.Surfaces.MapWalls[1]);
            createWall(ref wall3, currentmap.Surfaces.MapWalls[2]);
            createWall(ref wall4, currentmap.Surfaces.MapWalls[3]);
        }
#if DEBUG
        public void unloadBodies()
        {
            mWorld.RemoveBody(wall1.mSpriteBody);
            mWorld.RemoveBody(wall2.mSpriteBody);
            mWorld.RemoveBody(wall3.mSpriteBody);
            mWorld.RemoveBody(wall4.mSpriteBody);
        }
#endif
        public void loadMap(ContentManager cm)
        {
            MapSurfacesWall[] wallSpecs = currentmap.Surfaces.MapWalls;
            wall1.LoadContent(cm, wallSpecs[0].Asset.name);
            wall2.LoadContent(cm, wallSpecs[1].Asset.name);
            wall3.LoadContent(cm, wallSpecs[2].Asset.name);
            wall4.LoadContent(cm, wallSpecs[3].Asset.name);
        }

        public void drawMap(SpriteBatch sb)
        {
            wall1.Draw(sb);
            wall2.Draw(sb);
            wall3.Draw(sb);
            wall4.Draw(sb);
        }

        private void createWall(ref SpriteObjects.Wall wall, MapSurfacesWall wallSpec)
        {
            Vector2 wallPos = new Vector2(
                                            Convert.ToInt32(wallSpec.Asset.Position.X),
                                            Convert.ToInt32(wallSpec.Asset.Position.Y)
                                        );

            Vector2 scale = new Vector2(
                                        (float)Convert.ToDecimal(wallSpec.Asset.Scale.X),
                                        (float)Convert.ToDecimal(wallSpec.Asset.Scale.Y)
                );

            wall = new SpriteObjects.Wall(mWorld, wallPos);
            wall.WidthScale = scale.X;
            wall.HeightScale = scale.Y;

        }
    }
}
