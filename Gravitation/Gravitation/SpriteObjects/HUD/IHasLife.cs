using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravitation.SpriteObjects.HUD
{
    public interface IHasLife
    {
        int SheildValue
        {
            get;
        }

        int HealthValue
        {
            get;
        }
    }
}
