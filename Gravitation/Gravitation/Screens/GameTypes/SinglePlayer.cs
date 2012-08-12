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



namespace Gravitation.Screens.GameTypes
{
    class SinglePlayer : BaseGame
    {
        private CameraControls.Camera cam;
        
        // game content
        
        private ControllerAgents.LocalAgent mPlayer1;
        private Input.ControlConfig mPlayer1ControllerConfig;
        private SpriteObjects.Upgrade u;
        private List<String> powerups = new List<string>();


        private GameTime mgameTime;

        public SinglePlayer(DataClasses.GameConfiguration gameConfig): base(gameConfig)
        {
            SpriteObjects.Ship ship = gameConfig.Ship;
            ship.ShipPosition = mMapLoader.shipStartPosP1;
            ship.World = base.mWorld;
            

            powerups.Add("f");
            mPlayer1 = new ControllerAgents.LocalAgent(ship);
            this.cam = new CameraControls.Camera();
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
            initalisePlayer1Controles();
            initaliseXBOXControlerControls();
            u = new SpriteObjects.Upgrade(base.mWorld, new Vector2(mMapLoader.leftWallPosX, mMapLoader.topWallPosY), new Vector2(mMapLoader.rightWallPosX, mMapLoader.bottonWallPosY), powerups);


            u.LoadContent(Content, graphics);

        }

        public override void Update(GameTime gameTime)
        {
            mgameTime = gameTime;
            cam.updateCamera(mPlayer1.myPosition);

            u.Update(gameTime, cam.View);
            //update Controlling agients
            mPlayer1.applyMovement();

            mPlayer1.updateShot(gameTime, cam.View);

            mPlayer1.thrust(gameTime, cam.View);


            if (mPlayer1.mShip.sheilds <= 0)
            {
                mPlayer1.mShip.sheilds = 100;  //DEATH
                mPlayer1.reset2(mMapLoader.shipStartPosP1);
            }

            base.Update(gameTime);
        }


        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, GameTime gameTime)
        {
            mPlayer1.Draw(sb);
            u.Draw(sb);
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
        }

        public override void HandleKeyboard(KeyboardState state, KeyboardState prevState)
        {
            mPlayer1ControllerConfig.actionKeys(state, prevState);
            base.HandleKeyboard(state,prevState);
            

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
            mPlayer1ControllerConfig.registerIsNownKey(Keys.F, mPlayer1.fire);

            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.Space, mPlayer1.reset);
        }

        private void initaliseXBOXControlerControls()
        {
            mPlayer1ControllerConfig.registerXBOXButtonPress(Buttons.A, delegate() { mPlayer1.moveForward(); mPlayer1.mShip.mShipParticles.Emitter.Enabled = true; });
            mPlayer1ControllerConfig.registerXBOXButtonPress(new Input.XBOXControllerAnalog(Input.XBOXControllerAnalog.SICK.LEFT, Input.XBOXControllerAnalog.AXIS.X_AXIS), mPlayer1.moveLeft);
            mPlayer1ControllerConfig.registerXBOXButtonPress(new Input.XBOXControllerAnalog(Input.XBOXControllerAnalog.SICK.LEFT, Input.XBOXControllerAnalog.AXIS.X_AXIS), mPlayer1.moveRight);

            mPlayer1ControllerConfig.registerXBOXButtonPress(Buttons.DPadLeft, mPlayer1.moveLeft);
            mPlayer1ControllerConfig.registerXBOXButtonPress(Buttons.DPadRight, mPlayer1.moveRight);
            mPlayer1ControllerConfig.registerXBOXButtonIsDownAndWasUp(Buttons.DPadLeft, mPlayer1.stall);
            mPlayer1ControllerConfig.registerXBOXButtonIsDownAndWasUp(Buttons.DPadRight, mPlayer1.stall);

            mPlayer1ControllerConfig.registerXBOXButtonIsDownAndWasUp(Buttons.LeftThumbstickUp, delegate() { mPlayer1.mShip.mShipParticles.Emitter.Enabled = false; });
            mPlayer1ControllerConfig.registerXBOXButtonIsDownAndWasUp(Buttons.LeftThumbstickRight, mPlayer1.stall);
            mPlayer1ControllerConfig.registerXBOXButtonIsDownAndWasUp(Buttons.LeftThumbstickLeft, mPlayer1.stall);

            mPlayer1ControllerConfig.registerXBOXButtonPress(Buttons.RightShoulder, mPlayer1.fire);
        }

        public override Matrix getView()
        {
            return cam.View;
        }

    }
}
