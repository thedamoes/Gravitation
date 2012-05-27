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
        private SpriteObjects.Ship mShip2;

        public String MapName
        {
            get { return mMapName; }
        }

        public SpriteObjects.Ship Ship
        {
            get { return mShip; }
        }

        public SpriteObjects.Ship Ship2
        {
            get { return mShip2; }
        }

        public GameConfiguration(
                                String mapName,
                                SpriteObjects.Ship playerShip,
                                SpriteObjects.Ship player2Ship)
        {
            this.mShip = playerShip;
            this.mShip2 = player2Ship;

            this.mMapName = mapName;
        }
    
        public Type  getDataClassType()
        {
 	        return typeof(GameConfiguration);
        }
    }

    public class ShipConfiguration: IScreenExitData
    {
        private SpriteObjects.Ship mShip;

        public SpriteObjects.Ship Ship
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
