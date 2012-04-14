using System;
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


namespace Gravitation.SpriteObjects
{
    class Ship : Sprite
    {

        private World world;
        private Vector2 position;

        public Ship(World world, Vector2 position)
        {
           // base.AssetName = "floor";
          /*  base.mSpriteBody = BodyFactory.CreateRectangle(world, 96f / Sprite.MeterInPixels, 64f / Sprite.MeterInPixels, 1f, position);
            base.mSpriteBody.BodyType = BodyType.Dynamic;
            base.mSpriteBody.Restitution = 0.3f;
            base.mSpriteBody.Friction = 0.1f;
            base.mSpriteBody.IsStatic = false;*/

            this.world = world;
            this.position = position;
        }


        public override void LoadContent(ContentManager theContentManager, string theAssetName)
        {
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
            textureVertices = SimplifyTools.ReduceByDistance(textureVertices, 4f);

            //Since it is a concave polygon, we need to partition it into several smaller convex polygons
            list = BayazitDecomposer.ConvexPartition(textureVertices);


            base.mSpriteBody = BodyFactory.CreateCompoundPolygon(world, list, 1f, BodyType.Dynamic);
           // base.mSpriteBody.BodyType = BodyType.Dynamic;
            base.mSpriteBody.Restitution = 0.3f;
            base.mSpriteBody.Friction = 0.1f;
            base.mSpriteBody.IsStatic = false;
        }






        public override void Draw(SpriteBatch theSpriteBatch)
        {




            //Create a single body with multiple fixtures

            Vector2 spritePos = /*base.mSpriteBody.*/ position * MeterInPixels;

            theSpriteBatch.Draw(base.mSpriteTexture, spritePos, base.Source,
                Color.White, base.mSpriteBody.Rotation, base.spriteOrigin,
                new Vector2(base.WidthScale, base.HeightScale), SpriteEffects.None, 0f);




        }




    }
}
