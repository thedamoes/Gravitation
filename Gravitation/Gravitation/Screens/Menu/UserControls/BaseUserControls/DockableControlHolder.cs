using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Integration;
using Microsoft.Xna.Framework.Input;

namespace Gravitation.Screens.Menu.UserControls.BaseUserControls
{
    class DockableControlHolder
    {
        public DOCKSTATES Dock
        {
            get { return this.dock; }
            set { 
                    this.dock = value;
                    this.updateLayout();  
                }
        }
        
        public enum DOCKSTATES
        {
            FILL,
            LEFT,
            RIGHT,
            TOP,
            BOTTOM
        }

        private int windowHeight;
        private int windowWidth;

        private System.Windows.UIElement control;
        private System.Drawing.Point pos;
        private System.Drawing.Size size;
        private ElementHost host;

        private DOCKSTATES dock = DOCKSTATES.FILL;

        public DockableControlHolder(System.Windows.UIElement control, int windowWidth, int windowHeight)
        {
            this.control = control;

            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;

            this.size = new System.Drawing.Size(windowWidth - 60, windowHeight - 260);
            this.pos = new System.Drawing.Point(30, 30);
        }

        public DockableControlHolder(System.Windows.UIElement control, int windowWidth, int windowHeight, int width, int height) 
            : this(control,windowWidth,windowHeight)
        {
            this.size = new System.Drawing.Size(width, height);
        }

        public void createControl()
        {
            host = new ElementHost();
            host.Location = this.pos;
            host.Name = "elementHost1";
            host.Size = this.size;
            host.TabIndex = 1;
            host.Text = "elementHost1";

            host.Child = control;
            System.Windows.Forms.Form.FromHandle(Mouse.WindowHandle).Controls.Add(host);
        }

        public void destroyControl()
        {
            System.Windows.Forms.Form.FromHandle(Mouse.WindowHandle).Controls.Remove(host);
            this.host = null;
        }

        private void updateLayout()
        {

        }
    }
}
