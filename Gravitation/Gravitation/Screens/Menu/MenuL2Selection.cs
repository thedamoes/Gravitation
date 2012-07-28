using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Gravitation.Screens.Menu
{
    /*
     * This class is basically a Panel ( for those of you famillar with Win Forms but in XNA form)
     */
    class MenuL2Selection : IDrawableScreen
    {
        public event EventHandler<DataClasses.GameSelectedEventArgs> gameSelected;
        private IDrawableScreen[] displayedSprites;

        public void LoadContent(Microsoft.Xna.Framework.GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            
        }
        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.displayedSprites == null)
                return;

            foreach (IDrawableScreen s in this.displayedSprites)
                s.Update(gameTime);
        }
        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.displayedSprites == null)
                return;

            foreach (IDrawableScreen s in this.displayedSprites)
                s.Draw(sb, gameTime);
        }
        public void HandleKeyboard(Microsoft.Xna.Framework.Input.KeyboardState curState, Microsoft.Xna.Framework.Input.KeyboardState prevState)
        {
            
        }
        public void setView(IDrawableScreen[] elements)
        {
            this.displayedSprites = elements;
        }
        public Microsoft.Xna.Framework.Matrix getView()
        {
            throw new NotImplementedException();
        }
    }
}
