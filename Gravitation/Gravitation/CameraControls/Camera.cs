using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        protected float mapWidthInPixels;
        protected float mapHeightInPixels;

        //protected const float MAX_ZOOM = 0.1f;
        protected const float MAX_ZOOM = 0.5f;

        //protected float topWallPosY;
        //protected float bottomWallPosY;
        //protected float leftWallPosX;
        //protected float rightWallPosX;

        //camera prisemBounds
        protected Rectangle prizemBase;
        protected Vector3 prizemTip;

        // prisem Line equations y = mx +c
        protected float xRightM;
        protected float xLeftM;
        protected float yTopM;
        protected float yBottomM;

        protected float xRightC;
        protected float xLeftC;
        protected float yTopC;
        protected float yBottomC;


        public Matrix View { get { return _view; } }
        public Vector2 position { get { return _cameraPosition; } }
        public Vector3 zoom { get { return _cameraZoom; } }
        public Vector2 screenCenter { get { return _screenCenter; } }

        public Texture2D basePrisemTex;
        public Texture2D boundTex;


        public Camera()
        {
            _cameraZoom = new Vector3(MAX_ZOOM, MAX_ZOOM, MAX_ZOOM);
        }

        public void initCamera(GraphicsDeviceManager graphics, float leftWallPos, float rightWallPos, float topWallPos, float bottomWallPos)
        {
            // Initialize camera controls
            _view = Matrix.Identity *
                        Matrix.CreateScale(_cameraZoom);
            _cameraPosition = Vector2.Zero;
            _screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f,
                                                graphics.GraphicsDevice.Viewport.Height / 2f);

            screenHeight = graphics.GraphicsDevice.Viewport.Height;
            screenWidth = graphics.GraphicsDevice.Viewport.Width;

            this.mapHeightInPixels = bottomWallPos - topWallPos;
            this.mapWidthInPixels = rightWallPos - leftWallPos;

            float screenIncreaseFactor = 1/zoom.X;
            float acctualScreenWidth = screenWidth * screenIncreaseFactor;
            float acctualScreenHeight = screenHeight * screenIncreaseFactor;

            this.prizemBase = new Rectangle((int)((leftWallPos) + acctualScreenWidth/2),
                                            (int)((topWallPos) + acctualScreenHeight/2), 
                                            (int)(this.mapWidthInPixels - acctualScreenWidth),
                                            (int)(this.mapHeightInPixels - acctualScreenHeight));

            this.prizemTip = new Vector3(prizemBase.Center.X, prizemBase.Center.Y, this.mapWidthInPixels / screenWidth); // max zoom point

            this.initPrisemEdgeGradients();
            this.initYIntercepts();
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

        private void initPrisemEdgeGradients()
        {
            this.xRightM = this.calculateGradient(new Vector2(this.prizemTip.X, this.prizemTip.Z),
                                                (new Vector2(prizemBase.Right, 1/Camera.MAX_ZOOM)));

            this.xLeftM = this.calculateGradient(   new Vector2(this.prizemTip.X,  this.prizemTip.Z),
                                                    new Vector2(prizemBase.Left,  1/Camera.MAX_ZOOM));

            this.yTopM = this.calculateGradient( new Vector2(this.prizemTip.Y,  this.prizemTip.Z),
                                                new Vector2(prizemBase.Top,   1/Camera.MAX_ZOOM));

            this.yBottomM = this.calculateGradient(new Vector2(this.prizemTip.Y, this.prizemTip.Z),
                                                    new Vector2(prizemBase.Bottom, 1/ Camera.MAX_ZOOM));
        }
        private void initYIntercepts()
        {
            this.xRightC = this.calculateXIntercept(this.prizemTip, this.xRightM);
            this.xLeftC = this.calculateXIntercept(this.prizemTip, this.xLeftM);
            this.yTopC = this.calculateYIntercept(this.prizemTip, this.yTopM);
            this.yBottomC = this.calculateYIntercept(this.prizemTip, this.yBottomM);
        }
        private float calculateGradient(Vector2 point1, Vector2 point2)
        {
            return (point1.Y - point2.Y) / (point1.X - point2.X);
        }
        private float calculateYIntercept(Vector3 PointOnline, float gradient)
        {
            return PointOnline.Z - (PointOnline .Y * gradient);
        }

        private float calculateXIntercept(Vector3 PointOnline, float gradient)
        {
            return PointOnline.Z - (PointOnline.X * gradient);
        }

    }
}
