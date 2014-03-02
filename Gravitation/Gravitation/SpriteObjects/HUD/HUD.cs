using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravitation.SpriteObjects.HUD
{
    public class HUD : Sprite
    {
        private List<HUDItem> _huddItems = new List<HUDItem>();

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch theSpriteBatch)
        {
            foreach (HUDItem hudDprite in _huddItems)
                hudDprite.Draw(theSpriteBatch);
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager theContentManager)
        {
            foreach (HUDItem hudDprite in _huddItems)
                hudDprite.LoadContent(theContentManager);
        }

        public void AddHUDDObject(HUDItem huddObject)
        {
            _huddItems.Add(huddObject);
        }
    }
}
