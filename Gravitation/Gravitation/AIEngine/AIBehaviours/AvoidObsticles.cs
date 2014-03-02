using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.DebugViews;
using FarseerPhysics.Dynamics;
using Gravitation.Utils;

namespace Gravitation.AIEngine.AIBehaviours
{
    class AvoidObsticles : Behaviour
    {

        private int behaviourWeight = 9;

        public AvoidObsticles()
        {
        }

        public override Vector2 getDirection(SpriteObjects.Ship mShip, Body otherShipsBody, float directionWeight)
        {
            float rearRayLength = mShip.getRayCastLengths.ElementAt(4); //length of raycast at the back of the ship
            float frontRayLength = mShip.getRayCastLengths.ElementAt(0); //length of raycast at the front of the ship
            Vector2 thrust = new Vector2(0, 99);

            //if (otherShipsBody == null || mShip.mSpriteBody == null)
            //{
            //    thrust = thrust;
            //}
            //else if (otherShipsBody.Position.Equals(mShip.mSpriteBody.Position))
            //{
            //    thrust = Vector2.Zero;
            //}


            if (rearRayLength < (-SpriteObjects.Ship.BASE_SHIP_RAYCAST_LENGTH))
            {
                thrust = new Vector2(0, -directionWeight);
            }
            //else
            //{
            //    thrust = Vector2.Zero;
            //}
            if ((frontRayLength < (-SpriteObjects.Ship.BASE_SHIP_RAYCAST_LENGTH))) 
            {
                thrust = Vector2.Zero;
            }

            return thrust;
        }

        public override float getRotation(SpriteObjects.Ship mShip, Body otherShipsBody, float rotationWeight)
        {
            float rotationChange = 99f;
            int leftVote = 0;
            int rightVote = 0;
            List<float> rayLengths = mShip.getRayCastLengths;
            List<float> rightRayLengths = rayLengths.GetRange(1, 3);
            List<float> leftRayLengths = rayLengths.GetRange(5, 3);

            foreach (float rayLength in rightRayLengths)
            {
                if(rayLength < (-SpriteObjects.Ship.BASE_SHIP_RAYCAST_LENGTH)) {
                    leftVote++;
                }
            }

            foreach (float rayLength in leftRayLengths)
            {
                if(rayLength < (-SpriteObjects.Ship.BASE_SHIP_RAYCAST_LENGTH)) {
                    rightVote++;
                }
            }

            if (rightVote > leftVote)
            {
                rotationChange = rotationWeight;
            }
            else if (leftVote > rightVote)
            {
                rotationChange = -rotationWeight;
            }
            else
            {
                rotationChange = 99f;
            }

            return rotationChange;
        }

        public override int getWeight()
        {
            return this.behaviourWeight;
        }

        public override void performAction(SpriteObjects.Ship mShip, Body otherShipsBody)
        {
            //no action to perform
        }
    }
}
