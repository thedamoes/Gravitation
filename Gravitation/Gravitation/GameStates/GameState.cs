using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Gravitation.GameStates
{
    public class shipState
    {
        public Vector2 position;
        public float rotation;

        public shipState() { }

    }
    public class GameState
    {
        public shipState playerstate = new shipState();

        public GameState() { }
    }
}
