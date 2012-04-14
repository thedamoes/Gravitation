using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gravitation.ControllerAgents
{
    class LocalAgent : IControllerInterface
    {

        #region members
        private SpriteObjects.Ship mShip;
        private Vector2 mDirection; // X = unless you want weard stuff to happen this should be 0 (could have a powerup that sets this value to non 0)
                                    // Y = how mutch forward
        private float mRotatation = 0;
        private const float DIRECTION_WEIGHT = 50f;
        private const float ROTATION_WEIGHT = 2f;

        public Vector2 myPosition
        {
            get { return mShip.Position; }
        }

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
        }
        public void fire()
        {
            // implement later
        }

        #endregion

        #region IControlInterfaceMembers

        public void applyMovement()
        {
            if (mDirection == Vector2.Zero && mRotatation == 0 ) // optimisation
                return;

            
            float shipAngle;

            mShip.mSpriteBody.ApplyTorque(mRotatation);
            shipAngle = mShip.mSpriteBody.Rotation;

            mShip.mSpriteBody.ApplyForce(rotateVector(mDirection,shipAngle));

            resetParams();
        }
        public void loadShip(ContentManager cm)
        {
            mShip.LoadContent(cm ,"floor");
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
