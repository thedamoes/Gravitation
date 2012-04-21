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

namespace TestXnaGUILib
{
    /// <summary>
    /// ToolWindow is an example 
    /// </summary>
    public class ShipSettings : XGControl
    {
        //public Game1 Game;
        public XGTabControl TabControl { get; protected set; }
        public XGTabPage PageOne { get; protected set; }

        public ShipSettings(int screenWidth, int screenHeight)
            : base(new Rectangle(screenWidth-200, 2, 500, 300), true)
        {
           // Game = game;

            // Add our window to the Controls to be managed by the XNA GUI manager

            XnaGUIManager.Controls.Add(this);

            // Offset the Tab Control and Tab Page rectangles relative to this Tool Window 

            Rectangle pageRect = this.Rectangle; // tab pages are the size of the tab control
            pageRect.X = pageRect.Y = 0;  // but use parent relative coordintates

            // Create the Tab Control

            TabControl = new XGTabControl(pageRect);
            Children.Add(TabControl); // add the tab control to our child control list

            // Create the first tab page (ToolPage class)

            XGTabPage PageOne = new ToolPage(pageRect);
            TabControl.Children.Add(PageOne);
        }
    }

    /// <summary>
    /// ToolPage is an XGTabPage that displays various controls
    /// </summary>
    public class ToolPage : XGTabPage
    {
       // public Game1 Game;
        public  XGHSlider SheildStrength { get; protected set; }
        public XGLabel SheildLable { get; protected set; }
        public XGLabel SpeedLable { get; protected set; }

        public XGHSlider FirePowerSlider { get; protected set; }
        public XGLabel FireSpeed { get; protected set; }
        public XGLabel FirePower { get; protected set; }

        public XGButton okButton { get; protected set; }

        public XGLabel selectSprite { get; protected set; }

        private const int Y_INCRMENT = 30;

        public ToolPage(Rectangle rect)
            : base(rect, "Ship Settings")
        {
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

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        void Ok_Clicked(XGControl sender)
        {
            
        }

       
    }
}
