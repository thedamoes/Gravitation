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
        private Map mCurrentmap;
        private SpriteObjects.Wall mWall1;
        private SpriteObjects.Wall mWall2;
        private SpriteObjects.Wall mWall3;
        private SpriteObjects.Wall mWall4;
        private SpriteObjects.Sprite mBackground;


        private World mWorld;

        public Vector2 MapDimentions
        {
            get
            {
                return new Vector2 (mCurrentmap.MapDimentions.Height,
                mCurrentmap.MapDimentions.Width);
            }
        }
        public short leftWallPosX
        {
            get
            {
                return mCurrentmap.Surfaces.MapWalls[0].Asset.Position.X;
            }
        }
        public short rightWallPosX
        {
            get
            {
                return mCurrentmap.Surfaces.MapWalls[1].Asset.Position.X;
            }
        }
        public short bottonWallPosY
        {
            get
            {
                return mCurrentmap.Surfaces.MapWalls[2].Asset.Position.Y;
            }
        }
        public short topWallPosY
        {
            get
            {
                return mCurrentmap.Surfaces.MapWalls[3].Asset.Position.Y;
            }
        }

        public MapLoader(String fileName, World world)
        {
            this.mWorld = world;
            Stream mapXml= new FileStream(fileName,FileMode.Open);
            XmlSerializer serialiser = new XmlSerializer(typeof(Map));
            mCurrentmap = (Map)serialiser.Deserialize(mapXml);

            //loadBackground
            createBackground(ref mBackground, mCurrentmap.Surfaces.BackgoundPicture);

            //loadWalls
            createWall(ref mWall1, mCurrentmap.Surfaces.MapWalls[0]);
            createWall(ref mWall2, mCurrentmap.Surfaces.MapWalls[1]);
            createWall(ref mWall3, mCurrentmap.Surfaces.MapWalls[2]);
            createWall(ref mWall4, mCurrentmap.Surfaces.MapWalls[3]);
        }
#if DEBUG
        public void unloadBodies()
        {
            mWorld.RemoveBody(mWall1.mSpriteBody);
            mWorld.RemoveBody(mWall2.mSpriteBody);
            mWorld.RemoveBody(mWall3.mSpriteBody);
            mWorld.RemoveBody(mWall4.mSpriteBody);
        }
#endif
        public void loadMap(ContentManager cm)
        {
            MapSurfacesBackgoundPicture backgrnd = mCurrentmap.Surfaces.BackgoundPicture;
            mBackground.LoadContent(cm, backgrnd.AssetName);

            MapSurfacesWall[] wallSpecs = mCurrentmap.Surfaces.MapWalls;
            mWall1.LoadContent(cm, wallSpecs[0].Asset.name);
            mWall2.LoadContent(cm, wallSpecs[1].Asset.name);
            mWall3.LoadContent(cm, wallSpecs[2].Asset.name);
            mWall4.LoadContent(cm, wallSpecs[3].Asset.name);
        }

        public void drawMap(SpriteBatch sb)
        {
            mBackground.Draw(sb);
            mWall1.Draw(sb);
            mWall2.Draw(sb);
            mWall3.Draw(sb);
            mWall4.Draw(sb);
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

            float spriteRotation = (float)Convert.ToDecimal(wallSpec.Asset.Rotation);

            wall = new SpriteObjects.Wall(mWorld, wallPos, spriteRotation);
            wall.WidthScale = scale.X;
            wall.HeightScale = scale.Y;

        }

        private void createBackground(ref SpriteObjects.Sprite back, MapSurfacesBackgoundPicture backSpec)
        {
            Vector2 backPos = new Vector2(0, -800);
            float spriteRotation = 0;

          /*  Vector2 backPos = new Vector2(
                                            Convert.ToInt32(backSpec.Asset.Position.X),
                                            Convert.ToInt32(backSpec.Asset.Position.Y)
                                        );*/

            float backScale = (float)Convert.ToDecimal(backSpec.Scale);

          //  float spriteRotation = (float)Convert.ToDecimal(backSpec.Asset.Rotation);

            back = new SpriteObjects.Sprite(backPos, spriteRotation);
            back.WidthScale = backScale;
            back.HeightScale = backScale;



        }


    }
}
