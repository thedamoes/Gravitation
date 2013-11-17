using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;

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

            powerups.Add("shield");
            powerups.Add("Shotgun");
            powerups.Add("Machinegun");
            powerups.Add("Spiral");
            powerups.Add("Emp");
            powerups.Add("Net");
        }

        public override void LoadContent(GraphicsDeviceManager graphics, ContentManager Content)
        {
            base.LoadContent(graphics, Content);

            int exampleUpgradeTime = 10; //seconds
            u = new SpriteObjects.Upgrade(base.mWorld, mMapLoader.getPowerupSpawns, powerups, exampleUpgradeTime);
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
            base.Draw(sb, gameTime);
            u.Draw(sb);
        }

        public override void HandleKeyboard(KeyboardState state, KeyboardState prevState)
        {
            base.HandleKeyboard(state,prevState);
        }

        public override Matrix getView()
        {
            return base.getView();
        }
    }
}
