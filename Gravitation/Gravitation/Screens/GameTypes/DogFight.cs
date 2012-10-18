using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Gravitation.Screens.GameTypes
{
    class DogFight : TwoPlayerBaseGame
    {
        // game content
        
        
        private SpriteObjects.Upgrade u;
        private List<String> powerups = new List<string>();

        private GameTime mgameTime;

        public DogFight(DataClasses.GameConfiguration gameConfig): base(gameConfig)
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

            powerups.Add("shield");
            powerups.Add("power");
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

            int exampleUpgradeTime = 10; //seconds

            u = new SpriteObjects.Upgrade(base.mWorld, mMapLoader.getPowerupSpawns, powerups, exampleUpgradeTime);

            u.LoadContent(Content, graphics);
        }

        public override void Update(GameTime gameTime)
        {
            mgameTime = gameTime;
            u.Update(gameTime, cam.View);
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
            u.Draw(sb);

        }

        public override void HandleKeyboard(KeyboardState state, KeyboardState prevState)
        {
            base.HandleKeyboard(state,prevState);
        }



        public override Matrix getView()
        {
            return cam.View;
        }
    }
}
