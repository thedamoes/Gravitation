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

#if DEBUG
        public String MapFile;
#endif
        private Map mCurrentmap;
        
        private SpriteObjects.Wall mLeftWall;
        private SpriteObjects.Wall mRightWall;
        private SpriteObjects.Wall mBottomWall;
        private SpriteObjects.Wall mTopWall;
        private SpriteObjects.Sprite mBackground;

        private SpriteObjects.Obstical[] mObsicals;

        private World mWorld;

        public Vector2 MapDimentions
        {
            get
            {
                return new Vector2 (mCurrentmap.MapDimentions.Height,
                mCurrentmap.MapDimentions.Width);
            }
        }
        public float leftWallPosX
        {
            get
            {
                return (this.mLeftWall.Position.X * Screens.BaseGame.MeterInPixels) - (this.mLeftWall.spriteOrigin.X * this.mLeftWall.Scale);
            }
        }
        public float rightWallPosX
        {
            get
            {
                return (this.mRightWall.Position.X * Screens.BaseGame.MeterInPixels) + (this.mRightWall.spriteOrigin.X);
            }
        }
        public float bottonWallPosY
        {
            get
            {
                return (this.mBottomWall.Position.Y * Screens.BaseGame.MeterInPixels) + (this.mBottomWall.spriteOrigin.Y);
            }
        }
        public float topWallPosY
        {
            get
            {
                return (this.mTopWall.Position.Y * Screens.BaseGame.MeterInPixels) - (this.mTopWall.spriteOrigin.Y * this.mTopWall.Scale);
            }
        }

        public float rightWallWidth
        {
            get
            {
                return Convert.ToInt16(mCurrentmap.Surfaces.MapWalls[1].Asset.Width) * Convert.ToInt16(mCurrentmap.Surfaces.MapWalls[1].Asset.Scale.X);
            }
        }

        public Vector2 shipStartPosP1
        {
            get
            {
                return new Vector2(Convert.ToInt32(mCurrentmap.SpawnPoint.Player1.X),
                                   Convert.ToInt32(mCurrentmap.SpawnPoint.Player1.Y));
            }
        }

        public Vector2 shipStartPosP2
        {
            get
            {
                return new Vector2(Convert.ToInt32(mCurrentmap.SpawnPoint.Player2.X),
                                   Convert.ToInt32(mCurrentmap.SpawnPoint.Player2.Y));
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
            createWall(ref mLeftWall, mCurrentmap.Surfaces.MapWalls[0]);
            createWall(ref mRightWall, mCurrentmap.Surfaces.MapWalls[1]);
            createWall(ref mBottomWall, mCurrentmap.Surfaces.MapWalls[2]);
            createWall(ref mTopWall, mCurrentmap.Surfaces.MapWalls[3]);

            // create obsticals
            createObsicals(ref mObsicals, mCurrentmap.Surfaces.Obsticals);

#if DEBUG
            this.MapFile = fileName;
#endif
        }
#if DEBUG
        public void unloadBodies()
        {
            mWorld.RemoveBody(mLeftWall.mSpriteBody);
            mWorld.RemoveBody(mRightWall.mSpriteBody);
            mWorld.RemoveBody(mBottomWall.mSpriteBody);
            mWorld.RemoveBody(mTopWall.mSpriteBody);
        }
#endif
        public void loadMap(ContentManager cm)
        {
            // load map
            MapSurfacesBackgoundPicture backgrnd = mCurrentmap.Surfaces.BackgoundPicture;
            mBackground.LoadContent(cm, backgrnd.Asset.name);

            // load walls
            MapSurfacesWall[] wallSpecs = mCurrentmap.Surfaces.MapWalls;
            mLeftWall.LoadContent(cm, wallSpecs[0].Asset.name);
            mRightWall.LoadContent(cm, wallSpecs[1].Asset.name);
            mBottomWall.LoadContent(cm, wallSpecs[2].Asset.name);
            mTopWall.LoadContent(cm, wallSpecs[3].Asset.name);

            // load obsitcals
            for (int i = 0; i < mObsicals.Length; i++)
                mObsicals[i].LoadContent(cm, mCurrentmap.Surfaces.Obsticals[i].name);

        }

        public void drawMap(SpriteBatch sb)
        {
           // mBackground.Draw(sb); // draw background
            
            mLeftWall.Draw(sb); // draw walls
            mRightWall.Draw(sb);
            mBottomWall.Draw(sb);
            mTopWall.Draw(sb);

            for (int i = 0; i < mObsicals.Length; i++)
                mObsicals[i].Draw(sb);
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
            Vector2 backPos = new Vector2(
                                            Convert.ToInt32(backSpec.Asset.Position.X),
                                            Convert.ToInt32(backSpec.Asset.Position.Y)
                                        );

            float backScaleX = (float)Convert.ToDecimal(backSpec.Asset.Scale.X);
            float backScaleY = (float)Convert.ToDecimal(backSpec.Asset.Scale.Y);

            float spriteRotation = (float)Convert.ToDecimal(backSpec.Asset.Rotation);

            back = new SpriteObjects.Sprite(backPos, spriteRotation);
            back.WidthScale = backScaleX;
            back.HeightScale = backScaleY;
        }

        private void createObsicals(ref SpriteObjects.Obstical[] obsicals, MapSurfacesAsset[] obsicalsSpec)
        {
            obsicals = new SpriteObjects.Obstical[obsicalsSpec.Length];

            for (int i = 0; i < obsicals.Length; i++)
            {
                Vector2 obsticalPos = new Vector2(
                                               Convert.ToInt32(obsicalsSpec[i].Position.X),
                                               Convert.ToInt32(obsicalsSpec[i].Position.Y)
                                           );

                Vector2 obsticalPosScale = new Vector2(
                                            (float)Convert.ToDecimal(obsicalsSpec[i].Scale.X),
                                            (float)Convert.ToDecimal(obsicalsSpec[i].Scale.Y)
                    );

                float rotation = (float)Convert.ToDecimal(obsicalsSpec[i].Rotation);

                obsicals[i] = new SpriteObjects.Obstical(mWorld, obsticalPos, obsticalPosScale, rotation);
            }

        }


    }
}
