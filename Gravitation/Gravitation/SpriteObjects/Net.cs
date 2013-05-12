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
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PhysicsLogic;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.DebugViews;

namespace Gravitation.SpriteObjects
{

    /*
        Notes about the Net class:
     * 
     * > A Net object is made of multiple bodies
     * when one link signals a collision each link has
     * a RopeJoint created between it and the center
     * of the object the link collided with.
     * 
     * > Net has a "Health" of sorts such that 
     * when the overall damage done to the net
     * reaches the threshold then the net will 
     * be destroyed.
     * 
     * > The Net will transfer half of damage
     * done to it to the player it is connected to,
     * the other half will be taken from the Net's
     * health. (This ensures that Nets don't make
     * players invincible).
     * 
     * 
     * 
     * > Note: need to be sure that a collision on
     * ANY of the bodies that makes up the Net will
     * trigger the "On_Collision" method
     * 
     * 
     * 
     */

    public class Net : AlternateShot
    {
        public int netHealth = 100;

        private int trueLinkCount = 8; 

        private int linkIndexShift = 3; //number of links to shift to the left of the start position, to centre the total net.

        private int finalLinkIndex;

        private List<NetLink> netlinks = new List<NetLink>();

        public bool collided = false;

        private int time = 0;

        private int netLifetime = 30;  // time net stays on screen (in seconds)

        public Net(World world, Vector2 position, float rotation, DestroyMeHandler onDeathHandler)
        {
            base.mworld = world;

            base.mrotation = rotation;

            base.ondeathHandler = onDeathHandler;

            finalLinkIndex = trueLinkCount - linkIndexShift;

            Vector2 firstLinkPos = rotateVector(new Vector2(0.35f, 0), mrotation); //0.4

            for (int x = -linkIndexShift; x < finalLinkIndex; x++)
            {
                SpriteObjects.NetLink link = new NetLink(mworld, position+(x*firstLinkPos), mrotation, ondeathHandler); //change rotation
                netlinks.Add(link);
            }

        }

        public void LoadContent(ContentManager theContentManager, GraphicsDeviceManager graphics)
        {
            // Load each links content
    
            // then create rotational joints to turn into a net

            
            List<Body> linkBodies = new List<Body>();

            foreach (NetLink link in netlinks)
            {
                link.LoadContent(theContentManager, graphics);

                linkBodies.Add(link.mSpriteBody);
            }

            Body prevBody = null;
            foreach (Body link in linkBodies)
            {
                if (prevBody != null)
                {
                    Vector2 anchor = new Vector2(-0.25f, 0.0f); //0.3
                    RevoluteJoint jd = new RevoluteJoint(prevBody, link,
                                                         prevBody.GetLocalPoint(link.GetWorldPoint(anchor)), anchor);
                    jd.CollideConnected = true;
                    mworld.AddJoint(jd);

                }

                prevBody = link;
            }

        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            // cycle through each link calling draw
            foreach (NetLink link in netlinks)
            {
                link.Draw(theSpriteBatch);
            }

        }

        public void altFire(Vector2 theStartPosition, float rotation, float shotSpeed)
        {
            base.Visible = true;
            foreach (NetLink link in netlinks)
            {
                link.fire(link.mposition, rotation, shotSpeed); 
            }
        }


        public override void Update(GameTime gameTime, Matrix _view)
        {
            time += gameTime.ElapsedGameTime.Milliseconds;

            foreach (NetLink link in netlinks)
            {
                if (collided && !link.contactMade)
                {
                    link.contactMade = true;
                }

                if (link.contactMade && !collided)
                {
                    collided = true;
                }
                else
                {
                    if(link.damageTaken > 0 && netHealth > 0)
                    {
                        netHealth = (netHealth < (link.damageTaken / 2)) ? 0 : netHealth - (link.damageTaken);
                        link.damageTaken = 0;
                    }

                    link.Update(gameTime, _view);
                }
            }

                if (netHealth == 0 || time >= (netLifetime * 1000))
            {
                base.Visible = false;
                removeNet();
            }

        }


        public void removeNet()
        {
            if (!this.removed)
            {

                //for each NetLink
                foreach (NetLink link in netlinks)
                {
                    link.removeObject();
                }

                this.removed = true;
                this.ondeathHandler.Invoke(this); 
            }
        }


        private Vector2 rotateVector(Vector2 direction, float angle)
        {
            Vector2 newvec = new Vector2();

            newvec.X = (float)((Math.Cos(angle) * direction.X) - (Math.Sin(angle) * direction.Y));
            newvec.Y = (float)((Math.Sin(angle) * direction.X) + (Math.Cos(angle) * direction.Y));

            return newvec;
        }

    }

}
