﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;


namespace Gravitation.SpriteObjects
{
    class Sprite
    {

        #region member_variables

        public Vector2 Position
        {
            get { return mSpriteBody.Position; }
        }
        public string AssetName;
        public Rectangle Size;
        public bool falling = false;
        public Vector2 CenterPoint
        {
            get
            {
                Vector2 centP = new Vector2();
                centP.X = Position.X + (width / 2);
                centP.Y = Position.Y + (height / 2);
                return centP;
            }
        }
        
        public float height
        {
            get { return Size.Height; }
        }
        public float width
        {
            get { return Size.Width; }
        }
        public Rectangle Source
        {
            get { return mSource; }
            set
            {
                mSource = value;

                Size = new Rectangle(0, 0, (int)(mSource.Width * Scale), (int)(mSource.Height * Scale));

            }
        }
        public float Scale
        {
            get { return mWidthScale; }
            set
            {
                mWidthScale = value;
                mHeightScale = value;
                //Recalculate the Size of the Sprite with the new scale

                Size = new Rectangle(0, 0, (int)(Source.Width * mWidthScale), (int)(Source.Height * mHeightScale));
            }
        }
        public float WidthScale
        {
            get { return mWidthScale; }
            set
            {
                mWidthScale = value;

                //Recalculate the Size of the Sprite with the new scale

                Size = new Rectangle(0, 0, (int)(Source.Width * WidthScale), (int)(Source.Height * HeightScale));
            }
        }
        public float HeightScale
        {
            get { return mHeightScale; }
            set
            {
                mHeightScale = value;

                //Recalculate the Size of the Sprite with the new scale

                Size = new Rectangle(0, 0, (int)(Source.Width * WidthScale), (int)(Source.Height * HeightScale));
            }
        }
        public Body mSpriteBody;

        protected Texture2D mSpriteTexture;
        protected const float MeterInPixels = 64f;

        private float mWidthScale = 1f;
        private float mHeightScale = 1f;
        private Rectangle mSource;
     
        #endregion

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            AssetName = theAssetName;
            Source = new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height);
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * WidthScale), (int)(mSpriteTexture.Height * HeightScale));
        }

        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            Vector2 spritePos = mSpriteBody.Position * MeterInPixels;
            Vector2 spriteOrigin = new Vector2(mSpriteTexture.Width / 2f, mSpriteTexture.Height / 2f);

            theSpriteBatch.Draw(mSpriteTexture, spritePos, Source,
                Color.White, mSpriteBody.Rotation, spriteOrigin,
                new Vector2(WidthScale, HeightScale), SpriteEffects.None, 0f);

            


        }


    }
}







