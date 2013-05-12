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
using FarseerPhysics.Common.PhysicsLogic;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.DebugViews;

using DPSF;
using DPSF.ParticleSystems;

namespace Gravitation.SpriteObjects
{
    public class EmpShot : AlternateShot
    {
        //public delegate void DestroyMeHandler(AlternateShot shotToDestroy);

       // public ShotParticleSystem mShotParticles = null;
        public int effectTime;
        private String empTextureName = "empShotSize";
        private String empSpriteSheet = "empSprite";

        private int spriteSourceHeight = 30; //pixels
        private int spriteSourceWidth = 30; //pixels
        private int totalframes = 2;
        private int framesPerSec = 4;

        int spriteHeightScale = 3;
        int spriteWidthScale = 3;

        private static String USERDATA;

        public EmpShot(World world, Vector2 position, float rotation, DestroyMeHandler onDeathHandler)
        {
            base.mworld = world;

            base.mrotation = rotation;

            this.effectTime = 5; //5 secs

            base.mposition.Y = (position.Y * MeterInPixels) - (float)Math.Round(48f * Math.Sin(rotation + 1.57079633));
            base.mposition.X = (position.X * MeterInPixels) - (float)Math.Round(48f * Math.Cos(rotation + 1.57079633));

            base.ondeathHandler = onDeathHandler;

            USERDATA = "emp:" + effectTime;

            base.setTotalFrames(totalframes);

            base.setFramesPerSec(framesPerSec);

            base.setSpriteSourceHeightAndWidth(spriteSourceHeight, spriteSourceWidth);

        }

        public void LoadContent(ContentManager theContentManager, GraphicsDeviceManager graphics)
        {
            base.LoadContent(theContentManager, graphics, empTextureName, empSpriteSheet, spriteHeightScale, spriteWidthScale);

            base.mSpriteBody.OnCollision += Body_OnCollision;

            base.mSpriteBody.CollisionCategories = Category.Cat10;
            base.mSpriteBody.CollidesWith = Category.All & ~Category.Cat10;//Category.Cat1 & Category.Cat11;

            foreach (Fixture fixturec in base.mSpriteBody.FixtureList)
            {
                fixturec.UserData = USERDATA;

            }
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch);
        }

        public void altFire(Vector2 theStartPosition, float rotation, float shotSpeed)
        {
            base.mposition.Y = (theStartPosition.Y * MeterInPixels) - (float)Math.Round(88f * Math.Sin(rotation + 1.57079633));
            base.mposition.X = (theStartPosition.X * MeterInPixels) - (float)Math.Round(88f * Math.Cos(rotation + 1.57079633));

            base.fire(mposition, rotation, shotSpeed);
        }


        public override void Update(GameTime gameTime, Matrix _view)
        {
            // Set Visible to false here On collision

            if (base.Visible == true)
            {
                if (base.mSpriteBody.LinearVelocity.Y < -20)
                {
                    base.mSpriteBody.LinearVelocity = rotateVector(mDirection, mrotation);
                }
                else
                {
                    base.mSpriteBody.LinearVelocity = rotateVector(mDirection, mrotation);
                }

                base.playAnimation(gameTime, true);

            }
            else
                return;
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


        private Vector2 rotateVector(Vector2 direction, float angle)
        {
            Vector2 newvec = new Vector2();

            newvec.X = (float)((Math.Cos(angle) * direction.X) - (Math.Sin(angle) * direction.Y));
            newvec.Y = (float)((Math.Sin(angle) * direction.X) + (Math.Cos(angle) * direction.Y));

            return newvec;
        }

    }
}
