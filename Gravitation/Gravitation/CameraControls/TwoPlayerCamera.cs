using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Gravitation.CameraControls
{
    class TwoPlayerCamera : Camera
    {
        // overload updateCamera function for 2 players
        public void  updateCamera(Vector2 shipPos, Vector2 shipPos2) 
        {
            float screenIncreaseFactor = 1/base.zoom.X;
            int smallestScreenDimention = 0;
            if(base.screenHeight > base.screenWidth)
                smallestScreenDimention = base.screenWidth;
            else
                smallestScreenDimention = base.screenHeight;
            
            Vector2 player1PosInPixles = new Vector2((-shipPos.X * Screens.BaseGame.MeterInPixels),
                                                    (-shipPos.Y * Screens.BaseGame.MeterInPixels));
            Vector2 player2PosInPixles = new Vector2((-shipPos2.X * Screens.BaseGame.MeterInPixels),
                                                    (-shipPos2.Y * Screens.BaseGame.MeterInPixels));

            Vector2 distanceBetweenShips = player1PosInPixles - player2PosInPixles;
            if (distanceBetweenShips.X < 0) distanceBetweenShips.X *= -1; // take positive distances
            if (distanceBetweenShips.Y < 0) distanceBetweenShips.Y *= -1; // take positive distances

            // if the distance between the ships is > %70 of the screen
            if (distanceBetweenShips.X + distanceBetweenShips.Y
                > ((smallestScreenDimention * screenIncreaseFactor) * 0.70))
                _cameraZoom -= new Vector3(0.001f, 0.001f, 0.001f);

            else if (distanceBetweenShips.X + distanceBetweenShips.Y
               < ((smallestScreenDimention * screenIncreaseFactor) * 0.40))
                _cameraZoom += new Vector3(0.001f, 0.001f, 0.001f);

            Vector2 centerOfPlayers = findCenter(player1PosInPixles, player2PosInPixles);
            float camX = (centerOfPlayers.X + base.screenWidth);
            float camY = (centerOfPlayers.Y + base.screenHeight);

            _cameraPosition.X = camX;
            _cameraPosition.Y = camY;

            _view = Matrix.CreateTranslation(new Vector3(_cameraPosition - _screenCenter, 0f)) *
                Matrix.CreateTranslation(new Vector3(_screenCenter, 0f)) *
                 Matrix.CreateScale(_cameraZoom);
        }

        private Vector2 findCenter(Vector2 p1, Vector2 p2)
        {
            return new Vector2((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }
    }
}
