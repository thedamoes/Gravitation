using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using DPSF;
using DPSF.ParticleSystems;

namespace Gravitation.ControllerAgents
{
    public class LocalAgent : IControllerInterface
    {

        #region members
        public SpriteObjects.Ship mShip;
        private Vector2 mDirection; // X = unless you want weard stuff to happen this should be 0 (could have a powerup that sets this value to non 0)
                                    // Y = how mutch forward
        private float mRotatation = 0;
        private const float DIRECTION_WEIGHT = 3.5f; //2.5
        private const float ROTATION_WEIGHT = 0.5f;

        public Vector2 myPosition
        {
            get { return mShip.ShipPosition; }
        }
        public float Rotation
        {
            get { return this.mRotatation; }
        }




        private ContentManager cm;

        #endregion

        public LocalAgent(SpriteObjects.Ship ship)
        {
            this.mShip = ship;
            this.mDirection = new Vector2(0, 0);
        }

        #region playerInputHandlers

        public void moveRight()
        {
            mRotatation += ROTATION_WEIGHT;
        }
        public void moveLeft()
        {
            mRotatation -= ROTATION_WEIGHT;
        }
        public void moveBack()
        {
            mDirection.Y += DIRECTION_WEIGHT;
        }
        public void moveForward()
        {
            mDirection.Y -= DIRECTION_WEIGHT;
            mShip.mShipParticles.Emitter.BurstParticles = 1000;
        }

        #region analogHandlers
        public void moveRight(float amount)
        {
            if(amount < 0)
            mRotatation += amount*ROTATION_WEIGHT;
        }
        public void moveLeft(float amount)
        {
            if(amount > 0)
                mRotatation -= -1 * amount * ROTATION_WEIGHT;
        }
        public void moveBack(float amount)
        {
            mDirection.Y += amount*DIRECTION_WEIGHT;
        }
        public void moveForward(float amount)
        {
            mDirection.Y -= amount*DIRECTION_WEIGHT;
            mShip.mShipParticles.Emitter.BurstParticles = 1000;
        }
        #endregion
        public void fire()
        {

            mShip.fire();

        }
        public void stall()
        {
            mShip.mSpriteBody.AngularVelocity = 0;
            mRotatation = 0;
        }


        public void reset()
        {
            mShip.mSpriteBody.Position = new Vector2(4.25f, 3.75f);
            mShip.mSpriteBody.Rotation = 0;
            mShip.mSpriteBody.AngularVelocity = 0.0f;
            mShip.mSpriteBody.LinearVelocity = new Vector2(0, 0);

            
        }
        public void reset2(Vector2 spawnpoint)
        {
            mShip.mSpriteBody.Position = (spawnpoint*0.015625f); //FIGURE OUT WHY THIS IS!!
            mShip.mSpriteBody.Rotation = 0;
            mShip.mSpriteBody.AngularVelocity = 0.0f;
            mShip.mSpriteBody.LinearVelocity = new Vector2(0, 0);
            
        }
        
        

        #endregion

        #region IControlInterfaceMembers

        public void applyMovement()
        {
            if (mDirection == Vector2.Zero && mRotatation == 0) // optimisation
            {
                return;
            }
            
            float shipAngle;


            //following if satement is to limit the rotation velocity so we don't spin LIGHT SPPPEEEED
            if (mShip.mSpriteBody.AngularVelocity > 5)
            {
                mShip.mSpriteBody.AngularVelocity = 5;
                //do nothing
            }
            else if (mShip.mSpriteBody.AngularVelocity < -5)
            {
                mShip.mSpriteBody.AngularVelocity = -5;
                //do nothing
            }
            else
            {
                mShip.mSpriteBody.ApplyTorque(mRotatation);
            }

            shipAngle = mShip.mSpriteBody.Rotation;


            float Yvel = mShip.mSpriteBody.LinearVelocity.Y;
            float Xvel = mShip.mSpriteBody.LinearVelocity.X;

            int posSpd = (int)Math.Max(Math.Sqrt(Yvel * Yvel), Math.Sqrt(Xvel * Xvel));

            int totalSpeed = posSpd;
            int speedLimit = 50;//10

            float excessX = 0;
            float excessY = 0;

            if (Xvel > speedLimit)
            {
                excessX = Xvel - speedLimit;
            }
            else if (Xvel < -speedLimit)
            {
                excessX = Xvel + speedLimit;
            }

            if (Yvel > speedLimit)
            {
                excessY = Yvel - speedLimit;
            }
            else if (Yvel < -speedLimit)
            {
                excessY = Yvel + speedLimit;
            }

            if (totalSpeed > speedLimit) //15
            {
                mShip.mSpriteBody.ApplyLinearImpulse(new Vector2(-excessX,-excessY));
            }
            else
            {
                mShip.mSpriteBody.ApplyForce(rotateVector(mDirection,shipAngle));
            }
            resetParams();
        }


        public void updateShip(GameTime gameTime, Matrix _view)
        {
            mShip.updateShot(gameTime, _view);
            mShip.thrust(gameTime, _view);
            mShip.updatePassiveShipState(gameTime, _view);
        }

        public void loadShip(ContentManager cm, GraphicsDeviceManager graphics)
        {
            this.cm = cm;

            mShip.LoadContent(cm ,"Ship", graphics);

        }
        public void Draw(SpriteBatch sBatch)
        {
            mShip.Draw(sBatch);

        }

        #endregion


        private Vector2 rotateVector(Vector2 direction, float angle)
        {
            Vector2 newvec = new Vector2();

            newvec.X = (float)((Math.Cos(angle) * direction.X) - (Math.Sin(angle) * direction.Y));
            newvec.Y = (float)((Math.Sin(angle) * direction.X) + (Math.Cos(angle) * direction.Y));

            return newvec;
        }
        private void resetParams()
        {
            mDirection = Vector2.Zero;
            mRotatation = 0f;
        }
    }
}
