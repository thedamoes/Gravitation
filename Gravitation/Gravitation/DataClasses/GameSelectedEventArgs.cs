using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gravitation.Screens;

namespace Gravitation.DataClasses
{
    public class GameSelectedEventArgs : EventArgs, IGameHolder
    {
        private BaseGame game;
        public GameSelectedEventArgs(BaseGame game)
        {
            this.game = game;
        }

        public BaseGame getGame()
        {
            return this.game;
        }
    }
}
