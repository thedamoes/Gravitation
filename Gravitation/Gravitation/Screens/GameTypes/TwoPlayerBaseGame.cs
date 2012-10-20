﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Gravitation.Screens.GameTypes
{
    public class TwoPlayerBaseGame : BaseGame
    {
        protected ControllerAgents.LocalAgent mPlayer1;
        protected ControllerAgents.LocalAgent mPlayer2;
        protected Input.ControlConfig mPlayer1ControllerConfig;
        protected Input.ControlConfig mPlayer2ControllerConfig;

        protected CameraControls.TwoPlayerCamera cam;

        public TwoPlayerBaseGame(DataClasses.GameConfiguration gameConfig)
            : base(gameConfig)
        {
            // load ship 1
            SpriteObjects.Ship ship = gameConfig.Ship;
            ship.ShipPosition = mMapLoader.shipStartPosP1;
            ship.World = base.mWorld;

            // load ship 2
            SpriteObjects.Ship ship2 = gameConfig.Ship2;
            ship2.ShipPosition = mMapLoader.shipStartPosP2;
            ship2.World = base.mWorld;

            mPlayer1 = new ControllerAgents.LocalAgent(ship);
            mPlayer2 = new ControllerAgents.LocalAgent(ship2);

            cam = new CameraControls.TwoPlayerCamera();
        }

        public override void Update(GameTime gameTime)
        {

            cam.updateCamera(mPlayer1.myPosition, mPlayer2.myPosition);
            //update Controlling agients

            mPlayer1.thrust(gameTime, cam.View);
            mPlayer2.thrust(gameTime, cam.View);

            mPlayer1.applyMovement();
            mPlayer2.applyMovement();

            mPlayer1.updateShot(gameTime, cam.View);
            mPlayer2.updateShot(gameTime, cam.View);


            if (mPlayer1.mShip.sheilds <= 0)
            {
                mPlayer1.mShip.sheilds = 100;  //DEATH
                mPlayer1.reset2(mMapLoader.shipStartPosP1);
            }

            if (mPlayer2.mShip.sheilds <= 0)
            {
                mPlayer2.mShip.sheilds = 100;  //DEATH
                mPlayer2.reset2(mMapLoader.shipStartPosP2);
            }

            /*
             * mPlayer1.mShip.mSpriteBody.Mass = 0.5f; //Mass makes ship slower and harder to fly (powerup)
             */


            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, GameTime gameTime)
        {

#if DEBUG


            // calculate the projection and view adjustments for the debug view
            Matrix projection = Matrix.CreateOrthographicOffCenter(0f, graphics.GraphicsDevice.Viewport.Width / MeterInPixels,
                                                             graphics.GraphicsDevice.Viewport.Height / MeterInPixels, 0f, 0f,
                                                             1f);
            Matrix view = Matrix.CreateTranslation(new Vector3((cam.position / MeterInPixels) - (cam.screenCenter / MeterInPixels), 0f)) *
                                                Matrix.CreateTranslation(new Vector3((cam.screenCenter / MeterInPixels), 0f)) *
                                                 Matrix.CreateScale(cam.zoom);


            debugView.RenderDebugData(ref projection, ref view);



#endif
            base.Draw(sb, gameTime);
            mPlayer1.Draw(sb);
            mPlayer2.Draw(sb);

        }

        public override void LoadContent(GraphicsDeviceManager graphics, ContentManager Content)
        {
            base.LoadContent(graphics, Content);

            cam.initCamera(graphics,
                base.mMapLoader.leftWallPosX,
                base.mMapLoader.rightWallPosX,
                base.mMapLoader.topWallPosY,
                base.mMapLoader.bottonWallPosY);

            // load players
            mPlayer1.loadShip(Content, graphics);
            mPlayer2.loadShip(Content, graphics);

            initalisePlayer1Controles();
            initalisePlayer2Controles();
        }

        public override void HandleKeyboard(KeyboardState state, KeyboardState prevState)
        {

            mPlayer1ControllerConfig.actionKeys(state, prevState);
            mPlayer2ControllerConfig.actionKeys(state, prevState);

            base.HandleKeyboard(state, prevState);
        }

        private void initalisePlayer1Controles()
        {
            mPlayer1ControllerConfig = new Input.ControlConfig();
            mPlayer1ControllerConfig.registerIsNownKey(Keys.A, mPlayer1.moveLeft);
            mPlayer1ControllerConfig.registerIsNownKey(Keys.D, mPlayer1.moveRight);
            mPlayer1ControllerConfig.registerIsNownKey(Keys.W, delegate()
            {
                mPlayer1.mShip.mShipParticles.Emitter.Enabled = true;
                mPlayer1.moveForward();
            });

            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.W, delegate() { mPlayer1.mShip.mShipParticles.Emitter.Enabled = false; });
            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.D, mPlayer1.stall);
            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.A, mPlayer1.stall);
            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.F, mPlayer1.fire);
            //mPlayer1ControllerConfig.registerIsNownKey(Keys.F, mPlayer1.fire);

            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.Space, mPlayer1.reset);
        }
        private void initalisePlayer2Controles()
        {
            mPlayer2ControllerConfig = new Input.ControlConfig();
            mPlayer2ControllerConfig.registerIsNownKey(Keys.Left, mPlayer2.moveLeft);
            mPlayer2ControllerConfig.registerIsNownKey(Keys.Right, mPlayer2.moveRight);
            mPlayer2ControllerConfig.registerIsNownKey(Keys.Up, delegate()
            {
                mPlayer2.mShip.mShipParticles.Emitter.Enabled = true;
                mPlayer2.moveForward();
            });

            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.Up, delegate() { mPlayer2.mShip.mShipParticles.Emitter.Enabled = false; });
            mPlayer2ControllerConfig.registerIsUpAndWasDown(Keys.Right, mPlayer2.stall);
            mPlayer2ControllerConfig.registerIsUpAndWasDown(Keys.Left, mPlayer2.stall);
            mPlayer2ControllerConfig.registerIsUpAndWasDown(Keys.RightShift, mPlayer2.fire);
            //mPlayer2ControllerConfig.registerIsNownKey(Keys.RightShift, mPlayer2.fire);

            mPlayer2ControllerConfig.registerIsUpAndWasDown(Keys.Space, mPlayer2.reset);
        }

        public override Matrix getView()
        {
            return cam.View;
        }
    }
}