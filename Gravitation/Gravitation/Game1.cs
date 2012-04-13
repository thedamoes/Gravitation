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

namespace Gravitation
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private KeyboardState _oldKeyState;
        private GamePadState _oldPadState;
        private SpriteFont _font;

        private World _world;

        private Body _groundBody;

        private Texture2D _groundSprite;

        // Simple camera controls
        private Matrix _view;
        private Vector2 _cameraPosition;
        private Vector2 _screenCenter;

        private const float MeterInPixels = 64f;

        //gay ass member variables that should be removed....77
        private SpriteObjects.Ship player1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;


            _world = new World(new Vector2(0, 20));
            player1 = new SpriteObjects.Ship(_world, (new Vector2(graphics.PreferredBackBufferWidth / 2f,
                                                graphics.PreferredBackBufferHeight / 2f) / MeterInPixels) + new Vector2(0, -1.5f));

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

           // _font = Content.Load<SpriteFont>("font");

            // Load sprites
            _groundSprite = Content.Load<Texture2D>("platform"); // 512px x 64px =>   8m x 1m

            player1.LoadContent(Content, "floor");


            /* Ground */
            Vector2 groundPosition = (_screenCenter / MeterInPixels) + new Vector2(0, 1.25f);

            // Create the ground fixture
            _groundBody = BodyFactory.CreateRectangle(_world, 512f / MeterInPixels, 64f / MeterInPixels, 1f, groundPosition);
            _groundBody.IsStatic = true;
            _groundBody.Restitution = 0.3f;
            _groundBody.Friction = 0.5f;





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
            // TODO: Add your update logic here

            //We update the world

            //maintain camera position
            _cameraPosition.X = (player1.Position.X * -MeterInPixels) + graphics.PreferredBackBufferWidth / 2;
            _cameraPosition.Y = (player1.Position.Y * -MeterInPixels) + graphics.PreferredBackBufferHeight / 2;


            _view = Matrix.CreateTranslation(new Vector3(_cameraPosition - _screenCenter, 0f)) *
        Matrix.CreateTranslation(new Vector3(_screenCenter, 0f));

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
               /* if (state.IsKeyDown(Keys.A))
                    _cameraPosition.X += 1.5f;

                if (state.IsKeyDown(Keys.D))
                    _cameraPosition.X -= 1.5f;

                if (state.IsKeyDown(Keys.W))
                    _cameraPosition.Y += 1.5f;

                if (state.IsKeyDown(Keys.S))
                    _cameraPosition.Y -= 1.5f;*/

                _view = Matrix.CreateTranslation(new Vector3(_cameraPosition - _screenCenter, 0f)) *
                        Matrix.CreateTranslation(new Vector3(_screenCenter, 0f));
            }
            else
            {
                // We make it possible to rotate the circle body
                /*if (state.IsKeyDown(Keys.A))//g
                    _rectangleBody.ApplyForce(new Vector2(-10, 0));

                if (state.IsKeyDown(Keys.D))
                    _rectangleBody.ApplyForce(new Vector2(10, 0));

                if (state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
                    _rectangleBody.ApplyLinearImpulse(new Vector2(0, -10));
                
                if (state.IsKeyDown(Keys.E))
                    _rectangleBody.ApplyTorque(10);

                if (state.IsKeyDown(Keys.Q))
                    _rectangleBody.ApplyTorque(-10);*/

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

            /* Ground position and origin */
            Vector2 groundPos = _groundBody.Position * MeterInPixels;
            Vector2 groundOrigin = new Vector2(_groundSprite.Width / 2f, _groundSprite.Height / 2f);

     
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _view);

            //Draw ground
            spriteBatch.Draw(_groundSprite, groundPos, null, Color.White, 0f, groundOrigin, 1f, SpriteEffects.None, 0f);
            player1.Draw(spriteBatch);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
