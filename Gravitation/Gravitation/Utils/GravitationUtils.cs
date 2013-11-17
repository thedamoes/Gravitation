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

namespace Gravitation.Utils
{
    /**
     * Basic Utils class to contain commonly 
     * used functions/Variables.
     * (more to be added/moved into this class as dev continues)
     */ 
    class GravitationUtils
    {

        public static Vector2 rotateVector(Vector2 direction, float angle)
        {
            Vector2 newvec = new Vector2();

            newvec.X = (float)((Math.Cos(angle) * direction.X) - (Math.Sin(angle) * direction.Y));
            newvec.Y = (float)((Math.Sin(angle) * direction.X) + (Math.Cos(angle) * direction.Y));

            return newvec;
        }

        public static float getAngleInRadians(Vector2 a, Vector2 b)
        {

            double angle1 = Math.Atan2(a.Y, a.X);

            double angle2 = Math.Atan2(b.Y, b.X);

            return (float)(angle1 - angle2);

        }

        public static float radiansToDegrees(float radians)
        {
            float degreeDifference = (float)(radians * (180 / Math.PI));
            if (degreeDifference > 180)
            {
                degreeDifference = -(360 - degreeDifference);
            }
            else if (degreeDifference < -180)
            {
                degreeDifference = (360 + degreeDifference);
            }

            return degreeDifference;
        }


    }
}
