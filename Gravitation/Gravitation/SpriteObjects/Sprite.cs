using System;
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
using FarseerPhysics.DebugViews;

namespace Gravitation.SpriteObjects
{
    public class Sprite
    {

        #region member_variables

        public Vector2 Position
        {
            get
            {
                if (mSpriteBody != null)
                    return mSpriteBody.Position;
                else
                    return mspritePos;
            }
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
        public Vector2 spriteOrigin;
        public List<Vertices> list;


        protected Texture2D mSpriteBodyTexture;
        protected Texture2D mSpriteSheetTexture;

        public int spriteSourceWidth = 0;
        public int spriteSourceHeight = 0;
        public int totalFrames = 0;
        public int framesPerSec = 0;

        private int animationX = 0;
        private int animationY = 0;

        private int time = 0;

        private int frameTracker = 1;

        private bool animationComplete = false;
        
        protected const float MeterInPixels = 64f;

        private float mspriteRotation;
        private Vector2 mspritePos;

        private float mWidthScale = 1f;
        private float mHeightScale = 1f;
        private Rectangle mSource;
     
        #endregion



        public Sprite(Vector2 position, float rotation)
        {
            this.mspritePos = position;
            this.mspriteRotation = rotation;
        }

        public Sprite()
        {

        }

        public virtual void LoadContent(ContentManager theContentManager, string bodyAssetName, string spriteSheetAssetName)
        {
            mSpriteBodyTexture = theContentManager.Load<Texture2D>(bodyAssetName);
            mSpriteSheetTexture = theContentManager.Load<Texture2D>(spriteSheetAssetName);

            AssetName = bodyAssetName;
            Source = new Rectangle(0, 0, mSpriteBodyTexture.Width, mSpriteBodyTexture.Height);
            Size = new Rectangle(0, 0, (int)(mSpriteBodyTexture.Width * WidthScale), (int)(mSpriteBodyTexture.Height * HeightScale));


        }


        public void setSpriteDrawn(int x, int y) //manually set a specific frame of the sprite sheet to be drawn to the screen //Note. should NOT be used for animations
        {
            Source = new Rectangle(x, y, mSpriteBodyTexture.Width, mSpriteBodyTexture.Height);
        }

        public void setAnimationOrigin(int x, int y) //Sets the Source rectangle to a specific row of the sprite sheet, so that updateAnimation will "Play" that row.
        {
            animationX = x;
            animationY = y;
            animationComplete = false;
        }

        public void setFramesPerSec(int fps)
        {
            framesPerSec = fps;
        }

        public void setTotalFrames(int totalFrames)
        {
            this.totalFrames = totalFrames;
        }

        public void setSpriteSourceHeightAndWidth(int height, int width)
        {
            spriteSourceHeight = height;
            spriteSourceWidth = width;
        }

        public void playAnimation(GameTime gameTime, bool loop) //Cycles through a row of an animation multiple times
        {

            time += gameTime.ElapsedGameTime.Milliseconds;

            if (time >= 1000 / framesPerSec)
            {

                time = 0;
                Source = new Rectangle(animationX + (frameTracker * spriteSourceWidth), animationY, spriteSourceWidth, spriteSourceHeight);

                if (loop)
                {
                    if (frameTracker < totalFrames-1)
                    {
                        frameTracker++;
                    }
                    else
                    {
                        frameTracker = 0;
                    }
                }
                else
                {
                    if (frameTracker < totalFrames-1 && !animationComplete)
                    {
                        frameTracker++;
                    }
                    else
                    {
                        animationComplete = true;
                        frameTracker = 0;
                    }
                }
            }

        }

        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            if (mSpriteBody != null)
            {
                mspritePos = mSpriteBody.Position * MeterInPixels;
                mspriteRotation = mSpriteBody.Rotation;
            }


            theSpriteBatch.Draw(mSpriteSheetTexture, mspritePos, Source,
                Color.White, mspriteRotation , spriteOrigin,
                new Vector2(mWidthScale, mHeightScale), SpriteEffects.None, 0f);

        }


        public virtual void Draw(SpriteBatch theSpriteBatch, DebugViewXNA debugView, Matrix projection, Matrix view)
        {
            if (mSpriteBody != null)
            {
                mspritePos = mSpriteBody.Position * MeterInPixels;
                mspriteRotation = mSpriteBody.Rotation;
            }


            theSpriteBatch.Draw(mSpriteSheetTexture, mspritePos, Source,
                Color.White, mspriteRotation, spriteOrigin,
                new Vector2(mWidthScale, mHeightScale), SpriteEffects.None, 0f);

        }

    }
}







