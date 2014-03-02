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
using Gravitation.SpriteObjects.HUD;



namespace Gravitation.Screens.GameTypes
{
    class SinglePlayer : BaseGame
    {
        private CameraControls.Camera cam;
        
        // game content
        
        private ControllerAgents.LocalAgent mPlayer1;
        private Input.ControlConfig mPlayer1ControllerConfig;
        private ControllerAgents.AIAgent mAI2;
        private SpriteObjects.Upgrade u;
        private List<String> powerups = new List<string>();

        private int player1BaseShield = 0;
        private int player1BaseDamage = 0;

        private int player2BaseShield = 0;
        private int player2BaseDamage = 0;
        private HUD _hud = new HUD();

        private GameTime mgameTime;

        public SinglePlayer(DataClasses.GameConfiguration gameConfig): base(gameConfig)
        {
            SpriteObjects.Ship ship = gameConfig.Ship;
            player1BaseDamage = gameConfig.Ship.damage;
            player1BaseShield = gameConfig.Ship.sheilds;
            ship.ShipPosition = mMapLoader.shipStartPosP1;
            ship.World = base.mWorld;
            ship.ShipId = "1";

            SpriteObjects.Ship aIShip = gameConfig.Ship2;
            player2BaseDamage = gameConfig.Ship2.damage;
            player2BaseShield = gameConfig.Ship2.sheilds;
            aIShip.ShipPosition = mMapLoader.shipStartPosP2;
            aIShip.World = base.mWorld;
            aIShip.ShipId = "2";


            powerups.Add("shield");
            powerups.Add("Shotgun");
            powerups.Add("Machinegun");
            powerups.Add("Spiral");
            powerups.Add("Emp");
            powerups.Add("Net");

            mPlayer1 = new ControllerAgents.LocalAgent(ship);
            mAI2 = new ControllerAgents.AIAgent(aIShip, base.mWorld);
            this.cam = new CameraControls.Camera();
            initaliseHUD();
        }


        public override void LoadContent(GraphicsDeviceManager graphics, ContentManager Content)
        {
            base.LoadContent(graphics, Content);
            _hud.LoadContent(Content);
            cam.initCamera(graphics,
                base.mMapLoader.leftWallPosX,
                base.mMapLoader.rightWallPosX,
                base.mMapLoader.topWallPosY,
                base.mMapLoader.bottonWallPosY, 
                base.mMapLoader.MapDimentions.X,
                base.mMapLoader.MapDimentions.Y);

            // load players
            mPlayer1.loadShip(Content, graphics);
            mAI2.loadShip(Content, graphics);
            initalisePlayer1Controles();
            initaliseXBOXControlerControls();
            int exampleUpgradeTime = 10; //seconds
            u = new SpriteObjects.Upgrade(base.mWorld, mMapLoader.getPowerupSpawns, powerups, exampleUpgradeTime);
            u.LoadContent(Content, graphics);
        }

        public override void Update(GameTime gameTime)
        {
            mgameTime = gameTime;
            cam.updateCamera(mPlayer1.myPosition);

            u.Update(gameTime, cam.View);
            //update Controlling agients
            mPlayer1.applyMovement();
            mPlayer1.updateShip(gameTime, cam.View);

            mAI2.applyMovement();
            mAI2.updateShip(gameTime, cam.View);

            if (mPlayer1.mShip.sheilds <= 0)
            {
                mPlayer1.mShip.sheilds = player1BaseShield;  //DEATH
                mPlayer1.mShip.damage = player1BaseDamage;
                mPlayer1.mShip.currentFireState = SpriteObjects.Ship.fireState.Standard;
                mPlayer1.mShip.currentPassiveState = SpriteObjects.Ship.passiveState.Standard;
                mPlayer1.mShip.currentSecondaryFire = SpriteObjects.Ship.secondaryFire.Standard;
                mPlayer1.mShip.currentNegativeState = SpriteObjects.Ship.negativeState.Standard;
                mPlayer1.reset2(mMapLoader.shipStartPosP1);

                for (int x = 0; x < mAI2.mShip.altShots.Count; x++)
                {
                    if (mAI2.mShip.altShots.ElementAt(x) is Gravitation.SpriteObjects.Net)
                    {
                        if (((Gravitation.SpriteObjects.Net)mAI2.mShip.altShots.ElementAt(x)).collided)
                        {
                            ((Gravitation.SpriteObjects.Net)mAI2.mShip.altShots.ElementAt(x)).removeNet();
                        }
                    }
                }


                for (int x = 0; x < mPlayer1.mShip.altShots.Count; x++)
                {
                    if (mPlayer1.mShip.altShots.ElementAt(x) is Gravitation.SpriteObjects.Net)
                    {
                        if (((Gravitation.SpriteObjects.Net)mPlayer1.mShip.altShots.ElementAt(x)).collided)
                        {
                            ((Gravitation.SpriteObjects.Net)mPlayer1.mShip.altShots.ElementAt(x)).removeNet();
                        }
                    }
                }
            }

            if (mAI2.mShip.sheilds <= 0)
            {
                mAI2.mShip.sheilds = player2BaseShield;  //DEATH
                mAI2.mShip.damage = player2BaseDamage;
                mAI2.mShip.currentFireState = SpriteObjects.Ship.fireState.Standard;
                mAI2.mShip.currentPassiveState = SpriteObjects.Ship.passiveState.Standard;
                mAI2.mShip.currentSecondaryFire = SpriteObjects.Ship.secondaryFire.Standard;
                mAI2.mShip.currentNegativeState = SpriteObjects.Ship.negativeState.Standard;
                mAI2.reset2(mMapLoader.shipStartPosP2);

                for (int x = 0; x < mPlayer1.mShip.altShots.Count; x++)
                {
                    if (mPlayer1.mShip.altShots.ElementAt(x) is Gravitation.SpriteObjects.Net)
                    {
                        if (((Gravitation.SpriteObjects.Net)mPlayer1.mShip.altShots.ElementAt(x)).collided)
                        {
                            ((Gravitation.SpriteObjects.Net)mPlayer1.mShip.altShots.ElementAt(x)).removeNet();
                        }
                    }
                }


                for (int x = 0; x < mAI2.mShip.altShots.Count; x++)
                {
                    if (mAI2.mShip.altShots.ElementAt(x) is Gravitation.SpriteObjects.Net)
                    {
                        if (((Gravitation.SpriteObjects.Net)mAI2.mShip.altShots.ElementAt(x)).collided)
                        {
                            ((Gravitation.SpriteObjects.Net)mAI2.mShip.altShots.ElementAt(x)).removeNet();
                        }
                    }
                }
            }
            Console.WriteLine("Player one position is X = [" + MeterInPixels * mPlayer1.mShip.Position.X + "]  Y = [" + MeterInPixels * mPlayer1.mShip.Position.Y + "]");
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

            mPlayer1.Draw(sb, debugView, projection, view);
            mAI2.Draw(sb, debugView, projection, view);
            u.Draw(sb);
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
            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.W, delegate() { mPlayer1.mShip.pauseThrustSound(); mPlayer1.mShip.mShipParticles.Emitter.Enabled = false; });

            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.D, mPlayer1.stall);
            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.A, mPlayer1.stall);
            mPlayer1ControllerConfig.registerIsNownKey(Keys.F, mPlayer1.fire);
            mPlayer1ControllerConfig.registerIsNownKey(Keys.V, mPlayer1.altFire);

            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.R, mPlayer1.reset);
        }

        private void initaliseXBOXControlerControls()
        {
            mPlayer1ControllerConfig.registerXBOXButtonPress(Buttons.A, delegate() { mPlayer1.moveForward(); mPlayer1.mShip.mShipParticles.Emitter.Enabled = true; });
            mPlayer1ControllerConfig.registerXBOXButtonIsDownAndWasUp(Buttons.A, delegate() { mPlayer1.mShip.pauseThrustSound(); });
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

        private void initaliseHUD()
        {
            _hud.AddHUDDObject(new LifeBar(mPlayer1.mShip, new Vector2(20, 480)));
            ScreenManager.GetScreenManager.ScreenOverlay = _hud;
        }

        public override Matrix getView()
        {
            return cam.View;
        }

    }


    


            





}
