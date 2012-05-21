using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.DebugViews;
using FarseerPhysics;

using DPSF;
using DPSF.ParticleSystems;



namespace Gravitation.Screens
{
    class GameScreen : BaseGame
    {
        // Simple camera controls
        private Matrix _view;
        private Vector2 _cameraPosition;
        private Vector2 _screenCenter;
        private Vector3 _cameraZoom;
        
        // game content
        
        private ControllerAgents.LocalAgent mPlayer1;

        public GameScreen(DataClasses.GameConfiguration gameConfig): base(gameConfig)
        {
            SpriteObjects.Ship ship = gameConfig.Ship;
            ship.ShipPosition = mMapLoader.shipStartPosP1;
            ship.World = base.mWorld;

            mPlayer1 = new ControllerAgents.LocalAgent(ship);
            _cameraZoom = new Vector3(0.5f, 0.5f, 0.5f);
        }

        public override void LoadContent(GraphicsDeviceManager graphics, ContentManager Content)
        {
            // Initialize camera controls
            _view = Matrix.Identity *
                        Matrix.CreateScale(_cameraZoom);
            _cameraPosition = Vector2.Zero;
            _screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f,
                                                graphics.GraphicsDevice.Viewport.Height / 2f);
            

            // load players
            mPlayer1.loadShip(Content, graphics);

            base.LoadContent(graphics, Content);
        }

        public override DataClasses.IScreenExitData Update(GameTime gameTime)
        {
            //maintain camera position
            float playerPosInPixlesX = (-mPlayer1.myPosition.X * BaseGame.MeterInPixels);  // these here variables
            float playerPosInPixlesY = (-mPlayer1.myPosition.Y * BaseGame.MeterInPixels);  // are what the "zoom"
            float camX = (playerPosInPixlesX + base.graphics.PreferredBackBufferWidth);  // offsets the camera by
            float camY = (playerPosInPixlesY + base.graphics.PreferredBackBufferHeight); // from it's origin

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

            //update Controlling agients
            mPlayer1.applyMovement();

            mPlayer1.updateShot(gameTime, _view);

            foreach (SpriteObjects.Shot aShot in mPlayer1.mShip.remove_Shots)
            {
                if (aShot != null && aShot.Visible == false && aShot.removed == false)
                {
                    mPlayer1.mShip.shortRomoved();
                }
            }

            for (int i = 0; i < mPlayer1.mShip.remove_Shots.Count; i++)
            {
                if (mPlayer1.mShip.remove_Shots.ElementAt(i).removed == true)
                {
                    mPlayer1.mShip.remove_Shots.RemoveAt(i);
                }
            }
            return base.Update(gameTime);
        }


        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, GameTime gameTime)
        {
            mPlayer1.Draw(sb);
#if DEBUG


            // calculate the projection and view adjustments for the debug view
            Matrix projection = Matrix.CreateOrthographicOffCenter(0f, graphics.GraphicsDevice.Viewport.Width / MeterInPixels,
                                                             graphics.GraphicsDevice.Viewport.Height / MeterInPixels, 0f, 0f,
                                                             1f);
            Matrix view = Matrix.CreateTranslation(new Vector3((_cameraPosition / MeterInPixels) - (_screenCenter / MeterInPixels), 0f)) *
                                                Matrix.CreateTranslation(new Vector3((_screenCenter / MeterInPixels), 0f)) *
                                                 Matrix.CreateScale(_cameraZoom);


            debugView.RenderDebugData(ref projection, ref view);

#endif
            base.Draw(sb, gameTime);
        }

        public override void HandleKeyboard(KeyboardState state, KeyboardState prevState)
        {
            // We make it possible to rotate the circle body
            if (state.IsKeyDown(Keys.A))
                mPlayer1.moveLeft();

            if (state.IsKeyDown(Keys.D))
                mPlayer1.moveRight();

            if (state.IsKeyDown(Keys.W))
                mPlayer1.moveForward();

            if (state.IsKeyUp(Keys.D) && prevState.IsKeyDown(Keys.D))
                mPlayer1.stall();

            if (state.IsKeyUp(Keys.A) && prevState.IsKeyDown(Keys.A))
                mPlayer1.stall();

            if (state.IsKeyUp(Keys.F) && prevState.IsKeyDown(Keys.F))
                mPlayer1.fire();

            if (state.IsKeyDown(Keys.Space) && prevState.IsKeyUp(Keys.Space))
                mPlayer1.reset();

            base.HandleKeyboard(state,prevState);

        }

        public override Matrix getView()
        {
            return _view;
        }

    }
}
