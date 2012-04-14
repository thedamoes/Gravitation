using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gravitation.ControllerAgents
{
    interface IControllerInterface
    {
        void applyMovement();
        void loadShip(ContentManager cm);
        void Draw(SpriteBatch sBatch);
    }
}
