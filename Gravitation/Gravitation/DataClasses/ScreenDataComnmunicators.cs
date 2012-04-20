using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravitation.DataClasses
{
    interface IScreenExitData
    {
        Type getDataClassType();
    }

    class GameConfiguration : IScreenExitData
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
    
        public Type  getDataClassType()
        {
 	        return typeof(GameConfiguration);
        }
    }

    class ShipConfiguration: IScreenExitData
    {
        private SpriteObjects.Ship mShip;

        private SpriteObjects.Ship Ship
        {
            get { return mShip; }
        }

        public ShipConfiguration(SpriteObjects.Ship mShip)
        {
            this.mShip = mShip;
        }

        public Type getDataClassType()
        {
            return typeof(ShipConfiguration);
        }
    }

    class DisplayNewScreen : IScreenExitData
    {
        private Screens.IDrawableScreen mNewScreen;

        public Screens.IDrawableScreen NewScreen
        {
            get
            {
                return mNewScreen;
            }
        }

        public DisplayNewScreen(Screens.IDrawableScreen newScreen)
        {
            mNewScreen = newScreen;
        }

        public Type getDataClassType()
        {
            return typeof(DisplayNewScreen);
        }
    }
}
