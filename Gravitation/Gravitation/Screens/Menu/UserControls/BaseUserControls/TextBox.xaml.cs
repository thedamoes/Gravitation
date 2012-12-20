using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Xna.Framework.Input;

namespace Gravitation.Screens.Menu.UserControls.BaseUserControls
{
    /// <summary>
    /// Interaction logic for TextBox.xaml
    /// </summary>
    public partial class TextBox : UserControl
    {

        string var_text1 = "";
        int lastKey = -1;

        public bool Focused
        {
            get { return this.textBox1.IsFocused; }
        }

        public string Text
        {
            set { this.textBox1.Text = value; }
            get { return this.textBox1.Text; }
        }

        public TextBox()
        {
            InitializeComponent();
        }

        public void handleKeyboard(KeyboardState curState)
        {
            if (this.Focused)
            {
                bool lastKeyIsPressed = false;
                foreach (Microsoft.Xna.Framework.Input.Keys a in curState.GetPressedKeys())
                {
                    if (a == Keys.None)
                        continue;

                    if ((int)a != lastKey)
                    {
                        if (IsKeyADigit(a) || a == Keys.OemPeriod)
                        {
                            var_text1 = a.ToString();
                            if (a == Keys.OemPeriod)
                                var_text1 = ".";
                            else if (var_text1.Length < 3)
                                var_text1 = var_text1.Substring(1, 1);
                            else
                                var_text1 = var_text1.Substring(6, 1);

                            this.Text += var_text1;
                            this.textBox1.CaretIndex = this.textBox1.Text.Length;
                            lastKey = (int)a;
                            lastKeyIsPressed = true;
                        }
                    }
                    else
                        lastKeyIsPressed = true;
                }
                if (!lastKeyIsPressed)
                    this.lastKey = -1;

            }
        }

        private bool IsKeyADigit(Keys key)
        {
            return (key >= Keys.D0 && key <= Keys.D9) || (key >= Keys.NumPad0 && key <= Keys.NumPad9);
        }
    }
}
