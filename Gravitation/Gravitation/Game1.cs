using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.DebugViews;

namespace Gravitation
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        protected DebugViewXNA debugView;

        private KeyboardState _oldKeyState;
        private GamePadState _oldPadState;
        private SpriteFont _font;

        private World _world;

        private Texture2D _groundSprite;

        // Simple camera controls
        private Matrix _view;
        private Vector2 _cameraPosition;
        private Vector2 _screenCenter;

        private const float MeterInPixels = 64f;

        private Maps.MapLoader mMapLoader;

        private ControllerAgents.LocalAgent mPlayer1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;

            _world = new World(new Vector2(0, 20));
            mPlayer1 = new ControllerAgents.LocalAgent(new SpriteObjects.Ship(_world, (new Vector2(graphics.PreferredBackBufferWidth / 2f,
                                                graphics.PreferredBackBufferHeight / 2f) / MeterInPixels) + new Vector2((6f * MeterInPixels), -1.25f)));

            mMapLoader = new Maps.MapLoader("../../../Maps/firstLevel.xml",_world);

            

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            // Initialize camera controls
            _view = Matrix.Identity;
            _cameraPosition = Vector2.Zero;

            _screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f,
                                                graphics.GraphicsDevice.Viewport.Height / 2f);

            _font = Content.Load<SpriteFont>("font");

            // Load sprites
            _groundSprite = Content.Load<Texture2D>("platform"); // 512px x 64px =>   8m x 1m

            mPlayer1.loadShip(Content);


            /* Ground */
            Vector2 groundPosition = (_screenCenter / MeterInPixels)+ new Vector2(0, 1.25f);
          

            // load the map
            mMapLoader.loadMap(Content);


            debugView = new DebugViewXNA(_world);
            debugView.AppendFlags(DebugViewFlags.DebugPanel);
            debugView.DefaultShapeColor = Color.White;
            debugView.SleepingShapeColor = Color.LightGray;
            debugView.RemoveFlags(DebugViewFlags.Controllers);
            debugView.RemoveFlags(DebugViewFlags.Joint);
  


            debugView.LoadContent(GraphicsDevice, Content);



            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            HandleKeyboard();
         
            //maintain camera position
            _cameraPosition.X = (-mPlayer1.myPosition.X * MeterInPixels) + graphics.PreferredBackBufferWidth / 2;
            _cameraPosition.Y = (-mPlayer1.myPosition.Y * MeterInPixels) + graphics.PreferredBackBufferHeight / 2;

            _view = Matrix.CreateTranslation(new Vector3(_cameraPosition - _screenCenter, 0f)) *
                Matrix.CreateTranslation(new Vector3(_screenCenter, 0f));

            //update Controlling agients
            mPlayer1.applyMovement();

            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

            base.Update(gameTime);
        }



        private bool _rectangleBody_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }




        private void HandleKeyboard()
        {
            KeyboardState state = Keyboard.GetState();

            // Switch between circle body and camera control
            if (state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift))
            {
                // Move camera
                if (state.IsKeyDown(Keys.A))
                    _cameraPosition.X += 1.5f;

                if (state.IsKeyDown(Keys.D))
                    _cameraPosition.X -= 1.5f;

                if (state.IsKeyDown(Keys.W))
                    _cameraPosition.Y += 1.5f;

                if (state.IsKeyDown(Keys.S))
                    _cameraPosition.Y -= 1.5f;

                _view = Matrix.CreateTranslation(new Vector3(_cameraPosition - _screenCenter, 0f)) *
                        Matrix.CreateTranslation(new Vector3(_screenCenter, 0f));
            }
            else
            {
                // We make it possible to rotate the circle body
                if (state.IsKeyDown(Keys.A))
                    mPlayer1.moveLeft();

                if (state.IsKeyDown(Keys.D))
                    mPlayer1.moveRight();

                if (state.IsKeyDown(Keys.W))
                    mPlayer1.moveForward();

                if (state.IsKeyUp(Keys.D) && _oldKeyState.IsKeyDown(Keys.D))
                    mPlayer1.stall();

                if (state.IsKeyUp(Keys.A) && _oldKeyState.IsKeyDown(Keys.A))
                    mPlayer1.stall();

                if (state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
                    mPlayer1.reset();
#if DEBUG
                if (state.IsKeyUp(Keys.R) && _oldKeyState.IsKeyDown(Keys.R))
                {
                       // reinitalise and reload the map from xml DEBUG
                    mMapLoader.unloadBodies();
                    mMapLoader = new Maps.MapLoader("../../../Maps/firstLevel.xml", _world);
                    mMapLoader.loadMap(Content);
                }
#endif
               

            }


            
            if (state.IsKeyDown(Keys.Escape))
                Exit();

            _oldKeyState = state;
        }




        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here


     
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _view);

          

            mPlayer1.Draw(spriteBatch);

#if DEBUG


            // calculate the projection and view adjustments for the debug view
            Matrix projection = Matrix.CreateOrthographicOffCenter(0f, graphics.GraphicsDevice.Viewport.Width / MeterInPixels,
                                                             graphics.GraphicsDevice.Viewport.Height / MeterInPixels, 0f, 0f,
                                                             1f);
            Matrix view = Matrix.CreateTranslation(new Vector3((_cameraPosition / MeterInPixels) - (_screenCenter / MeterInPixels), 0f)) * Matrix.CreateTranslation(new Vector3((_screenCenter / MeterInPixels), 0f));


            debugView.RenderDebugData(ref projection, ref view);

#endif

            mMapLoader.drawMap(spriteBatch);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
