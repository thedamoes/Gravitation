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
using Gravitation.Utils;

using DPSF;
using DPSF.ParticleSystems;


namespace Gravitation.SpriteObjects
{
    public class Shot : Sprite
    {
        public delegate void DestroyMeHandler(Shot shotToDestroy);

       // public ShotParticleSystem mShotParticles = null;
        public World mworld;
        public Vector2 mposition;
        public float mrotation;
        public Vector2 mDirection;
        public bool Visible = false;
        public bool removed = false;
        public int damage;

        private DestroyMeHandler ondeathHandler;
        private static String USERDATA;


        ContentManager mtheContentManager;
        GraphicsDeviceManager mgraphics;
        SpriteBatch mtheSpriteBatch;




        public Shot(World world, Vector2 position, float rotation, int damage, DestroyMeHandler onDeathHandler)
        {
            this.mworld = world;
            this.mrotation = rotation;

            this.mposition.Y = (position.Y * MeterInPixels) - (float)Math.Round(48f * Math.Sin(rotation + 1.57079633));
            this.mposition.X = (position.X * MeterInPixels) - (float)Math.Round(48f * Math.Cos(rotation + 1.57079633));

            this.damage = damage;
            this.ondeathHandler = onDeathHandler;

            USERDATA = "standard:" + damage;
        }


        public void LoadContent(ContentManager theContentManager, GraphicsDeviceManager graphics)
        {
            this.mtheContentManager = theContentManager;
            this.mgraphics = graphics;

            base.mSpriteBodyTexture = theContentManager.Load<Texture2D>("shot");
            base.mSpriteSheetTexture = theContentManager.Load<Texture2D>("shot");
            base.AssetName = "shot";
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


            base.mSpriteBody = BodyFactory.CreateCompoundPolygon(mworld, list, 0f, (mposition / MeterInPixels), BodyType.Dynamic);
            base.mSpriteBody.Restitution = 0.3f;
            base.mSpriteBody.Friction = 1f;
            base.mSpriteBody.IsStatic = false;
            base.mSpriteBody.IsBullet = true;
            base.mSpriteBody.IgnoreGravity = true;
            base.mSpriteBody.Mass = 0.00000001f;

            base.mSpriteBody.OnCollision += Body_OnCollision;

            base.mSpriteBody.CollisionCategories = Category.Cat10;
            base.mSpriteBody.CollidesWith = Category.All & ~Category.Cat10;//Category.Cat1 & Category.Cat11;

            foreach (Fixture fixturec in base.mSpriteBody.FixtureList)
            {
                fixturec.UserData = USERDATA;

            }

            
          //  mShotParticles = new ShotParticleSystem(null, base.mSpriteBody.Position * (MeterInPixels), mrotation, new Vector2(0, -20));
           // mShotParticles.AutoInitialize(mgraphics.GraphicsDevice, mtheContentManager, this.mtheSpriteBatch);

        }
        





        public override void Draw(SpriteBatch theSpriteBatch)
        {
            //Create a single body with multiple fixtures
            this.mtheSpriteBatch = theSpriteBatch;

            Vector2 spritePos = base.mSpriteBody.Position * MeterInPixels;

            if(Visible == true)
            {
                Vector3 _cameraZoom = new Vector3(0.5f, 0.5f, 0.5f);
                 
               //mShotParticles.SpriteBatchSettings.TransformationMatrix = Matrix.Identity * Matrix.CreateScale(_cameraZoom);    


                theSpriteBatch.Draw(base.mSpriteSheetTexture, spritePos, base.Source,
                Color.White, mrotation, base.spriteOrigin,
                new Vector2(base.WidthScale, base.HeightScale), SpriteEffects.None, 0f);

          //  mShotParticles.Draw();

            }


        }





        public void Update(GameTime gameTime, Matrix _view)
        {
            // Set Visible to false here On collision

            if (Visible == true)
            {
               /* mShotParticles.SpriteBatchSettings.TransformationMatrix = _view;
                mShotParticles.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                mShotParticles.UpdateParticleEmmiter(base.mSpriteBody.Position * (MeterInPixels), mrotation);

                foreach (DefaultSpriteParticle particle in mShotParticles.Particles)
                {
                    mShotParticles.UpdateParticle(particle, -rotateVector(mDirection, mrotation));
                }*/
                

                if (base.mSpriteBody.LinearVelocity.Y < -20)
                {
                    base.mSpriteBody.LinearVelocity = GravitationUtils.rotateVector(mDirection, mrotation);
                }
                else
                    base.mSpriteBody.LinearVelocity = GravitationUtils.rotateVector(mDirection, mrotation);
            }
            else
                return;
        }



        public void fire(Vector2 theStartPosition, float rotation, float shotSpeed)
        {

            this.mposition.Y = (theStartPosition.Y * MeterInPixels) - (float)Math.Round(88f * Math.Sin(rotation + 1.57079633));
            this.mposition.X = (theStartPosition.X * MeterInPixels) - (float)Math.Round(88f * Math.Cos(rotation + 1.57079633));


            mDirection = new Vector2(0, shotSpeed);

            Visible = true;

            mrotation = rotation;

            base.mSpriteBody.Rotation = mrotation;

            float shotAngle = mrotation;

            base.mSpriteBody.LinearVelocity = GravitationUtils.rotateVector(mDirection, mrotation);
        }

        private bool Body_OnCollision(Fixture fixturea, Fixture fixtureb, Contact contact)
        {
            if (!this.removed)
            {
                mworld.RemoveBody(base.mSpriteBody);
                this.ondeathHandler.Invoke(this);
            }
            this.removed = true;
            return true;
        }
    }
}
