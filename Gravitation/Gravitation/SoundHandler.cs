using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Gravitation
{
    public class SoundHandler
    {
        public enum Sounds {SHIP_CRASH1 , SHIP_FIRE1, MOVE_MENU};

        private Dictionary<Sounds, String> mAssetNames;
        private Dictionary<Sounds, SoundEffect> mSoundEffects;
        private ContentManager mCm;

        public SoundHandler(ContentManager cm)
        {
            this.mCm = cm;
            mSoundEffects = new Dictionary<Sounds, SoundEffect>();
            mAssetNames = new Dictionary<Sounds, string>();

            mSoundEffects.Add(Sounds.SHIP_CRASH1,null); // load sounds on demand
            mSoundEffects.Add(Sounds.SHIP_FIRE1,null);  // later if somebody can be bothered we could load only the sounds that are needed
            mSoundEffects.Add(Sounds.MOVE_MENU, null); // based on what ships have been chosen the map and so on. because this way will likley give lag.. but we will see

            mAssetNames.Add(Sounds.SHIP_CRASH1, "Sounds/explosion_2");
            mAssetNames.Add(Sounds.SHIP_FIRE1, "Sounds/strela");
            mAssetNames.Add(Sounds.MOVE_MENU, "Sounds/click");

            SoundEffect.MasterVolume = 0.2f;
        }

        public void playSound(Sounds sound) // load sounds on demand
        {
            if (mSoundEffects[sound] != null)
                mSoundEffects[sound].Play();

            else
            {
                mSoundEffects[sound] = mCm.Load<SoundEffect>(mAssetNames[sound]);
                mSoundEffects[sound].Play();
            }

        }
    }
}
