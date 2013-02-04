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
using FarseerPhysics.Collision;

namespace Gravitation.SpriteObjects
{
    class Upgrade : Sprite
    {

        public World mworld;
        public Vector2 mposition;
        public bool Visible = false;
        private bool upgradeSelected;
        private Dictionary<String,Int32> upgradeList; // <upgradeName, number of times used>
        private Random rnd = new Random();
        private bool move = false;
        private int upgradeTimer = 0;
        private int timeToWait = 0;

        private Dictionary <Vector2, Int32> spawnPoints =  new Dictionary<Vector2,int>();
        ContentManager mtheContentManager;
        GraphicsDeviceManager mgraphics;
        SpriteBatch mtheSpriteBatch;

        public Upgrade(World world, List<Vector2> spawnPoints, List<String> upgradeList, int upgradeTimer)
        {
            this.mworld = world;

            foreach (Vector2 x in spawnPoints)
            {
                this.spawnPoints.Add(x, 0); 
            }

            /*this.mposition.Y = (position.Y * MeterInPixels);
            this.mposition.X = (position.X * MeterInPixels);*/

            int index = (int) GetRandomNumber(0f, spawnPoints.Count());

            this.mposition = selectRandomSpawn()*MeterInPixels;//new Vector2((-0.5f * MeterInPixels),0);
            

            this.upgradeList = upgradeList.ToDictionary(v => v, v => 0);

            this.upgradeTimer = upgradeTimer;

        }


        public void LoadContent(ContentManager theContentManager, GraphicsDeviceManager graphics)
        {
            this.mtheContentManager = theContentManager;
            this.mgraphics = graphics;

            base.mSpriteBodyTexture = theContentManager.Load<Texture2D>("WhiteRect");
            base.mSpriteSheetTexture = theContentManager.Load<Texture2D>("WhiteRect");
            base.AssetName = "WhiteRect";
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



            base.mSpriteBody = BodyFactory.CreateCompoundPolygon(mworld, list, 0f, (mposition / MeterInPixels), BodyType.Kinematic);
            base.mSpriteBody.Restitution = 0.3f;
            base.mSpriteBody.Friction = 1f;
            //base.mSpriteBody.IsStatic = true;
            base.mSpriteBody.IgnoreGravity = true;

            base.mSpriteBody.OnCollision += Body_OnCollision;

            base.mSpriteBody.CollisionCategories = Category.Cat12;
            base.mSpriteBody.CollidesWith = Category.All & ~Category.Cat10;//Category.Cat1 | Category.Cat11;

            String selectedUpgrade = randomSelect(upgradeList);


            foreach (Fixture fixturec in base.mSpriteBody.FixtureList)
            {
                fixturec.UserData = selectedUpgrade;

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


                theSpriteBatch.Draw(base.mSpriteSheetTexture, spritePos, base.Source,
                    Color.White, 0, base.spriteOrigin,
                    new Vector2(base.WidthScale, base.HeightScale), SpriteEffects.None, 0f);

                //  mShotParticles.Draw();

            }


        }

        public void Update(GameTime gameTime, Matrix _view)
        {
                         

            if (Visible)
            {

                if (!upgradeSelected)
                {
                    upgradeSelected = true;
                    String selectedUpgrade = randomSelect(upgradeList);
                    foreach (Fixture fixturec in base.mSpriteBody.FixtureList)
                    {
                        fixturec.UserData = selectedUpgrade;

                    }

                }

 
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
                    String removeUpgrade = "";
                    foreach (Fixture fixturec in base.mSpriteBody.FixtureList)
                    {
                        fixturec.UserData = removeUpgrade;
                    }


                    int firstVal = timeToWait;
                    int secondVal = (int)gameTime.ElapsedGameTime.Milliseconds;

                    timeToWait += secondVal;//(secondVal - firstVal);

                    if (timeToWait >= (upgradeTimer*1000))
                    {
                        base.mSpriteBody.Position = selectRandomSpawn();
                        move = false;
                        Console.WriteLine("upgrade pos = " + base.mSpriteBody.Position);
                    }
                }
                else
                {

                    //Console.WriteLine("upgrade pos = " + base.mSpriteBody.Position);
                    timeToWait = 0;
                    Visible = true;
                }
            }
                
        }

        private bool Body_OnCollision(Fixture fixturea, Fixture fixtureb, Contact contact)
        {
            if (fixtureb.Body.IsBullet)
            {
                //collision with ship
                if (Visible)
                {
                    Visible = false;
                    move = true;
                    upgradeSelected = false;
                    return false;
                }
                else
                {
                    return false;
                }
                
            }
            else
            {
                return true;
            }
            

           
        }


        public String randomSelect( Dictionary<String,Int32> upgradeList)
        {

            String powerup = "";

            int index = (int)GetRandomNumber(0f, upgradeList.Count());
            int value = upgradeList.ElementAt(index).Value;

            if (value == upgradeList.Values.Min())
            {
                powerup = upgradeList.ElementAt(index).Key;
                upgradeList[upgradeList.ElementAt(index).Key]++;
            }
            else
            {

                foreach (KeyValuePair<string, int> pair in upgradeList)
                {

                    if (pair.Value == upgradeList.Values.Min())
                    {
                        powerup = pair.Key; // Founds
                        upgradeList[pair.Key]++;
                        break;
                    }
                }
            }

            return powerup;
        }


        public float GetRandomNumber(float minimum, float maximum)
        { 
            return (float) rnd.NextDouble() * (maximum - minimum) + minimum;
        }




        private Vector2 selectRandomSpawn()
        {

            Vector2 position = new Vector2();

            int index = (int)GetRandomNumber(0f, spawnPoints.Count());
            int value = spawnPoints.ElementAt(index).Value;

            if (value == spawnPoints.Values.Min())
            {
                position = spawnPoints.ElementAt(index).Key;
                spawnPoints[spawnPoints.ElementAt(index).Key]++; //this SHOULD increment the value associated with the key in the Dictionary
                                                                 //Also why the fuck doesn't c# have Hashmaps....like what the hell man .
            }
            else
            {
                for(int v = 0; v<spawnPoints.Values.Count; v++)
                {
                    if(spawnPoints.Values.ElementAt(v) == spawnPoints.Values.Min())
                    {
                        position = spawnPoints.Keys.ElementAt(v);
                        spawnPoints[spawnPoints.ElementAt(v).Key]++;
                        break;
                    }
                }

            }

           

            return position/MeterInPixels;

        }



    }





}
