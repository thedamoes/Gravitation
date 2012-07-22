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
    public class Ship : Sprite
    {

        public World world;

        public List<SpriteObjects.Shot> mShots = new List<SpriteObjects.Shot>();
        public List<SpriteObjects.Shot> remove_Shots = new List<SpriteObjects.Shot>();
        public ShipParticleSystem mShipParticles = null;

        public int sheilds = 100;
        public int damage = 25;        

        ContentManager theContentManager;
        GraphicsDeviceManager graphics;
        SpriteBatch mtheSpriteBatch;

        private Vector2 mPosition;

        private SoundHandler mPlayer;

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

        public Ship(SoundHandler player)
        {
            this.world = null;
            this.mPosition = new Vector2(100f, 100f);

            this.mPlayer = player;
        }

        public Ship()
        {
            this.world = null;
            this.mPosition = new Vector2(100f, 100f);
            this.mPlayer = null;
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName, GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
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


            base.mSpriteBody.OnCollision += Body_OnCollision;

            base.mSpriteBody.CollisionCategories = Category.Cat11;
            base.mSpriteBody.CollidesWith = Category.All;

            mShipParticles = new ShipParticleSystem(null, base.mSpriteBody.Position * (MeterInPixels), base.mSpriteBody.Rotation, new Vector2(0, -20));
            mShipParticles.AutoInitialize(graphics.GraphicsDevice, theContentManager, this.mtheSpriteBatch);

            
        }


        public void setPlayer(SoundHandler player)
        {
            this.mPlayer = player;
        }

        public void fire()
        {

                SpriteObjects.Shot aShot = new SpriteObjects.Shot(world, base.mSpriteBody.Position, base.mSpriteBody.Rotation, damage);

                aShot.LoadContent(theContentManager, graphics);

                aShot.fire(base.mSpriteBody.Position, base.mSpriteBody.Rotation);

                mShots.Add(aShot);
                remove_Shots.Add(aShot);

                mPlayer.playSound(SoundHandler.Sounds.SHIP_FIRE1);
        }

        public void updateShot(GameTime gameTime, Matrix _view)
        {
            foreach (SpriteObjects.Shot aShot in mShots)
            {
                if(aShot.Visible == true)
                    aShot.Update(gameTime, _view);
            }
        }

        public void shortRomoved()
        {
            foreach (SpriteObjects.Shot aShot in remove_Shots)
            {
                if(aShot.Visible == false && aShot.removed == false)
                {
                    //world.RemoveBody(aShot.mSpriteBody);
                    mShots.Remove(aShot);
                    aShot.removed = true;
                }
            }

        }


        public void thrust(GameTime gameTime, Matrix _view)
        {
            mShipParticles.SpriteBatchSettings.TransformationMatrix = _view;
            mShipParticles.Update((float)gameTime.ElapsedGameTime.TotalSeconds);          

            mShipParticles.UpdateParticleEmmiter(base.mSpriteBody.Position * (MeterInPixels), base.mSpriteBody.Rotation, base.mSpriteBody.LinearVelocity);

            foreach (DefaultSpriteParticle particle in mShipParticles.Particles)
            {
                //mShipParticles.UpdateParticle(particle, -rotateVector(new Vector2(0, (-2 +(-0.1f * (float)Math.Sqrt((base.mSpriteBody.LinearVelocity.Y) * (base.mSpriteBody.LinearVelocity.Y))))), base.mSpriteBody.Rotation));
                mShipParticles.UpdateParticle(particle, rotateVector(new Vector2(0, 2), base.mSpriteBody.Rotation));
            }
        }



        private bool Body_OnCollision(Fixture fixturea, Fixture fixtureb, Contact contact)
        {

          if(fixtureb.Body.IsBullet)
          {

              int damage = Convert.ToInt32(fixtureb.UserData); 

              sheilds -= damage;

              //been shot
              return true;
          }

            return true;
        }



        public override void Draw(SpriteBatch theSpriteBatch)
        {
            //Create a single body with multiple fixtures
            this.mtheSpriteBatch = theSpriteBatch;

            Vector2 spritePos = base.mSpriteBody.Position * MeterInPixels;

            theSpriteBatch.Draw(base.mSpriteTexture, spritePos, base.Source,
                Color.White, base.mSpriteBody.Rotation, base.spriteOrigin,
                new Vector2(base.WidthScale, base.HeightScale), SpriteEffects.None, 0f);


            mShipParticles.Draw();

            foreach (SpriteObjects.Shot aShot in mShots)
            {
                if (aShot.Visible == true)
                    aShot.Draw(theSpriteBatch);
            }
           
        }


        private Vector2 rotateVector(Vector2 direction, float angle)
        {
            Vector2 newvec = new Vector2();

            newvec.X = (float)((Math.Cos(angle) * direction.X) - (Math.Sin(angle) * direction.Y));
            newvec.Y = (float)((Math.Sin(angle) * direction.X) + (Math.Cos(angle) * direction.Y));

            return newvec;
        }

    }
}
