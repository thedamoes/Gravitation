using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Gravitation.SpriteObjects.HUD
{
    public class LifeBar : HUDItem
    {
        private IHasLife _hasLife;

        public LifeBar(IHasLife hasLife, Vector2 position) : base(position)
        {
            this._hasLife = hasLife;

        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, "lifeBar", "lifeBar");
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            base.WidthScale = _hasLife.HealthValue * 10;
            base.Draw(theSpriteBatch);
        }


    }
}
