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

            graphics.PreferredBackBufferWidth = 700;
            graphics.PreferredBackBufferHeight = 500;

            Sound = new SoundHandler(Content);
            currentScreen = new Screens.Menu.MenuScreen(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, Sound, graphics, Content);
            //currentScreen = new Screens.GameTypes.SinglePlayer(new DataClasses.GameConfiguration("../../../Maps/level1.xml", new SpriteObjects.Ship(), null));
            

            currentScreen.gameSelected += this.gameSelected;

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
            
            currentScreen.Update(gameTime);
           

            base.Update(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            if (this.currentScreen != null)
                this.currentScreen.windowCloseing();
        }

        private void gameSelected(object sender, DataClasses.GameSelectedEventArgs e)
        {
            currentScreen = e.getGame();
            currentScreen.LoadContent(graphics, Content);
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
