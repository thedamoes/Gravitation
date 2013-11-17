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
    class FireOnSight : Behaviour
    {
        public FireOnSight()
        {
        }

        public override Vector2 getDirection(SpriteObjects.Ship mShip, Body otherShipsBody, float directionWeight)
        {
            return new Vector2(9, 0); ; //hack for now
        }

        public override float getRotation(SpriteObjects.Ship mShip, Body otherShipsBody, float rotationWeight)
        {
            return 99f; //hack for now
        }

        public override void performAction(SpriteObjects.Ship mShip, Body otherShipsBody)
        {

            if (otherShipsBody == null || mShip.mSpriteBody == null)
            {
                return;
            }

            Vector2 aIShipStartPoint = mShip.mSpriteBody.Position + GravitationUtils.rotateVector(new Vector2(0, -0.6f), mShip.mSpriteBody.Rotation); //mine
            Vector2 aIShipEndPoint = aIShipStartPoint + GravitationUtils.rotateVector(new Vector2(0, -5), mShip.mSpriteBody.Rotation);

            Vector2 aIPoint = new Vector2(aIShipStartPoint.Y - aIShipEndPoint.Y, aIShipStartPoint.X - aIShipEndPoint.X);

            Vector2 otherShipStartPoint = aIShipStartPoint;
            Vector2 otherShipEndPoint = otherShipsBody.Position;

            Vector2 otherShipPoint = new Vector2(otherShipStartPoint.Y - otherShipEndPoint.Y, otherShipStartPoint.X - otherShipEndPoint.X);

            float degreeDifference = GravitationUtils.radiansToDegrees(GravitationUtils.getAngleInRadians(aIPoint, otherShipPoint));

            if (degreeDifference > -10 && degreeDifference < 10)
            {
                mShip.fire();
            }
        }
    }
}
