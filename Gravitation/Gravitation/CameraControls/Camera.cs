using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Gravitation.CameraControls
{
    class Camera
    {
        protected Matrix _view;
        protected Vector2 _cameraPosition;
        protected Vector2 _screenCenter;
        protected Vector3 _cameraZoom;

        protected int screenHeight;
        protected int screenWidth;

        public Matrix View { get { return _view; } }
        public Vector2 position { get { return _cameraPosition; } }
        public Vector3 zoom { get { return _cameraZoom; } }
        public Vector2 screenCenter { get { return _screenCenter; } }


        public Camera()
        {
            _cameraZoom = new Vector3(0.5f, 0.5f, 0.5f);
        }

        public void initCamera(GraphicsDeviceManager graphics)
        {
            // Initialize camera controls
            _view = Matrix.Identity *
                        Matrix.CreateScale(_cameraZoom);
            _cameraPosition = Vector2.Zero;
            _screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f,
                                                graphics.GraphicsDevice.Viewport.Height / 2f);

            screenHeight = graphics.PreferredBackBufferHeight;
            screenWidth = graphics.PreferredBackBufferWidth;
        }

        public void updateCamera(Vector2 shipPos)
        {
            //maintain camera position
            float playerPosInPixlesX = (-shipPos.X * Screens.BaseGame.MeterInPixels);  // these here variables
            float playerPosInPixlesY = (-shipPos.Y * Screens.BaseGame.MeterInPixels);  // are what the "zoom"
            float camX = (playerPosInPixlesX + this.screenWidth);  // offsets the camera by
            float camY = (playerPosInPixlesY + this.screenHeight); // from it's origin

            // couldent think of any other better way to do it?? if u can feel free
            // these lines stop the cammera from moveing past the bounds of the map


            //if (-playerPosInPixlesX < (graphics.PreferredBackBufferWidth - mMapLoader.leftWallPosX)) _cameraPosition.X = (mMapLoader.leftWallPosX);
            //else if (-playerPosInPixlesX > (mMapLoader.rightWallPosX - graphics.PreferredBackBufferWidth)) _cameraPosition.X = -(mMapLoader.rightWallPosX - (graphics.PreferredBackBufferWidth) * 2);
            //else _cameraPosition.X = camX;


            _cameraPosition.X = camX;

            //if (-playerPosInPixlesY < (graphics.PreferredBackBufferHeight + mMapLoader.topWallPosY)) _cameraPosition.Y = (-mMapLoader.topWallPosY);
            //else if (-playerPosInPixlesY > (mMapLoader.bottonWallPosY - graphics.PreferredBackBufferHeight)) _cameraPosition.Y = (graphics.PreferredBackBufferHeight * 2) - mMapLoader.bottonWallPosY;
            //else _cameraPosition.Y = camY;

            _cameraPosition.Y = camY;

            _view = Matrix.CreateTranslation(new Vector3(_cameraPosition - _screenCenter, 0f)) *
                Matrix.CreateTranslation(new Vector3(_screenCenter, 0f)) *
                 Matrix.CreateScale(_cameraZoom);
        }

    }
}
