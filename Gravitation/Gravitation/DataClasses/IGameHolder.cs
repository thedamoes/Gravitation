using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gravitation.Screens;

namespace Gravitation.DataClasses
{
    interface IGameHolder
    {
        BaseGame getGame();
    }
}
