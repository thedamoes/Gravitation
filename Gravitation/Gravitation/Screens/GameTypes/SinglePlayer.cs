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

        public SinglePlayer(DataClasses.GameConfiguration gameConfig): base(gameConfig)
        {
            SpriteObjects.Ship ship = gameConfig.Ship;
            ship.ShipPosition = mMapLoader.shipStartPosP1;
            ship.World = base.mWorld;

            mPlayer1 = new ControllerAgents.LocalAgent(ship);
            this.cam = new CameraControls.Camera();
        }

        public override void LoadContent(GraphicsDeviceManager graphics, ContentManager Content)
        {
            cam.initCamera(graphics,base.mMapLoader.MapDimentions.Y,base.mMapLoader.MapDimentions.X,
                base.mMapLoader.leftWallPosX,
                base.mMapLoader.rightWallPosX,
                base.mMapLoader.topWallPosY,
                base.mMapLoader.bottonWallPosY);

            // load players
            mPlayer1.loadShip(Content, graphics);
            initalisePlayer1Controles();
            base.LoadContent(graphics, Content);
        }

        public override DataClasses.IScreenExitData Update(GameTime gameTime)
        {
            cam.updateCamera(mPlayer1.myPosition);

            //update Controlling agients
            mPlayer1.applyMovement();

            mPlayer1.updateShot(gameTime, cam.View);

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
            mPlayer1ControllerConfig.registerIsNownKey(Keys.W, mPlayer1.moveForward);

            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.D, mPlayer1.stall);
            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.A, mPlayer1.stall);
            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.F, mPlayer1.fire);
            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.Space, mPlayer1.reset);
        }

        public override Matrix getView()
        {
            return cam.View;
        }

    }
}
