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

namespace Gravitation.AIEngine.AIBehaviours
{
    public abstract class Behaviour
    {
        public abstract int getWeight(); // larger weight = more important
        public abstract Vector2 getDirection(SpriteObjects.Ship mShip, Body otherShipsBody, float directionWeight);
        public abstract float getRotation(SpriteObjects.Ship mShip, Body otherShipsBody, float rotationWeight);
        public abstract void performAction(SpriteObjects.Ship mShip, Body otherShipsBody);
    }
}
