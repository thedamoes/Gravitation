using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Gravitation.SpriteObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Gravitation.Screens.Menu
{
    class MenuButton : Sprite, IDrawableScreen
    {
        public event EventHandler<DataClasses.GameSelectedEventArgs> gameSelected;
        public event EventHandler click;

        private SpriteFont mFont;
        private String myText;
        private Vector2 txtPosition;


        private bool mIsHighlighted = false;
 
        public MenuButton(String text, Vector2 position, float scaleX, float scaleY) : base(position,0f)
        {
            this.myText = text;
            base.WidthScale = scaleX;
            base.HeightScale = scaleY;
        }

        public void highlight()
        {
            this.mIsHighlighted = true;
        }

        public void unHighlight()
        {
            this.mIsHighlighted = false;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            if(this.mIsHighlighted)
                base.Draw(sb);

            sb.DrawString(mFont, this.myText, this.txtPosition, Color.White);
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager theContentManager, string theAssetName)
        {
            base.LoadContent(theContentManager, theAssetName);

            mFont = theContentManager.Load<SpriteFont>("font");

            this.txtPosition = new Vector2(base.Position.X + (base.width / 2) - mFont.MeasureString(myText).X/2
                , base.Position.Y + (base.height / 2) - mFont.MeasureString(myText).Y / 2);
            

        }
        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            Rectangle position = new Rectangle((int)base.Position.X, (int)base.Position.Y, (int)base.Size.Width, (int)base.Size.Height);

            #if WINDOWS
            if (position.Contains(ms.X, ms.Y))
                    this.mIsHighlighted = true;
                else
                    this.mIsHighlighted = false;

            if (ms.LeftButton == ButtonState.Pressed && this.mIsHighlighted)
            {
                this.fireClick();
            }
            #endif
        }

        public void HandleKeyboard(KeyboardState curState, KeyboardState prevState)
        {
            if (this.mIsHighlighted && (curState.IsKeyDown(Keys.Enter) && !prevState.IsKeyDown(Keys.Enter)))
                this.fireClick();
        }

        public void LoadContent(GraphicsDeviceManager dMan, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            throw new NotImplementedException();
        }


        public Matrix getView()
        {
            throw new NotImplementedException();
        }

        #region Event Fireres

        private void fireClick()
        {
            if (this.click != null)
                this.click(this, new EventArgs());
        }
        #endregion


    }
}
