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
    class Obstical : SolidSprite
    {

        public Obstical(World world, Vector2 position, Vector2 scale, float rot)
        {
            base.mPosition = position;
            base.mWorld = world;
            base.mrotation = rot;
            
        }
    }
}
