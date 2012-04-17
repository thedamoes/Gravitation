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

namespace Gravitation.Screens
{
    class GameScreen : IDrawableScreen
    {
        private World mWorld;
        private GraphicsDeviceManager graphics;
        private ContentManager Content;

        // Simple camera controls
        private Matrix _view;
        private Vector2 _cameraPosition;
        private Vector2 _screenCenter;
        private Vector3 _cameraZoom;
        private SpriteFont _font;

        protected DebugViewXNA debugView;

        private const float MeterInPixels = 64f;

        // game content
        private Maps.MapLoader mMapLoader;
        private ControllerAgents.LocalAgent mPlayer1;

        public GameScreen()
        {
            mWorld = new World(new Vector2(0, 2));
            mMapLoader = new Maps.MapLoader("../../../Maps/firstLevel.xml", mWorld);
            mPlayer1 = new ControllerAgents.LocalAgent(new SpriteObjects.Ship(mWorld, mMapLoader.shipStartPosP1));

            _cameraZoom = new Vector3(0.5f, 0.5f, 0.5f);
        }

        public void LoadContent(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, GraphicsDeviceManager graphics, ContentManager Content)
        {
            this.graphics = graphics;
            this.Content = Content;

            // Initialize camera controls
            _view = Matrix.Identity *
                        Matrix.CreateScale(_cameraZoom);
            _cameraPosition = Vector2.Zero;
            _screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f,
                                                graphics.GraphicsDevice.Viewport.Height / 2f);
            _font = Content.Load<SpriteFont>("font");

            // load players
            mPlayer1.loadShip(Content);

            // load the map
            mMapLoader.loadMap(Content);


#if DEBUG
            debugView = new DebugViewXNA(mWorld);
            debugView.AppendFlags(DebugViewFlags.DebugPanel);
            debugView.DefaultShapeColor = Color.White;
            debugView.SleepingShapeColor = Color.LightGray;
            debugView.RemoveFlags(DebugViewFlags.Controllers);
            debugView.RemoveFlags(DebugViewFlags.Joint);



            debugView.LoadContent(graphics.GraphicsDevice, Content);
#endif
        }

        public void Update(GameTime gameTime)
        {
            //maintain camera position
            float playerPosInPixlesX = (-mPlayer1.myPosition.X * MeterInPixels);  // these here variables
            float playerPosInPixlesY = (-mPlayer1.myPosition.Y * MeterInPixels);  // are what the "zoom"
            float camX = (playerPosInPixlesX + graphics.PreferredBackBufferWidth);  // offsets the camera by
            float camY = (playerPosInPixlesY + graphics.PreferredBackBufferHeight); // from it's origin

            // couldent think of any other better way to do it?? if u can feel free
            // these lines stop the cammera from moveing past the bounds of the map
            if (-playerPosInPixlesX < (graphics.PreferredBackBufferWidth - mMapLoader.leftWallPosX)) _cameraPosition.X = (mMapLoader.leftWallPosX);
            else if (-playerPosInPixlesX > (mMapLoader.rightWallPosX - graphics.PreferredBackBufferWidth)) _cameraPosition.X = -(mMapLoader.rightWallPosX - (graphics.PreferredBackBufferWidth) * 2);
            else _cameraPosition.X = camX;

            if (-playerPosInPixlesY < (graphics.PreferredBackBufferHeight + mMapLoader.topWallPosY)) _cameraPosition.Y = (-mMapLoader.topWallPosY);
            else if (-playerPosInPixlesY > (mMapLoader.bottonWallPosY - graphics.PreferredBackBufferHeight)) _cameraPosition.Y = (graphics.PreferredBackBufferHeight * 2) - mMapLoader.bottonWallPosY;
            else _cameraPosition.Y = camY;

            _view = Matrix.CreateTranslation(new Vector3(_cameraPosition - _screenCenter, 0f)) *
                Matrix.CreateTranslation(new Vector3(_screenCenter, 0f)) *
                 Matrix.CreateScale(_cameraZoom);

            //update Controlling agients
            mPlayer1.applyMovement();

            mWorld.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            mMapLoader.drawMap(sb);

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
        }

        public void HandleKeyboard(KeyboardState state, KeyboardState prevState)
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

            if (state.IsKeyDown(Keys.Space) && prevState.IsKeyUp(Keys.Space))
                mPlayer1.reset();
#if DEBUG
            if (state.IsKeyUp(Keys.R) && prevState.IsKeyDown(Keys.R))
            {
                // reinitalise and reload the map from xml DEBUG
                mMapLoader.unloadBodies();
                mMapLoader = new Maps.MapLoader("../../../Maps/firstLevel.xml", mWorld);
                mMapLoader.loadMap(Content);
            }
#endif
        }


        public Matrix getView()
        {
            return _view;
        }
    }
}
