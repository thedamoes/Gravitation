﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.DebugViews;


namespace Gravitation.SpriteObjects
{
    class Ship : Sprite
    {

        public World world;

       // public List<SpriteObjects.Shot> mShots = new List<SpriteObjects.Shot>();
        public SpriteObjects.Shot mShot;
        ContentManager theContentManager;

        private Vector2 mPosition;

        public Vector2 ShipPosition
        {
            set { mPosition = value; }
            get { return base.Position; }
        }

        public World World
        {
            set { world = value; }
        }


        public Ship(World world, Vector2 position)
        {
            this.world = world;
            this.mPosition = position;
        }

        public Ship(World world)
        {
            this.world = world;
            this.mPosition = new Vector2(100f,100f);
        }

        public Ship()
        {
            this.world = null;
            this.mPosition = new Vector2(100f, 100f);
        }


        public override void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            this.theContentManager = theContentManager;
            base.mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            base.AssetName = theAssetName;
            base.Source = new Rectangle(0, 0, base.mSpriteTexture.Width, base.mSpriteTexture.Height);
            base.Size = new Rectangle(0, 0, (int)(base.mSpriteTexture.Width * base.WidthScale), (int)(base.mSpriteTexture.Height * base.HeightScale));



            uint[] data = new uint[base.mSpriteTexture.Width * base.mSpriteTexture.Height];

            //Transfer the texture data to the array
            base.mSpriteTexture.GetData(data);

            //Find the vertices that makes up the outline of the shape in the texture
            Vertices textureVertices = PolygonTools.CreatePolygon(data, base.mSpriteTexture.Width, false);

            //The tool return vertices as they were found in the texture.
            //We need to find the real center (centroid) of the vertices for 2 reasons:

            //1. To translate the vertices so the polygon is centered around the centroid.
            Vector2 centroid = -textureVertices.GetCentroid();
            textureVertices.Translate(ref centroid);

            //2. To draw the texture the correct place.
            base.spriteOrigin = -centroid;

            //We simplify the vertices found in the texture.
            textureVertices = SimplifyTools.ReduceByDistance(textureVertices, 4f); //WOOOOOT

            //Since it is a concave polygon, we need to partition it into several smaller convex polygons
            list = BayazitDecomposer.ConvexPartition(textureVertices);

            //Scale the vertices so that they're not HUUUGE.
            Vector2 vertScale = new Vector2((1 / MeterInPixels) * WidthScale, (1 / MeterInPixels) * HeightScale) * 1f;
            foreach (Vertices verti in list)
            {
                verti.Scale(ref vertScale);
            }



            base.mSpriteBody = BodyFactory.CreateCompoundPolygon(world, list, 1f, (mPosition / MeterInPixels), BodyType.Dynamic);
            base.mSpriteBody.Restitution = 0.3f;
            base.mSpriteBody.Friction = 1f;
            base.mSpriteBody.IsStatic = false;


           // mShot.LoadContent(theContentManager);

            
        }




        public void fire()
        {
            bool aCreateNew = true;

            if (aCreateNew == true)
            {

                mShot = new SpriteObjects.Shot(world, base.mSpriteBody.Position, base.mSpriteBody.Rotation);

                mShot.LoadContent(theContentManager);

                mShot.fire(base.mSpriteBody.Position, base.mSpriteBody.Rotation);
            }


        }

        public void updateShot()
        {

                mShot.Update();
        }






        public override void Draw(SpriteBatch theSpriteBatch)
        {
            //Create a single body with multiple fixtures

            Vector2 spritePos = base.mSpriteBody.Position * MeterInPixels;

            theSpriteBatch.Draw(base.mSpriteTexture, spritePos, base.Source,
                Color.White, base.mSpriteBody.Rotation, base.spriteOrigin,
                new Vector2(base.WidthScale, base.HeightScale), SpriteEffects.None, 0f);

            if (mShot != null)
                mShot.Draw(theSpriteBatch);

        }




    }
}
