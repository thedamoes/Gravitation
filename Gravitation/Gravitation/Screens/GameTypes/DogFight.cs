using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Gravitation.Screens.GameTypes
{
    class DogFight : TwoPlayerBaseGame
    {

        
        // game content
        
        
        private SpriteObjects.Upgrade u;
        private List<String> powerups = new List<string>();

        private GameTime mgameTime;

        public DogFight(DataClasses.GameConfiguration gameConfig): base(gameConfig)
        {
            powerups.Add("f");
        }

        public override void LoadContent(GraphicsDeviceManager graphics, ContentManager Content)
        {
            base.LoadContent(graphics, Content);
            u = new SpriteObjects.Upgrade(base.mWorld, mMapLoader.getPowerupSpawns, powerups);

            u.LoadContent(Content, graphics);
        }

        public override void Update(GameTime gameTime)
        {
            mgameTime = gameTime;
            u.Update(gameTime, cam.View);
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, GameTime gameTime)
        {
            u.Draw(sb);
            base.Draw(sb, gameTime);
        }

        public override void HandleKeyboard(KeyboardState state, KeyboardState prevState)
        {
            base.HandleKeyboard(state,prevState);
        }



        public override Matrix getView()
        {
            return cam.View;
        }
    }
}
