using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravitation.DataClasses
{
    class GameConfiguration
    {
        private String mMapName;
        private SpriteObjects.Ship mShip; // deff need to add more later

        public String MapName
        {
            get { return mMapName; }
        }

        public SpriteObjects.Ship Ship
        {
            get { return mShip; }
        }

        public GameConfiguration(
                                String mapName,
                                SpriteObjects.Ship playerShip)
        {
            this.mShip = playerShip;
            this.mMapName = mapName;
        }
    }
}
