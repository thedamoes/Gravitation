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
using Gravitation.Utils;

using DPSF;
using DPSF.ParticleSystems;


namespace Gravitation.SpriteObjects
{
    public class Ship : Sprite, HUD.IHasLife
    {

        public World world;

        private List<SpriteObjects.Shot> mShots = new List<SpriteObjects.Shot>();
        public List<SpriteObjects.AlternateShot> altShots = new List<AlternateShot>();
        public ShipParticleSystem mShipParticles = null;

        public int sheilds = 100;
        public int damage = 25;

        public int shotsPerSec = 3;
        public float shotSpeed = -20;
        

        public fireState currentFireState = fireState.Standard;
        public passiveState currentPassiveState = passiveState.Standard;
        public secondaryFire currentSecondaryFire = secondaryFire.Standard;
        public negativeState currentNegativeState = negativeState.Standard;

        public int altFireQuantity = 0;

        public enum fireState 
        {
            Standard,
            Shotgun,
            Machinegun,
            Laser,
            Heavyshot, //basically has high mass
            SlowShot, //shotSpeed is reduced, not ROF
            DoubleShot,
        };

        public enum secondaryFire
        {
            EmpShot,
            GuidedShot, 
            Mines,
            Turrets,
            Tether,
            WeponisedThruster,
            Net,
            HeavyNet,
            FreezeNet,
            Drones,
            RocketPods,
            RearGunner,
            BouncyShots,
            Standard
        };

        public enum passiveState
        {
            Standard,
            SpiralFire,
            NoGravity,
            AutoCannon,
            Invincible,
            IncreaseROF
        };

        public enum negativeState
        {
            Standard,
            Emped,
            HighGravity,
            InverseGravity,
            ReverseControls,
            DecreaseROF
        };

        public bool isAIControlled
        {
            get { return this.aiControlled; }
            set { this.aiControlled = value; }
        }


        public List<float> getRayCastLengths
        {
            get { return this.rayCastLengths; }
        }

        ContentManager theContentManager;
        GraphicsDeviceManager graphics;
        SpriteBatch mtheSpriteBatch;

        private Vector2 mPosition;

        private String shipId;

        private SoundHandler mPlayer;

        private Random rand = new Random();
        private bool timer = true;
        private bool altTimer = true;
        private bool aiControlled = false;
        private int time = 0;
        private int altTime = 0;
        private float spiralShotRotation = 0;
        private int negativeEffectTime = 0;

        private static int numberOfRayCasts = 8;
        private static float radianIncrementation = (float)Math.PI * (360f / numberOfRayCasts) / 180.0f;

        private List<bool> raycastHitList = new List<bool>(numberOfRayCasts);

        private List<Vector2> raycastPointList = new List<Vector2>(numberOfRayCasts);
        private List<Vector2> raycastStartPointList = new List<Vector2>(numberOfRayCasts);
        private List<Vector2> raycastEndPointList = new List<Vector2>(numberOfRayCasts);
        private List<Vector2> raycastNormalList = new List<Vector2>(numberOfRayCasts);

        public static float BASE_SHIP_RAYCAST_LENGTH = -5;
        private List<float> rayCastLengths = new List<float>();

        #region Properties
        public Vector2 ShipPosition
        {
            set { mPosition = value; }
            get { return base.Position; }
        }

        public String ShipId
        {
            set { shipId = value; }
            get { return this.shipId; }
        }
        public World World
        {
            set { world = value; }
        }
        public int SheildValue
        {
            get { return this.sheilds; }
        }
        public int HealthValue
        {
            get { return this.damage; }
        }
        #endregion


        public Ship(SoundHandler player, float power, float sheildStrength)
        {
            this.world = null;
            this.mPosition = new Vector2(100f, 100f);

            this.mPlayer = player;

            this.damage = (int)power; // need to change this 
            this.sheilds = this.sheilds - (10*(int)sheildStrength); // and this too
            
            Vector2 y = new Vector2(0,0);

            for (int x = 0; x < numberOfRayCasts; x++)
            {
                raycastHitList.Add(false);

                raycastPointList.Add(y);
                raycastStartPointList.Add(y);
                raycastEndPointList.Add(y);
                raycastNormalList.Add(y);
                rayCastLengths.Add(BASE_SHIP_RAYCAST_LENGTH);
            }


        }

        public void LoadContent(ContentManager theContentManager, string theAssetName, GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            this.theContentManager = theContentManager;
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

            base.mSpriteBody = BodyFactory.CreateCompoundPolygon(world, list, 1f, (mPosition / MeterInPixels), BodyType.Dynamic);
            base.mSpriteBody.Restitution = 0.3f;
            base.mSpriteBody.Friction = 1f;
            base.mSpriteBody.IsStatic = false;
            base.mSpriteBody.IsBullet = true;

            base.mSpriteBody.OnCollision += Body_OnCollision;

            base.mSpriteBody.CollisionCategories = Category.Cat11;
            base.mSpriteBody.CollidesWith = Category.All;

            foreach (Fixture fixturec in base.mSpriteBody.FixtureList)
            {
                fixturec.UserData = "ship"+":"+shipId;

            }

            mShipParticles = new ShipParticleSystem(null, base.mSpriteBody.Position * (MeterInPixels), base.mSpriteBody.Rotation, new Vector2(0, -20));
            mShipParticles.AutoInitialize(graphics.GraphicsDevice, theContentManager, this.mtheSpriteBatch);

            base.mSpriteBody.LinearDamping = 1f;
        }


        public void setPlayer(SoundHandler player)
        {
            this.mPlayer = player;
        }

        public void fire()
        {
            switch(currentFireState)
            {
                case (fireState.Standard):
                    {
                        if (timer)
                        {
                            SpriteObjects.Shot aShot = new SpriteObjects.Shot(world, base.mSpriteBody.Position, base.mSpriteBody.Rotation, damage, this.removeShot);

                            aShot.LoadContent(theContentManager, graphics);

                            //foreach (Fixture fixturec in aShot.mSpriteBody.FixtureList)
                            //{
                            //    fixturec.UserData = fixturec.UserData + ":" + this.shipId;  // TODO : setup shots to have shipid , so as to avoid friendly fire, also so raycast doesn't catch own shots

                            //}

                            aShot.fire(base.mSpriteBody.Position, base.mSpriteBody.Rotation, shotSpeed);
                            mShots.Add(aShot);
                            mPlayer.playSound(SoundHandler.Sounds.SHIP_FIRE1);

                            timer = false;
                        }
                        break;
                    }
                case (fireState.Shotgun):
                    {
                        if (timer)
                        {

                            SpriteObjects.Shot aShot = new SpriteObjects.Shot(world, base.mSpriteBody.Position, base.mSpriteBody.Rotation, damage, this.removeShot);
                            SpriteObjects.Shot bShot = new SpriteObjects.Shot(world, base.mSpriteBody.Position, base.mSpriteBody.Rotation, damage, this.removeShot);
                            SpriteObjects.Shot cShot = new SpriteObjects.Shot(world, base.mSpriteBody.Position, base.mSpriteBody.Rotation, damage, this.removeShot);

                            aShot.LoadContent(theContentManager, graphics);
                            bShot.LoadContent(theContentManager, graphics);
                            cShot.LoadContent(theContentManager, graphics);

                            aShot.fire(base.mSpriteBody.Position, base.mSpriteBody.Rotation, shotSpeed);
                            bShot.fire(base.mSpriteBody.Position, base.mSpriteBody.Rotation + 0.05f, shotSpeed);
                            cShot.fire(base.mSpriteBody.Position, base.mSpriteBody.Rotation - 0.05f, shotSpeed);

                            mShots.Add(aShot);
                            mShots.Add(bShot);
                            mShots.Add(cShot);

                            mPlayer.playSound(SoundHandler.Sounds.SHIP_FIRE1);

                            timer = false;
                        }

                        break;
                    }
                case(fireState.Laser):
                    {
                        if (timer)
                        {                           
                            SpriteObjects.Shot aShot = new SpriteObjects.Shot(world, base.mSpriteBody.Position, base.mSpriteBody.Rotation, 1, this.removeShot);

                            aShot.LoadContent(theContentManager, graphics);
                            aShot.fire(base.mSpriteBody.Position, base.mSpriteBody.Rotation, (shotSpeed * 100));
                            mShots.Add(aShot);
                            timer = false;
                        }
                        break;
                    }
                case (fireState.Machinegun):
                    {
                        if(timer)
                        {
                            float randomRotation = rand.Next(-1, 1);
                            float secondRotation = rand.Next(-9,9);

                            secondRotation = secondRotation / 100;

                            randomRotation = (randomRotation / 10) + secondRotation;

                            SpriteObjects.Shot aShot = new SpriteObjects.Shot(world, base.mSpriteBody.Position, base.mSpriteBody.Rotation, damage, this.removeShot);

                            aShot.LoadContent(theContentManager, graphics);
                            aShot.fire(base.mSpriteBody.Position, base.mSpriteBody.Rotation + randomRotation, shotSpeed);
                            mShots.Add(aShot);
                            mPlayer.playSound(SoundHandler.Sounds.SHIP_FIRE1);

                            timer = false;
                        }

                        break;
                    }
            }
        }

        public void updateShot(GameTime gameTime, Matrix _view)
        {
            time += gameTime.ElapsedGameTime.Milliseconds;

            switch(currentFireState)
            {
                case (fireState.Standard):
                    {
                        if (time >= (1000 / shotsPerSec)) //250
                        {
                            timer = true;
                            time = 0;
                        }
                        break;
                    }
                case(fireState.Shotgun):
                    {
                        if (time >= (1000 / shotsPerSec)) //250
                        {
                            timer = true;
                            time = 0;
                        }
                        break;
                    }
                case(fireState.Laser):
                    {
                        timer = true;
                        break;
                    }
                case(fireState.Machinegun):
                    {
                        if (time >= (500 / shotsPerSec)) //250
                        {
                            timer = true;
                            time = 0;
                        }
                        break;
                    }
            }

            foreach (SpriteObjects.Shot aShot in mShots)
            {
                if(aShot.Visible == true)
                    aShot.Update(gameTime, _view);
            }
        }

        private void removeShot(Shot shotToRemove)
        {
            mShots.Remove(shotToRemove);
        }


        public void altFire()
        {
            switch(currentSecondaryFire){

                case(secondaryFire.EmpShot):
                {
                    if (altFireQuantity > 0)
                    {
                        if (altTimer)
                        {
                            SpriteObjects.EmpShot aShot = new SpriteObjects.EmpShot(world, base.mSpriteBody.Position, base.mSpriteBody.Rotation, this.removeAltShot);

                            aShot.LoadContent(theContentManager, graphics);
                            aShot.altFire(base.mSpriteBody.Position, base.mSpriteBody.Rotation, shotSpeed); 
                            altShots.Add(aShot);
                            mPlayer.playSound(SoundHandler.Sounds.SHIP_FIRE1);

                            altTimer = false;
                            altFireQuantity--;
                        }
                    }
                    break;
                }
                case (secondaryFire.Net):
                {
                    if (altFireQuantity > 0)
                    {
                        if (altTimer)
                        {

                            SpriteObjects.Net aNet = new SpriteObjects.Net(world, base.mSpriteBody.Position, base.mSpriteBody.Rotation, this.removeAltShot);
                            aNet.LoadContent(theContentManager, graphics);
                            aNet.altFire(base.mSpriteBody.Position, base.mSpriteBody.Rotation, (shotSpeed / 2)); // lower velocity than a bullet (so as not to be completely un dodgeable)
                            altShots.Add(aNet);
                            mPlayer.playSound(SoundHandler.Sounds.SHIP_FIRE1);

                            altTimer = false;
                            altFireQuantity--;
                        }
                    }

                    break;
                }

            }
        }


        public void updateAltShot(GameTime gameTime, Matrix _view)
        {
            altTime += gameTime.ElapsedGameTime.Milliseconds;

            switch (currentSecondaryFire) //standard does nothing ie. empty
            {
                case (secondaryFire.EmpShot):
                    {
                        if (altTime >= (1000 / shotsPerSec)) //3 shots per sec
                        {
                            altTimer = true;
                            altTime = 0;
                        }
                        break;
                    }
                case (secondaryFire.Net):
                    {
                        if (altTime >= (3000 / shotsPerSec)) //1 shots per sec
                        {
                            altTimer = true;
                            altTime = 0;
                        }
                        break;
                    }
            }

            //foreach(SpriteObjects.AlternateShot shot in altShots)
            for (int x = 0; x < altShots.Count; x++ )
            {
                if (altShots.ElementAt(x).Visible == true)
                    altShots.ElementAt(x).Update(gameTime, _view);
            }
        }

        private void removeAltShot(AlternateShot shotToRemove)
        {
            altShots.Remove(shotToRemove);
        }

        public void updatePassiveShipState(GameTime gameTime, Matrix _view)
        {

            switch (currentPassiveState)
            {
                case (passiveState.SpiralFire):
                    {
                        SpriteObjects.Shot aShot = new SpriteObjects.Shot(world, base.mSpriteBody.Position, spiralShotRotation, damage, this.removeShot);

                        aShot.LoadContent(theContentManager, graphics);
                        aShot.fire(base.mSpriteBody.Position, spiralShotRotation, shotSpeed);
                        mShots.Add(aShot);

                        spiralShotRotation += 0.1f;

                        if(spiralShotRotation == 6.3){
                            spiralShotRotation = 0;
                        }
                        
                        break;
                    }
            }

            if (isAIControlled)
            {
                Vector2 raycaststart;
                Vector2 raycastD;

                for (int x = 0; x < numberOfRayCasts; x++)
                {
                    raycaststart = this.mSpriteBody.Position + GravitationUtils.rotateVector(new Vector2(0, -0.6f), this.mSpriteBody.Rotation + (radianIncrementation * x));

                    raycastStartPointList[x] = raycaststart;

                    raycastD = GravitationUtils.rotateVector(new Vector2(0, BASE_SHIP_RAYCAST_LENGTH), this.mSpriteBody.Rotation + (radianIncrementation * x));
                    raycastEndPointList[x] = raycastStartPointList[x] + raycastD;

                    raycastPointList[x] = Vector2.Zero;
                    raycastNormalList[x] = Vector2.Zero;
                    raycastHitList[x] = false;

                    world.RayCast((f, p, n, fr) =>
                    {
                        //String data = Convert.ToString(f.UserData);
                        //Console.WriteLine("Ship data retrieved = " + data);
                        raycastHitList[x] = true;
                        raycastPointList[x] = p;
                        raycastNormalList[x] = n;
                        return fr;
                    }, raycastStartPointList[x], raycastEndPointList[x]);

                }
            }


        }

        public void updateNegativeShipState(GameTime gameTime, Matrix _view)
        {
            switch (currentNegativeState)
            {
                case (negativeState.Emped):
                    {
                        negativeEffectTime -= gameTime.ElapsedGameTime.Milliseconds;

                        if (negativeEffectTime <= 0)
                        {
                            currentNegativeState = negativeState.Standard;
                            negativeEffectTime = 0;
                        }
                        break;
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
                mShipParticles.UpdateParticle(particle, GravitationUtils.rotateVector(new Vector2(0, 10), base.mSpriteBody.Rotation), base.mSpriteBody.Rotation);
            }

        }

        public void playThrustSound()
        {
                mPlayer.playSound(SoundHandler.Sounds.SHIP_THRUST1, 1f);
        }

        public void pauseThrustSound()
        {
            mPlayer.pauseSound(SoundHandler.Sounds.SHIP_THRUST1);
        }


        private bool Body_OnCollision(Fixture fixturea, Fixture fixtureb, Contact contact)
        {

            if (!Convert.ToString(fixtureb.UserData).Equals("wall") && !Convert.ToString(fixtureb.UserData).Equals("ship"))
            {
                if (fixtureb.Body.IsBullet)
                {
                    String data = Convert.ToString(fixtureb.UserData);
                    String[] splitdata = data.Split(':');
                    String shotType = splitdata[0];
                    int shotEffect = Convert.ToInt32(splitdata[1]);
                    
                    
                    switch(shotType)
                    {
                        case("standard"):
                        {
                            sheilds -= shotEffect;
                            break;
                        }

                        case ("emp"):
                        {
                            currentNegativeState = negativeState.Emped;
                            negativeEffectTime = (shotEffect*1000);
                            break;
                        }

                        case ("net"):
                        {
                            sheilds -= shotEffect;
                            if(shotEffect > 0)
                            {
                                Console.Out.WriteLine("Damage = " + shotEffect);
                                Console.Out.WriteLine("sheilds = " + sheilds);
                            }
                            break;
                        }


                    }
                    //been shot
                    return true;
                }
                else if (!Convert.ToString(fixtureb.UserData).Equals(""))
                {
                    String powerupData = Convert.ToString(fixtureb.UserData);

                    switch (powerupData)
                    {
                        case "shield":
                            {
                                sheilds = 200;
                                break;
                            }
                        case "Shotgun":
                            {
                                currentFireState = fireState.Shotgun;
                                break;
                            }
                        case "Laser":
                            {
                                currentFireState = fireState.Laser;
                                break;
                            }
                        case "Machinegun":
                            {
                                currentFireState = fireState.Machinegun;
                                break;
                            }
                        case "Spiral":
                            {
                                currentPassiveState = passiveState.SpiralFire;
                                break;
                            }
                        case "Emp":
                            {
                                currentSecondaryFire = secondaryFire.EmpShot;
                                altFireQuantity = 5;
                                break;
                            }
                        case "Net":
                            {
                                currentSecondaryFire = secondaryFire.Net;
                                altFireQuantity = 1;
                                break;
                            }
                    }

                    return false; //powerup
                }
                else
                {
                    return true;
                }
            }
            else
            {
                float Yvel = base.mSpriteBody.LinearVelocity.Y;
                float Xvel = base.mSpriteBody.LinearVelocity.X;

                int wallDamage = (int)Math.Max(Math.Sqrt(Yvel*Yvel), Math.Sqrt(Xvel*Xvel));

                sheilds -= wallDamage;

                return true;
            }

           
        }


        public override void Draw(SpriteBatch theSpriteBatch, DebugViewXNA debugView, Matrix projection, Matrix view)
        {

            //Create a single body with multiple fixtures
            this.mtheSpriteBatch = theSpriteBatch;

            Vector2 spritePos = base.mSpriteBody.Position * MeterInPixels;

            theSpriteBatch.Draw(base.mSpriteSheetTexture, spritePos, base.Source,
                Color.White, base.mSpriteBody.Rotation, base.spriteOrigin,
                new Vector2(base.WidthScale, base.HeightScale), SpriteEffects.None, 0f);


            mShipParticles.Draw();

            foreach (SpriteObjects.Shot aShot in mShots)
            {
                if (aShot.Visible == true)
                    aShot.Draw(theSpriteBatch);
            }

            foreach (SpriteObjects.AlternateShot aShot in altShots)
            {
                if (aShot.Visible == true)
                    aShot.Draw(theSpriteBatch);
            }

            if (isAIControlled)
            {
                for (int x = 0; x < numberOfRayCasts; x++)
                {
                    if (raycastHitList[x])
                    {
                        debugView.BeginCustomDraw(ref projection, ref view);
                        debugView.DrawPoint(raycastPointList[x], .5f, new Color(0.4f, 0.9f, 0.4f));

                        debugView.DrawSegment(raycastStartPointList[x], raycastPointList[x], new Color(0.8f, 0.8f, 0.8f));

                        Vector2 head = raycastPointList[x] + 0.5f * raycastNormalList[x];
                        debugView.DrawSegment(raycastPointList[x], head, new Color(0.9f, 0.9f, 0.4f));
                        debugView.EndCustomDraw();

                        //d = √ (x₂ - x₁)^2 + (y₂ - y₁)^2
                        double x1 = raycastStartPointList[x].X;
                        double x2 = raycastPointList[x].X;
                        double y1 = raycastStartPointList[x].Y;
                        double y2 = raycastPointList[x].Y;

                        double xPoints = (x2 - x1) * (x2 - x1);
                        double yPoints = (y2 - y1) * (y2 - y1);


                        float totalLength = (float) Math.Sqrt((xPoints + yPoints));

                        if(rayCastLengths.Count < numberOfRayCasts) {
                            rayCastLengths.Add(totalLength);
                        } else {
                            rayCastLengths[x] = totalLength;
                        }

                        //Console.WriteLine("Total length of ship line ["+x+"] = " + totalLength);

                    }
                    else
                    {
                        debugView.BeginCustomDraw(ref projection, ref view);
                        debugView.DrawSegment(raycastStartPointList[x], raycastEndPointList[x], new Color(0.8f, 0.8f, 0.8f));
                        debugView.EndCustomDraw();


                        // Could do the same calculation with start and enpoints here but
                        //its a bit pointless since the number will always be the constant

                        if (rayCastLengths.Count < numberOfRayCasts)
                        {
                            rayCastLengths.Add(-BASE_SHIP_RAYCAST_LENGTH); 
                        }
                        else
                        {
                            rayCastLengths[x] = -BASE_SHIP_RAYCAST_LENGTH;
                        }
                    }
                }
            }
        }
    }
}
