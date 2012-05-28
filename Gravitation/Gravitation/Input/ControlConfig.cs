using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Gravitation.Input
{
    public class ControlConfig
    {
        public delegate void handelKey();

        private Dictionary<Keys, handelKey> mIsDownKeyMaps;
        private Dictionary<Keys, handelKey> mIsUpAndWasDownKeyMaps;

        public ControlConfig()
        {
            mIsDownKeyMaps = new Dictionary<Keys, handelKey>();
            mIsUpAndWasDownKeyMaps = new Dictionary<Keys, handelKey>();
        }

        public void registerIsNownKey(Keys key,handelKey func)
        {
            this.mIsDownKeyMaps.Add(key, func);
        }

        public void registerIsUpAndWasDown(Keys key, handelKey func)
        {
            this.mIsUpAndWasDownKeyMaps.Add(key, func);
        }

        public void actionKeys(KeyboardState state, KeyboardState prevState)
        {
            Dictionary<Keys, handelKey>.Enumerator isDownEnum = mIsDownKeyMaps.GetEnumerator();
            Dictionary<Keys, handelKey>.Enumerator isUpAndWasDownEnum = mIsUpAndWasDownKeyMaps.GetEnumerator();

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

        }
    }
}
