using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Gravitation.SpriteObjects.HUD
{
    public abstract class HUDItem : Sprite
    {
        public HUDItem(Vector2 position) : base(position,0)
        {

        }
        public abstract void LoadContent(ContentManager theContentManager);


    }
}
