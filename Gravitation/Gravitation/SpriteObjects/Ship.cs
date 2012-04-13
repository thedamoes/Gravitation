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

namespace Gravitation.SpriteObjects
{
    class Ship : Sprite
    {
        public Ship(World world, Vector2 position)
        {
            base.AssetName = "floor";
            base.mSpriteBody = BodyFactory.CreateRectangle(world, 96f / Sprite.MeterInPixels, 64f / Sprite.MeterInPixels, 1f, position);
            base.mSpriteBody.BodyType = BodyType.Dynamic;
            base.mSpriteBody.Restitution = 0.3f;
            base.mSpriteBody.Friction = 0.1f;
            base.mSpriteBody.IsStatic = false;
        }
    }
}
