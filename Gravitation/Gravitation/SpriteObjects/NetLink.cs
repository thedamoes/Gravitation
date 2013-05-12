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
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PhysicsLogic;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.DebugViews;

namespace Gravitation.SpriteObjects
{
    public class NetLink : AlternateShot
    {
        public int damageToDeal = 0;
        public int damageTaken = 0;
        public bool contactMade = false;

        private Joint fixedJoint = null;

        private String netLinkTextureName = "NetLinkSize";
        private String netLinkSpriteSheet = "NetLinkSprite";

        private int spriteSourceHeight = 4; 
        private int spriteSourceWidth = 8; 
        private int totalframes = 1;
        private int framesPerSec = 1;

        int spriteHeightScale = 2;
        int spriteWidthScale = 2;

        private static String USERDATA;

        public NetLink(World world, Vector2 position, float rotation, DestroyMeHandler onDeathHandler)
        {
            base.mworld = world;

            base.mrotation = rotation;

            base.mposition.Y = (position.Y * MeterInPixels) - (float)Math.Round(48f * Math.Sin(rotation + 1.57079633));
            base.mposition.X = (position.X * MeterInPixels) - (float)Math.Round(48f * Math.Cos(rotation + 1.57079633)); 

            base.ondeathHandler = onDeathHandler;

            USERDATA = "net:" + 0;

            base.setTotalFrames(totalframes);

            base.setFramesPerSec(framesPerSec);

            base.setSpriteSourceHeightAndWidth(spriteSourceHeight, spriteSourceWidth);

        }

        public void LoadContent(ContentManager theContentManager, GraphicsDeviceManager graphics)
        {
            base.LoadContent(theContentManager, graphics, netLinkTextureName, netLinkSpriteSheet, spriteHeightScale, spriteWidthScale);

            base.mSpriteBody.OnCollision += Body_OnCollision;
            base.mSpriteBody.Mass = 0.01f;
            base.mSpriteBody.IgnoreGravity = false;
            base.mSpriteBody.CollisionCategories = Category.Cat13;
            base.mSpriteBody.CollidesWith = Category.All & ~Category.Cat13 & ~Category.Cat12;

            base.mSpriteBody.Rotation = mrotation;

            foreach (Fixture fixturec in base.mSpriteBody.FixtureList)
            {
                fixturec.UserData = USERDATA;
            }
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {

            Vector2 spritePos = base.mSpriteBody.Position * MeterInPixels;


            Vector3 _cameraZoom = new Vector3(0.5f, 0.5f, 0.5f);

            theSpriteBatch.Draw(base.mSpriteSheetTexture, spritePos, base.Source,
            Color.White, base.mSpriteBody.Rotation, base.spriteOrigin,
            new Vector2(base.WidthScale, base.HeightScale), SpriteEffects.None, 0f);
        }


        public override void Update(GameTime gameTime, Matrix _view)
        {

            if (base.Visible == true && !contactMade)
            {
                if (base.mSpriteBody.LinearVelocity.Y < -2) // lower velocity than a bullet (so as not to be completely un dodgeable) though the initial speed is decided by the Ship.alternateFire() method
                {
                    base.mSpriteBody.LinearVelocity = rotateVector(mDirection, mrotation);
                }
                else
                {
                    base.mSpriteBody.LinearVelocity = rotateVector(mDirection, mrotation);
                }

            }
            else
                return;
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


                    switch (shotType)
                    {
                        case ("standard"):
                            {
                                this.damageTaken += (shotEffect / 2);
                                break;
                            }
                    }
                }
            }
            else
            {
                if (fixedJoint == null && Convert.ToString(fixtureb.UserData).Equals("ship"))
                {

                    Manifold manifold;
                    contact.GetManifold(out manifold);
                    fixedJoint = new DistanceJoint(base.mSpriteBody, fixtureb.Body, new Vector2(0, 0), manifold.Points[0].LocalPoint);
                    fixedJoint.CollideConnected = true;
                    mworld.AddJoint(fixedJoint);
                }

                this.contactMade = true;
                
            }

            return true;

        }



        public void removeObject()
        {
            mworld.RemoveBody(base.mSpriteBody);
            this.ondeathHandler.Invoke(this);
            if (fixedJoint != null)
            {
                mworld.RemoveJoint(fixedJoint);
            }
            this.removed = true;
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
