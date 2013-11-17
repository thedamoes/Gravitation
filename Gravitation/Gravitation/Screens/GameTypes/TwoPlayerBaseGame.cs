using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;

using Gravitation.GameStates;

namespace Gravitation.Screens.GameTypes
{
    public class TwoPlayerBaseGame : BaseGame
    {
        protected ControllerAgents.LocalAgent mPlayer1;
        protected ControllerAgents.LocalAgent mPlayer2;
        protected Input.ControlConfig mPlayer1ControllerConfig;
        protected Input.ControlConfig mPlayer2ControllerConfig;

        private int player1BaseShield = 0;
        private int player1BaseDamage = 0;

        private int player2BaseShield = 0;
        private int player2BaseDamage = 0;

        private float _angle;

        bool hitClosest;

        Vector2 point;
        Vector2 point1;
        Vector2 point2;
        Vector2 normal;

        protected CameraControls.TwoPlayerCamera cam;

        private TwoPlayerGameState state = new TwoPlayerGameState();

        public TwoPlayerGameState GameState
        {
            get {
                    this.state.player2State.position = this.mPlayer2.mShip.Position;
                    this.state.playerstate.position = this.mPlayer1.mShip.Position;

                    this.state.player2State.rotation = this.mPlayer2.Rotation;
                    this.state.playerstate.rotation = this.mPlayer1.Rotation;
                    
                    return this.state;

                }
        }

        public TwoPlayerBaseGame(DataClasses.GameConfiguration gameConfig)
            : base(gameConfig)
        {
            // load ship 1
            SpriteObjects.Ship ship = gameConfig.Ship;
            player1BaseDamage = gameConfig.Ship.damage;
            player1BaseShield = gameConfig.Ship.sheilds;
            ship.ShipPosition = mMapLoader.shipStartPosP1;
            ship.World = base.mWorld;
            ship.ShipId = "1";

            // load ship 2
            SpriteObjects.Ship ship2 = gameConfig.Ship2;
            player2BaseDamage = gameConfig.Ship2.damage;
            player2BaseShield = gameConfig.Ship2.sheilds;
            ship2.ShipPosition = mMapLoader.shipStartPosP2;
            ship2.World = base.mWorld;
            ship2.ShipId = "2";

            mPlayer1 = new ControllerAgents.LocalAgent(ship);
            mPlayer2 = new ControllerAgents.LocalAgent(ship2);

            cam = new CameraControls.TwoPlayerCamera();

            _angle = 0.0f;
        }

        public override void Update(GameTime gameTime)
        {
            
            cam.updateCamera(mPlayer1.myPosition, mPlayer2.myPosition);
            //update Controlling agients

            mPlayer1.updateShip(gameTime, cam.View);
            mPlayer2.updateShip(gameTime, cam.View);

            mPlayer1.applyMovement();
            mPlayer2.applyMovement();

            if (mPlayer1.mShip.sheilds <= 0)
            {
                mPlayer1.mShip.sheilds = player1BaseShield;  //DEATH
                mPlayer1.mShip.damage = player1BaseDamage;
                mPlayer1.mShip.currentFireState = SpriteObjects.Ship.fireState.Standard;
                mPlayer1.mShip.currentPassiveState = SpriteObjects.Ship.passiveState.Standard;
                mPlayer1.mShip.currentSecondaryFire = SpriteObjects.Ship.secondaryFire.Standard;
                mPlayer1.mShip.currentNegativeState = SpriteObjects.Ship.negativeState.Standard;
                mPlayer1.reset2(mMapLoader.shipStartPosP1);

                for (int x = 0; x < mPlayer2.mShip.altShots.Count; x++) 
                {
                    if (mPlayer2.mShip.altShots.ElementAt(x) is Gravitation.SpriteObjects.Net) 
                    {
                        if (((Gravitation.SpriteObjects.Net)mPlayer2.mShip.altShots.ElementAt(x)).collided)
                        {
                            ((Gravitation.SpriteObjects.Net)mPlayer2.mShip.altShots.ElementAt(x)).removeNet();
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

            if (mPlayer2.mShip.sheilds <= 0)
            {
                mPlayer2.mShip.sheilds = player2BaseShield;  //DEATH
                mPlayer2.mShip.damage = player2BaseDamage;
                mPlayer2.mShip.currentFireState = SpriteObjects.Ship.fireState.Standard;
                mPlayer2.mShip.currentPassiveState = SpriteObjects.Ship.passiveState.Standard;
                mPlayer2.mShip.currentSecondaryFire = SpriteObjects.Ship.secondaryFire.Standard;
                mPlayer2.mShip.currentNegativeState = SpriteObjects.Ship.negativeState.Standard;
                mPlayer2.reset2(mMapLoader.shipStartPosP2);

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


                for (int x = 0; x < mPlayer2.mShip.altShots.Count; x++)
                {
                    if (mPlayer2.mShip.altShots.ElementAt(x) is Gravitation.SpriteObjects.Net)
                    {
                        if (((Gravitation.SpriteObjects.Net)mPlayer2.mShip.altShots.ElementAt(x)).collided)
                        {
                            ((Gravitation.SpriteObjects.Net)mPlayer2.mShip.altShots.ElementAt(x)).removeNet();
                        }
                    }
                }
            }

            /*
             * mPlayer1.mShip.mSpriteBody.Mass = 0.5f; //Mass makes ship slower and harder to fly (powerup)
             */


            // THE BELOW GOES IN THE UPDATE STATEMENT

            const float l = 11.0f;
            point1 = new Vector2(23f, -12.5f);
            Vector2 d = new Vector2(l * (float)Math.Cos(_angle), l * (float)Math.Sin(_angle));
            point2 = point1 + d;

            point = Vector2.Zero;
            normal = Vector2.Zero;
            hitClosest = false;

            base.mWorld.RayCast((f, p, n, fr) =>
            {
                //Console.WriteLine("Hit Closest");
                hitClosest = true;
                point = p;
                normal = n;
                return fr;
            }, point1, point2);



            //rotate the beam
            _angle += 0.25f * 3.14159f / 180.0f;


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


            if (hitClosest)
            {
                debugView.BeginCustomDraw(ref projection, ref view);
                debugView.DrawPoint(point, .5f, new Color(0.4f, 0.9f, 0.4f));

                debugView.DrawSegment(point1, point, new Color(0.8f, 0.8f, 0.8f));

                Vector2 head = point + 0.5f * normal;
                debugView.DrawSegment(point, head, new Color(0.9f, 0.9f, 0.4f));
                debugView.EndCustomDraw();

                //d = √ (x₂ - x₁)^2 + (y₂ - y₁)^2
                double x1 = point1.X;
                double x2 = point.X;
                double y1 = point1.Y;
                double y2 = point.Y;

                double xPoints = (x2-x1)*(x2-x1);
                double yPoints = (y2-y1)*(y2-y1);


                double totalLength = Math.Sqrt((xPoints + yPoints));

                //Console.WriteLine("Total length of line  = " + totalLength);

            } else {
                debugView.BeginCustomDraw(ref projection, ref view);
                debugView.DrawSegment(point1, point2, new Color(0.8f, 0.8f, 0.8f));
                debugView.EndCustomDraw();

                double x1 = point1.X;
                double x2 = point2.X;
                double y1 = point1.Y;
                double y2 = point2.Y;

                double xPoints = (x2-x1)*(x2-x1);
                double yPoints = (y2-y1)*(y2-y1);

                double totalLength = Math.Sqrt((xPoints + yPoints));

                //Console.WriteLine("Total length of line  = " + totalLength);
            }



            debugView.RenderDebugData(ref projection, ref view);



#endif
            base.Draw(sb, gameTime);
            mPlayer1.Draw(sb, debugView, projection,  view);
            mPlayer2.Draw(sb, debugView, projection, view);

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
                //mPlayer1.mShip.playThrustSound();
            });

            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.W, delegate() { mPlayer1.mShip.pauseThrustSound();  mPlayer1.mShip.mShipParticles.Emitter.Enabled = false; });
            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.D, mPlayer1.stall);
            mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.A, mPlayer1.stall);
            //mPlayer1ControllerConfig.registerIsUpAndWasDown(Keys.F, mPlayer1.fire);
            mPlayer1ControllerConfig.registerIsNownKey(Keys.F, mPlayer1.fire);
            mPlayer1ControllerConfig.registerIsNownKey(Keys.V, mPlayer1.altFire);

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
                //mPlayer2.mShip.playThrustSound();
            });

            mPlayer2ControllerConfig.registerIsUpAndWasDown(Keys.Up, delegate() { mPlayer2.mShip.pauseThrustSound(); mPlayer2.mShip.mShipParticles.Emitter.Enabled = false; });
            mPlayer2ControllerConfig.registerIsUpAndWasDown(Keys.Right, mPlayer2.stall);
            mPlayer2ControllerConfig.registerIsUpAndWasDown(Keys.Left, mPlayer2.stall);
            //mPlayer2ControllerConfig.registerIsUpAndWasDown(Keys.RightShift, mPlayer2.fire);
            mPlayer2ControllerConfig.registerIsNownKey(Keys.RightShift, mPlayer2.fire);
            mPlayer1ControllerConfig.registerIsNownKey(Keys.RightControl, mPlayer2.altFire);

            mPlayer2ControllerConfig.registerIsUpAndWasDown(Keys.Space, mPlayer2.reset);
        }

        public override Matrix getView()
        {
            return cam.View;
        }
    }
}
