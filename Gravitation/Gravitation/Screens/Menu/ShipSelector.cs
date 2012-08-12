using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using XnaGUILib;
using Gravitation;

namespace Gravitation.Screens.Menu
{
    /// <summary>
    /// ToolWindow is an example 
    /// </summary>
    public class ShipSettings : XGControl
    {
        //public Game1 Game;
        public XGTabControl TabControl { get; protected set; }

        public event EventHandler<EventArgs> okClicked;
        private SoundHandler handler;
        private XGTabPage[] playerTabs;
        private XGButton okBttn;

        public ShipSettings(int screenWidth, int screenHeight, SoundHandler hanlder, int numOfPlayers)
            : base(new Rectangle(screenWidth-500, 2, 500, 300), true)
        {
            this.handler = hanlder;

            XnaGUIManager.Controls.Add(this);

            Rectangle pageRect = this.Rectangle;
            pageRect.X = pageRect.Y = 0;

            TabControl = new XGTabControl(pageRect);
            Children.Add(TabControl);

            playerTabs = new XGTabPage[numOfPlayers];
            for (int i = 0; i < numOfPlayers; i++)
            {
                playerTabs[i] = new ToolPage(pageRect, handler);
                ((ToolPage)playerTabs[i]).okClicked += this.okClick;
                TabControl.Children.Add(playerTabs[i]);
            }

            
        }

        public DataClasses.ShipConfiguration[] getShips()
        {
            DataClasses.ShipConfiguration[] ships = new DataClasses.ShipConfiguration[this.playerTabs.Count()];
            for(int i =0; i <this.playerTabs.Count(); i++)
            {
                ToolPage page = (ToolPage)this.playerTabs[i];
                DataClasses.ShipConfiguration config = page.ShipConfiguration;
                ships[i] = config;
            }
            return ships;
        }

        private void okClick(object sender, EventArgs e) 
        {
            if (this.okClicked != null) 
                this.okClicked(this, new EventArgs());
        }
    }

    public class ToolPage : XGTabPage
    {
        public  XGHSlider SheildStrength { get; protected set; }
        public XGLabel SheildLable { get; protected set; }
        public XGLabel SpeedLable { get; protected set; }

        public XGHSlider FirePowerSlider { get; protected set; }
        public XGLabel FireSpeed { get; protected set; }
        public XGLabel FirePower { get; protected set; }
        public XGButton okButton { get; protected set; }
        public XGLabel selectSprite { get; protected set; }
        public event EventHandler<EventArgs> okClicked;
        public DataClasses.ShipConfiguration ShipConfiguration
        {
            get
            {
                return new Gravitation.DataClasses.ShipConfiguration(new Gravitation.SpriteObjects.Ship(this.handler, this.FirePowerSlider.Value, this.SheildStrength.Value));
            }
        }


        private const int Y_INCRMENT = 30;
        private SoundHandler handler;

        public ToolPage(Rectangle rect, SoundHandler handler)
            : base(rect, "Ship Settings")
        {
            this.handler = handler;
            // stupid c# isent letting me pass these by refence so i have to do it lik ethis
            int currenty = 66;

            SheildStrength = new XGHSlider(new Rectangle(130, currenty, 200, 20), 5f, 10f);
            SheildLable = new XGLabel(new Rectangle(0, currenty, 10, 20), "Sheild Strength");
            SpeedLable = new XGLabel(new Rectangle(350, currenty, 10, 20), "Speed");

            Children.Add(SheildStrength);
            Children.Add(SheildLable);
            Children.Add(SpeedLable);

            currenty += Y_INCRMENT;

            FirePowerSlider = new XGHSlider(new Rectangle(130, currenty, 200, 20), 5f, 10f);
            FireSpeed = new XGLabel(new Rectangle(0, currenty, 10, 20), "Fire Speed");
            FirePower = new XGLabel(new Rectangle(350, currenty, 10, 20), "Fire Power");

            Children.Add(FirePowerSlider);
            Children.Add(FireSpeed);
            Children.Add(FirePower);

            currenty += Y_INCRMENT;

            selectSprite = new XGLabel(new Rectangle(0, currenty, 10, 20), "Ship: ");
            Children.Add(selectSprite);

            currenty += 100;
            okButton = new XGButton(new Rectangle(350, currenty, 100, 20), "Ok", this.Ok_Clicked);
            Children.Add(okButton);

        }

        void Ok_Clicked(XGControl sender)
        {
            if (okClicked != null)
                okClicked(this, new EventArgs());
        }


       
    }
}
