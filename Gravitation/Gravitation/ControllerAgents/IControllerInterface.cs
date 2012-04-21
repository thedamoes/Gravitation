﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Gravitation.ControllerAgents
{
    interface IControllerInterface
    {
        void applyMovement();
        void loadShip(ContentManager cm, GraphicsDeviceManager graphics);
        void Draw(SpriteBatch sBatch);
    }
}
