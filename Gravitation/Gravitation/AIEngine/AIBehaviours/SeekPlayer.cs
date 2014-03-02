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
    class SeekPlayer : Behaviour
    {
        private int behaviourWeight = 7;
            
        public override Vector2 getDirection(SpriteObjects.Ship mShip, Body otherShipsBody, float directionWeight)
        {
            Vector2 thrust = Vector2.Zero;

            if (otherShipsBody == null || mShip.mSpriteBody == null)
            {
                thrust = Vector2.Zero;
            }
            else if (otherShipsBody.Position.Equals(mShip.mSpriteBody.Position))
            {
                thrust = Vector2.Zero;
            }
            else
            {
                Vector2 startPoint = mShip.mSpriteBody.Position + GravitationUtils.rotateVector(new Vector2(0, -0.6f), mShip.mSpriteBody.Rotation);
                Vector2 otherShipEndPoint = otherShipsBody.Position;
                double xPoints = (otherShipEndPoint.X - startPoint.X) * (otherShipEndPoint.X - startPoint.X);
                double yPoints = (otherShipEndPoint.Y - startPoint.Y) * (otherShipEndPoint.Y - startPoint.Y);
                float totalLength = (float)Math.Sqrt((xPoints + yPoints));

                //Console.WriteLine("total line length is [" + totalLength + "]");
                if (totalLength > 5)
                {
                    thrust = new Vector2(0, -directionWeight);
                }
                else
                {
                    thrust = Vector2.Zero;
                }

            }
            return thrust;
        }

        public override float getRotation(SpriteObjects.Ship mShip, Body otherShipsBody, float rotationWeight)
        {
            float rotationChange = 99f;

            if (otherShipsBody == null || mShip.mSpriteBody == null)
            {
                return rotationChange;
            }

            Vector2 aIShipStartPoint = mShip.mSpriteBody.Position + GravitationUtils.rotateVector(new Vector2(0, -0.6f), mShip.mSpriteBody.Rotation); //mine
            Vector2 aIShipEndPoint = aIShipStartPoint + GravitationUtils.rotateVector(new Vector2(0, -5), mShip.mSpriteBody.Rotation);

            Vector2 aIPoint = new Vector2(aIShipStartPoint.Y - aIShipEndPoint.Y, aIShipStartPoint.X - aIShipEndPoint.X);

            Vector2 otherShipStartPoint = aIShipStartPoint;
            Vector2 otherShipEndPoint = otherShipsBody.Position;

            Vector2 otherShipPoint = new Vector2(otherShipStartPoint.Y - otherShipEndPoint.Y, otherShipStartPoint.X - otherShipEndPoint.X);

            float degreeDifference = GravitationUtils.radiansToDegrees(GravitationUtils.getAngleInRadians(aIPoint, otherShipPoint));

            if(degreeDifference > -10 && degreeDifference < 10) {
                rotationChange = 0;
            } else {
                rotationChange = (degreeDifference > 0) ? rotationWeight : -rotationWeight;
            }

            //Console.WriteLine("Rotation change is ["+rotationChange+"]  current ");

            //Console.WriteLine("rotational difference between ai and player is [" + radiansToDegrees(getAngleInRadians(aIPoint, otherShipPoint)) + "]");

            return rotationChange;
        }

        public override void performAction(SpriteObjects.Ship mShip, Body otherShipsBody)
        {
            //no action to take.
        }

        public override int getWeight()
        {
            return this.behaviourWeight;
        }
    }
}
