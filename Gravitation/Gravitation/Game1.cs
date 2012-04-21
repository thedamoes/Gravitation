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
using XnaGUILib;

using DPSF;
using DPSF.ParticleSystems;


namespace Gravitation
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SoundHandler Sound;

        private KeyboardState _oldKeyState;
        private GamePadState _oldPadState;

        

        private Screens.IDrawableScreen currentScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;

            Sound = new SoundHandler(Content);
            currentScreen = new Screens.MenuScreen(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, Sound, graphics, Content);
            //currentScreen = new Screens.GameScreen(new DataClasses.GameConfiguration("../../../Maps/firstLevel.xml", new SpriteObjects.Ship()));

        }
        protected override void Initialize()
        {

            XnaGUIManager.Initialize(this);

            // create the tool window

            

            this.IsMouseVisible = true; // display the GUI

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            currentScreen.LoadContent(graphics, Content);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            HandleKeyboard();

            DataClasses.IScreenExitData config = currentScreen.Update(gameTime);
            if (config != null)
            {

                if (config.GetType().Equals(typeof(DataClasses.GameConfiguration)))
                {
                    currentScreen = new Screens.GameScreen((DataClasses.GameConfiguration)config);
                    currentScreen.LoadContent(graphics, Content);
                }
            }

            base.Update(gameTime);
        }

        private void HandleKeyboard()
        {
            KeyboardState state = Keyboard.GetState();

            currentScreen.HandleKeyboard(state, _oldKeyState);

            if (state.IsKeyDown(Keys.Escape))
                Exit();

            if (state.IsKeyDown(Keys.Space) && !_oldKeyState.IsKeyDown(Keys.Space))
                Sound.playSound(SoundHandler.Sounds.SHIP_CRASH1);

            _oldKeyState = state;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, currentScreen.getView());

            currentScreen.Draw(spriteBatch, gameTime);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
