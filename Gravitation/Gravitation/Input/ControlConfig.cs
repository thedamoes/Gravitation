using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Gravitation.Input
{
    public class XBOXControllerAnalog
    {
       public enum AXIS
       {
           X_AXIS,
           Y_AXIS
       }

       public enum SICK
       {
           LEFT,
           RIGHT
       }

       private AXIS axis;
       private SICK stick;

       public XBOXControllerAnalog(SICK thumbStick, AXIS axis)
       {
           this.axis = axis;
           this.stick = thumbStick;
       }

       public float getValue()
       {
           Vector2 axises;

           if (this.stick == SICK.LEFT)
               axises = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One, GamePadDeadZone.Circular).ThumbSticks.Left;
           else
               axises = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One, GamePadDeadZone.Circular).ThumbSticks.Right;

           if (this.axis == AXIS.X_AXIS)
               return axises.X;
           else
               return axises.Y;

       }

    }
    public class ControlConfig
    {
        public delegate void handelKey();
        public delegate void handelAnalog(float value);

        private Dictionary<Keys, handelKey> mIsDownKeyMaps;
        private Dictionary<Keys, handelKey> mIsUpAndWasDownKeyMaps;
        private Dictionary<Buttons, handelKey> mXBOXButtonPress;
        private Dictionary<Buttons, handelKey> mXBOXisPressedAndWasNot;

        private Dictionary<XBOXControllerAnalog, handelAnalog> mXBOXAnalogPress;

        private GamePadState prevPadState;

        public ControlConfig()
        {
            mIsDownKeyMaps = new Dictionary<Keys, handelKey>();
            mIsUpAndWasDownKeyMaps = new Dictionary<Keys, handelKey>();
            mXBOXButtonPress = new Dictionary<Buttons, handelKey>();
            mXBOXisPressedAndWasNot = new Dictionary<Buttons, handelKey>();
            mXBOXAnalogPress = new Dictionary<XBOXControllerAnalog, handelAnalog>();
            prevPadState = new GamePadState();
           
        }

        public void registerIsNownKey(Keys key,handelKey func)
        {
            this.mIsDownKeyMaps.Add(key, func);
        }

        public void registerIsUpAndWasDown(Keys key, handelKey func)
        {
            this.mIsUpAndWasDownKeyMaps.Add(key, func);
        }

        public void registerXBOXButtonPress(Buttons button, handelKey func)
        {
            this.mXBOXButtonPress.Add(button, func);
        }

        public void registerXBOXButtonIsDownAndWasUp(Buttons button, handelKey func)
        {
            this.mXBOXisPressedAndWasNot.Add(button, func);
        }

        public void registerXBOXButtonPress(XBOXControllerAnalog button, handelAnalog func)
        {
            this.mXBOXAnalogPress.Add(button, func);
        }

        public void actionKeys(KeyboardState state, KeyboardState prevState)
        {
            Dictionary<Keys, handelKey>.Enumerator isDownEnum = mIsDownKeyMaps.GetEnumerator();
            Dictionary<Keys, handelKey>.Enumerator isUpAndWasDownEnum = mIsUpAndWasDownKeyMaps.GetEnumerator();
            Dictionary<Buttons, handelKey>.Enumerator XBOXButtonPress = mXBOXButtonPress.GetEnumerator();
            Dictionary<Buttons, handelKey>.Enumerator XBOXButtonPressAndWasNot = mXBOXisPressedAndWasNot.GetEnumerator();
            Dictionary<XBOXControllerAnalog, handelAnalog>.Enumerator XBOXAnalogPress = mXBOXAnalogPress.GetEnumerator();
            

            while (isDownEnum.MoveNext())
            {
                KeyValuePair<Keys, handelKey> keymap = isDownEnum.Current;
                if (state.IsKeyDown(keymap.Key))
                    keymap.Value();
            }

            while (isUpAndWasDownEnum.MoveNext())
            {
                KeyValuePair<Keys, handelKey> keymap = isUpAndWasDownEnum.Current;
                if (state.IsKeyUp(keymap.Key) && prevState.IsKeyDown(keymap.Key))
                    keymap.Value();
            }

            while (XBOXButtonPress.MoveNext())
            {
                KeyValuePair<Buttons, handelKey> keymap = XBOXButtonPress.Current;
                
                if (GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One,GamePadDeadZone.Circular).IsButtonDown(keymap.Key))
                {
                    keymap.Value();
                }
            }

            while (XBOXButtonPressAndWasNot.MoveNext())
            {
                KeyValuePair<Buttons, handelKey> keymap = XBOXButtonPressAndWasNot.Current;

                if (!GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One, GamePadDeadZone.Circular).IsButtonDown(keymap.Key) &&
                        this.prevPadState.IsButtonDown(keymap.Key))
                {
                    keymap.Value();
                }
            }

            while (XBOXAnalogPress.MoveNext())
            {
                KeyValuePair<XBOXControllerAnalog, handelAnalog> keymap = XBOXAnalogPress.Current;
                keymap.Value(keymap.Key.getValue());
            }

            this.prevPadState = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One, GamePadDeadZone.Circular);
        }
    }
}
