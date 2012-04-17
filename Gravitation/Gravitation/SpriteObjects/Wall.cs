using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Factories;

namespace Gravitation.SpriteObjects
{
    class Wall : SolidSprite
    {

        public Wall(World world, Vector2 position, float rotation)
        {
            this.mPosition = position;
            this.mWorld = world;
            this.mrotation = rotation;
        }
    }
}
