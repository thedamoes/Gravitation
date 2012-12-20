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

namespace Gravitation.Screens
{
    public class BaseGame: IDrawableScreen
    {
        public event EventHandler<DataClasses.GameSelectedEventArgs> gameSelected;
        protected World mWorld;
        protected GraphicsDeviceManager graphics;
        private ContentManager Content;
        private SpriteFont _font;
        public const float MeterInPixels = 64f;

        protected DebugViewXNA debugView;

        protected Maps.MapLoader mMapLoader;

        public BaseGame(DataClasses.GameConfiguration gameConfig)
        {
            mWorld = new World(new Vector2(0, 1f)); //0.5
            mMapLoader = new Maps.MapLoader(gameConfig.MapName, mWorld);
        }

        public virtual void LoadContent(GraphicsDeviceManager dMan, ContentManager cm)
        {
            this.graphics = dMan;
            this.Content = cm;

            // load the map
            mMapLoader.loadMap(Content);

            _font = Content.Load<SpriteFont>("font");
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

        public virtual void Update(GameTime gameTime)
        {
            mWorld.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
        }

        public virtual void Draw(SpriteBatch sb, GameTime gameTime)
        {
            mMapLoader.drawMap(sb);
        }

        public virtual void HandleKeyboard(KeyboardState curState, KeyboardState prevState)
        {

#if DEBUG
            if (curState.IsKeyUp(Keys.R) && prevState.IsKeyDown(Keys.R))
            {
                // reinitalise and reload the map from xml DEBUG
                mMapLoader.unloadBodies();
                mMapLoader = new Maps.MapLoader(mMapLoader.MapFile, mWorld);
                mMapLoader.loadMap(Content);
            }
#endif
        }

        public virtual Matrix getView()
        {
            return new Matrix(); // dont call this method always override
        }


        public virtual void windowCloseing()
        {
            
        }
    }
}
