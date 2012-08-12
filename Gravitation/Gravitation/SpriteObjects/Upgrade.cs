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
using FarseerPhysics.DebugViews;

using DPSF;
using DPSF.ParticleSystems;

namespace Gravitation.SpriteObjects
{
    class Upgrade : Sprite
    {

        public World mworld;
        public Vector2 mposition;
        public bool Visible = false;
        private Dictionary<String,Int32> upgradeList;
        private Random rnd = new Random();
        private bool move = false;

        private Vector2 mapTopLeft;
        private Vector2 mapBottomRight;

        ContentManager mtheContentManager;
        GraphicsDeviceManager mgraphics;
        SpriteBatch mtheSpriteBatch;

        public Upgrade(World world, Vector2 mapTopLeft, Vector2 mapBottomRight, List<String> upgradeList)
        {
            this.mworld = world;


            this.mapTopLeft = mapTopLeft;
            this.mapBottomRight = mapBottomRight;


            /*this.mposition.Y = (position.Y * MeterInPixels);
            this.mposition.X = (position.X * MeterInPixels);*/

            this.mposition = randomBetweenTwoVectors(mapTopLeft, mapBottomRight);//MeterInPixels;
            Console.WriteLine("upgrade pos = " + this.mposition);

            this.upgradeList = upgradeList.ToDictionary(v => v, v => 0);

        }


        public void LoadContent(ContentManager theContentManager, GraphicsDeviceManager graphics)
        {
            this.mtheContentManager = theContentManager;
            this.mgraphics = graphics;

            base.mSpriteTexture = theContentManager.Load<Texture2D>("WhiteRect");
            base.AssetName = "WhiteRect";
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



            base.mSpriteBody = BodyFactory.CreateCompoundPolygon(mworld, list, 0f, (mposition / MeterInPixels), BodyType.Dynamic);
            base.mSpriteBody.Restitution = 0.3f;
            base.mSpriteBody.Friction = 1f;
            //base.mSpriteBody.IsStatic = true;
            base.mSpriteBody.IgnoreGravity = true;

            base.mSpriteBody.OnCollision += Body_OnCollision;

            base.mSpriteBody.CollisionCategories = Category.Cat12;
            base.mSpriteBody.CollidesWith = Category.All & ~Category.Cat10;//Category.Cat1 | Category.Cat11;

            foreach (Fixture fixturec in base.mSpriteBody.FixtureList)
            {
                fixturec.UserData = randomSelect(upgradeList);

            }


            //  mShotParticles = new ShotParticleSystem(null, base.mSpriteBody.Position * (MeterInPixels), mrotation, new Vector2(0, -20));
            // mShotParticles.AutoInitialize(mgraphics.GraphicsDevice, mtheContentManager, this.mtheSpriteBatch);

        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            //Create a single body with multiple fixtures
            this.mtheSpriteBatch = theSpriteBatch;

            Vector2 spritePos = base.mSpriteBody.Position * MeterInPixels;

            if (Visible == true)
            {
                Vector3 _cameraZoom = new Vector3(0.5f, 0.5f, 0.5f);

                //mShotParticles.SpriteBatchSettings.TransformationMatrix = Matrix.Identity * Matrix.CreateScale(_cameraZoom);    


                theSpriteBatch.Draw(base.mSpriteTexture, spritePos, base.Source,
                    Color.White, 0, base.spriteOrigin,
                    new Vector2(base.WidthScale, base.HeightScale), SpriteEffects.None, 0f);

                //  mShotParticles.Draw();

            }


        }

        public void Update(GameTime gameTime, Matrix _view)
        {

            if (Visible)
            {
                /* mShotParticles.SpriteBatchSettings.TransformationMatrix = _view;
                 mShotParticles.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                 mShotParticles.UpdateParticleEmmiter(base.mSpriteBody.Position * (MeterInPixels), mrotation);

                 foreach (DefaultSpriteParticle particle in mShotParticles.Particles)
                 {
                     mShotParticles.UpdateParticle(particle, -rotateVector(mDirection, mrotation));
                 }*/

            }
            else if (Visible == false)
            {
                if (move)
                {
                    base.mSpriteBody.Position = randomBetweenTwoVectors(mapTopLeft, mapBottomRight);
                    move = false;
                    Console.WriteLine("upgrade pos = " + base.mSpriteBody.Position);
                }
                else
                {
                    Visible = true;
                }
            }
                
        }

        private bool Body_OnCollision(Fixture fixturea, Fixture fixtureb, Contact contact)
        {
            if (fixtureb.Body.IsBullet)
            {
                //collision with ship
                Visible = false;
                move = true;
                return false;
            }
            else //if (Convert.ToString(base.mSpriteBody.ContactList.Other.UserData).Equals("Dynamic"))
            {
                //collision with wall; need to move
                move = true;
                return true;
            }
            

           
        }


        public String randomSelect( Dictionary<String,Int32> upgradeList)
        {

            String powerup = "";
            foreach (KeyValuePair<string, int> pair in upgradeList)
            {

                if (pair.Value == upgradeList.Values.Min())
                {
                    powerup = pair.Key; // Found
                    break;
                }
            }

            return powerup;
        }


        public Vector2 randomBetweenTwoVectors(Vector2 first, Vector2 second)
        {
            return new Vector2(GetRandomNumber(first.X, second.X), GetRandomNumber(first.Y, second.Y));
        }


        public float GetRandomNumber(float minimum, float maximum)
        { 
            return (float) rnd.NextDouble() * (maximum - minimum) + minimum;
        }




    }





}
