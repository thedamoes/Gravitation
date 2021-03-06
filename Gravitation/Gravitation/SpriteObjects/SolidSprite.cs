﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Factories;

namespace Gravitation.SpriteObjects
{
    class SolidSprite : Sprite
    {
        protected World mWorld;
        protected Vector2 mPosition;
        protected float mrotation;


        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            base.mSpriteBodyTexture = theContentManager.Load<Texture2D>(theAssetName);
            base.mSpriteSheetTexture = theContentManager.Load<Texture2D>(theAssetName);
            base.AssetName = theAssetName;
            base.Source = new Rectangle(0, 0, base.mSpriteBodyTexture.Width, base.mSpriteBodyTexture.Height);
            base.Size = new Rectangle(0, 0, (int)(base.mSpriteBodyTexture.Width * base.WidthScale), (int)(base.mSpriteBodyTexture.Height * base.HeightScale));


            uint[] data = new uint[base.mSpriteBodyTexture.Width * base.mSpriteBodyTexture.Height];

            //Transfer the texture data to the array
            base.mSpriteBodyTexture.GetData(data);

            //Find the vertices that makes up the outline of the shape in the texture
            Vertices textureVertices = PolygonTools.CreatePolygon(data, base.mSpriteBodyTexture.Width, false);

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



            base.mSpriteBody = BodyFactory.CreateCompoundPolygon(mWorld, list, 1f, (mPosition / MeterInPixels), BodyType.Static);
            base.mSpriteBody.Restitution = 0.3f;
            base.mSpriteBody.Friction = 0.4f;
            base.mSpriteBody.IsStatic = true;
            base.mSpriteBody.Rotation = mrotation;

            base.mSpriteBody.CollisionCategories = Category.Cat1;
            base.mSpriteBody.CollidesWith = Category.All;

            foreach (Fixture fixturec in base.mSpriteBody.FixtureList)
            {
                fixturec.UserData = "wall";

            }


        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            //Create a single body with multiple fixtures

            Vector2 spritePos = base.mSpriteBody.Position * MeterInPixels;

            theSpriteBatch.Draw(base.mSpriteSheetTexture, spritePos, base.Source,
                Color.White, base.mSpriteBody.Rotation, base.spriteOrigin,
                new Vector2(base.WidthScale, base.HeightScale), SpriteEffects.None, 0f);

        }
    }
}
