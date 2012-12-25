using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravitation.Screens.GameTypes
{
    public class TwoPlayerServerWrapper : IDrawableScreen
    {
        public event EventHandler<DataClasses.GameSelectedEventArgs> gameSelected;
        TwoPlayerBaseGame game;
        Comms.Server server;

        public TwoPlayerServerWrapper(TwoPlayerBaseGame game, Comms.Server server)
        {
            this.game = game;
            this.server = server;
        }

        public void LoadContent(Microsoft.Xna.Framework.GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            this.game.LoadContent(dMan, cm);
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            this.game.Update(gameTime);
            this.server.sendGamestaeUpdate(game.GameState, gameTime.TotalGameTime);
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Microsoft.Xna.Framework.GameTime gameTime)
        {
            this.game.Draw(sb, gameTime);
        }

        public void HandleKeyboard(Microsoft.Xna.Framework.Input.KeyboardState curState, Microsoft.Xna.Framework.Input.KeyboardState prevState)
        {
            this.game.HandleKeyboard(curState, prevState);
        }

        public Microsoft.Xna.Framework.Matrix getView()
        {
            return this.game.getView();
        }


        public void windowCloseing()
        {
            
        }
    }
}
