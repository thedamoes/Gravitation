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
        World mWorld;

        public MapLoader(String fileName, World world)
        {
            this.mWorld = world;
            Stream mapXml= new FileStream(fileName,FileMode.Open);
            XmlSerializer serialiser = new XmlSerializer(typeof(Map));
            currentmap = (Map)serialiser.Deserialize(mapXml);

            Vector2 wallPos = new Vector2(
                                            currentmap.Surfaces.MapWalls.First().Asset.Position.X,
                                            currentmap.Surfaces.MapWalls.First().Asset.Position.Y
                                        );

            wall1 = new SpriteObjects.Wall(world, wallPos);
        }
#if DEBUG
        public void unloadBodies()
        {
            mWorld.RemoveBody(wall1.mSpriteBody);
        }
#endif
        public void loadMap(ContentManager cm)
        {
            wall1.LoadContent(cm, currentmap.Surfaces.MapWalls.First().Asset.name);
        }

        public void drawMap(SpriteBatch sb)
        {
            wall1.Draw(sb);
        }
    }
}
