using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gravitation.CameraControls
{
    public class TwoPlayerCamera : Camera
    {

        // overload updateCamera function for 2 players
        public void updateCamera(Vector2 shipPos, Vector2 shipPos2)
        {
            float screenIncreaseFactor = 1 / base.zoom.X;
            float acctualScreenWidth = base.screenWidth * screenIncreaseFactor;
            float acctualScreenHeight = base.screenHeight * screenIncreaseFactor;

            int smallestScreenDimention = 0;
            if (base.screenHeight > base.screenWidth)
                smallestScreenDimention = base.screenWidth;
            else
                smallestScreenDimention = base.screenHeight;

            Vector2 player1PosInPixles = new Vector2((shipPos.X * Screens.BaseGame.MeterInPixels),
                                                    (shipPos.Y * Screens.BaseGame.MeterInPixels));
            Vector2 player2PosInPixles = new Vector2((shipPos2.X * Screens.BaseGame.MeterInPixels),
                                                    (shipPos2.Y * Screens.BaseGame.MeterInPixels));

            Vector2 centerOfPlayers = findCenter(player1PosInPixles, player2PosInPixles);
            float camY = centerOfPlayers.Y;
            float camX = centerOfPlayers.X;
            this.applyZoom(player1PosInPixles, player2PosInPixles, smallestScreenDimention, screenIncreaseFactor);

            base.correctCameraPositionX(ref camX,base.zoom.X);
            base.correctCameraPositionY(ref camY,base.zoom.X);

            base._cameraPosition.X = -(camX - (acctualScreenWidth / 2));
            base._cameraPosition.Y = -(camY - (acctualScreenHeight / 2));

            _view = Matrix.CreateTranslation(new Vector3(_cameraPosition, 0f)) *
                 Matrix.CreateScale(_cameraZoom);
        }

       

        private Vector2 findCenter(Vector2 p1, Vector2 p2)
        {
            return new Vector2((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }
        private void applyZoom(Vector2 player1Pos, Vector2 player2Pos, int smallestScreenDimention, float screenIncreaseFactor)
        {
            Vector2 distanceBetweenShips = player1Pos - player2Pos;
            if (distanceBetweenShips.X < 0) distanceBetweenShips.X *= -1; // take positive distances
            if (distanceBetweenShips.Y < 0) distanceBetweenShips.Y *= -1; // take positive distances

            float distanceAppartasPercent = (distanceBetweenShips.X + distanceBetweenShips.Y) / (smallestScreenDimention * screenIncreaseFactor);
            if (distanceAppartasPercent > 0.90)
            {
                if (1/_cameraZoom.X < base.prizemTip.Z)
                    _cameraZoom -= _cameraZoom * new Vector3(0.005f, 0.005f, 0);
            }
            if (distanceAppartasPercent < 0.50)
            {
                if(_cameraZoom.X < Camera.MAX_ZOOM)
                    _cameraZoom += _cameraZoom * new Vector3(0.005f, 0.005f, 0);
            }
        }
    }
}
