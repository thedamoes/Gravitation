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
            float acctualScreenWidth = base.screenWidth * screenIncreaseFactor;
            float acctualScreenHeight = base.screenHeight * screenIncreaseFactor;

            int smallestScreenDimention = 0;
            if(base.screenHeight > base.screenWidth)
                smallestScreenDimention = base.screenWidth;
            else
                smallestScreenDimention = base.screenHeight;
            
            Vector2 player1PosInPixles = new Vector2((shipPos.X * Screens.BaseGame.MeterInPixels),
                                                    (shipPos.Y * Screens.BaseGame.MeterInPixels));
            Vector2 player2PosInPixles = new Vector2((shipPos2.X * Screens.BaseGame.MeterInPixels),
                                                    (shipPos2.Y * Screens.BaseGame.MeterInPixels));

           

            float camX = _cameraPosition.X;
            float camY = _cameraPosition.Y;
            Vector2 centerOfPlayers = findCenter(player1PosInPixles, player2PosInPixles);
            camY = ((acctualScreenHeight / 2) - centerOfPlayers.Y);
            camX = ((acctualScreenWidth / 2) - centerOfPlayers.X);

            bool camHitLeftWall = this.viewHitLeftWall(base.leftWallPosX,
                                                        centerOfPlayers,
                                                        acctualScreenWidth);

            bool camHitRightWall = this.viewHitRightWall(base.rightWallPosX,
                                            centerOfPlayers,
                                            acctualScreenWidth);

            bool camHitBottemWall = this.viewHitBottonWall(base.bottomWallPosY,
                                                           centerOfPlayers,
                                                           acctualScreenHeight);

            bool camHitTopWall = this.viewHitTopWall(base.topWallPosY,
                                                        centerOfPlayers,
                                                        acctualScreenHeight);


            if (camHitLeftWall || camHitRightWall)
            {
                if (camHitLeftWall && camHitRightWall)
                {
                    camX = ((acctualScreenWidth / 2) - ((base.rightWallPosX + base.leftWallPosX)/2)); // center x of map
                }
                else if (camHitRightWall)
                {
                    camX = ((acctualScreenWidth / 2) - centerOfPlayers.X) - (base.rightWallPosX - ((centerOfPlayers.X) + (acctualScreenWidth / 2)));
                    this.applyZoom(player1PosInPixles, player2PosInPixles, smallestScreenDimention, screenIncreaseFactor);
                    Console.WriteLine("hitRight");
                }
                else if (camHitLeftWall)
                {
                    camX = ((acctualScreenWidth / 2) - centerOfPlayers.X) + ((centerOfPlayers.X) - (acctualScreenWidth / 2) - base.leftWallPosX);
                    this.applyZoom(player1PosInPixles, player2PosInPixles, smallestScreenDimention, screenIncreaseFactor);
                    Console.WriteLine("hitLeft");
                }
            }
            if (camHitBottemWall || camHitTopWall)
            {
                if (camHitBottemWall && camHitTopWall)
                {
                    camY = ((acctualScreenHeight / 2) - ((base.bottomWallPosY + base.topWallPosY) / 2)); // center y of map
                }
                else if (camHitBottemWall)
                {
                    camY = ((acctualScreenHeight / 2) - centerOfPlayers.Y) - (base.bottomWallPosY - ((centerOfPlayers.Y) + (acctualScreenHeight / 2)));
                    this.applyZoom(player1PosInPixles, player2PosInPixles, smallestScreenDimention, screenIncreaseFactor);
                    Console.WriteLine("hitBotton");
                }
                else if (camHitTopWall)
                {
                    camY = ((acctualScreenHeight / 2) - centerOfPlayers.Y) + (((centerOfPlayers.Y) - (acctualScreenHeight / 2)) - base.topWallPosY);
                    this.applyZoom(player1PosInPixles, player2PosInPixles, smallestScreenDimention, screenIncreaseFactor);
                    Console.WriteLine("hitTop");
                }
            }
            else
            {
                this.applyZoom(player1PosInPixles, player2PosInPixles, smallestScreenDimention, screenIncreaseFactor);
            }

            _cameraPosition.X = camX;
            _cameraPosition.Y = camY;

            _view = Matrix.CreateTranslation(new Vector3(_cameraPosition , 0f)) *
                 Matrix.CreateScale(_cameraZoom);
        }

        private Vector2 findCenter(Vector2 p1, Vector2 p2)
        {
            return new Vector2((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }

        private bool viewHitLeftWall(float leftWallPos, Vector2 CamCenter, float camWidth)
        {
            if ((CamCenter.X) - (camWidth/2) <= leftWallPos)
                return true;
            else
                return false;
        }

        private bool viewHitRightWall(float rightWallPos, Vector2 CamCenter, float camWidth)
        {
            if ((CamCenter.X) + (camWidth / 2) >= rightWallPos)
                return true;
            else
                return false;
        }

        private bool viewHitBottonWall(float BottonWallPos, Vector2 CamCenter, float camHeight)
        {
            if ((CamCenter.Y) + (camHeight / 2) >= BottonWallPos)
                return true;
            else
                return false;
        }

        private bool viewHitTopWall(float TopWallPos, Vector2 CamCenter, float camHeight)
        {
            if ((CamCenter.Y) - (camHeight / 2) <= TopWallPos)
                return true;
            else
                return false;
        }

        private void applyZoom(Vector2 player1Pos, Vector2 player2Pos, int smallestScreenDimention, float screenIncreaseFactor)
        {
            Vector2 distanceBetweenShips = player1Pos - player2Pos;
            if (distanceBetweenShips.X < 0) distanceBetweenShips.X *= -1; // take positive distances
            if (distanceBetweenShips.Y < 0) distanceBetweenShips.Y *= -1; // take positive distances

            float distanceAppartasPercent = (distanceBetweenShips.X + distanceBetweenShips.Y) / (smallestScreenDimention * screenIncreaseFactor);
            if (distanceAppartasPercent > 0.90)
                _cameraZoom -= _cameraZoom * new Vector3(0.005f, 0.005f, 0);
            if (distanceAppartasPercent < 0.50)
                _cameraZoom += _cameraZoom * new Vector3(0.005f, 0.005f, 0);
        }
    }
}
