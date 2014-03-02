using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.DebugViews;
using FarseerPhysics.Dynamics;
using Gravitation.AIEngine.AIBehaviours;

namespace Gravitation.AIEngine
{
    /**
     * Behavoirs will be kept as 1 class, 
     * they will be expected to implement the getDirection and getRotation methods
     * as well as the performAction method, even if there is no action to be performed, 
     * or there is no movement alterations to be performed.
     * 
     * This decision has been made due to the fact that a large majority of 
     * the time we wish to fire (in the more complex behaviours) 
     * we will also need to alter (at least) the rotation to track in some manner.
     * Seperating out specific movement and fire behaviours seemed an unecessary complication
     * since the two are so closely linked.
     * 
     */
    public enum behaviour
    {
        SeekPlayer, // move to player position
        AvoidObsticles,
        FireOnSight,
        Wander, // random wander behaviour
        SeekUpgrades,
        FlankPlayer, // tries to be behind player at all times
        Hide // tries to keep obsticles between player and it at all times  
    };

    public class AIEngine
    {
        private float DIRECTION_WEIGHT;
        private float ROTATION_WEIGHT;
        private List<Behaviour> enabledBehaviours = new List<Behaviour>(); //list of Behaviour object instances, which will be instanciated in constructor
        private Vector2 mDirection = new Vector2(0,0);
        private float mRotation;

        public Vector2 Direction
        {
            get { return this.mDirection;}
        }

        public float Rotation
        {
            get { return this.mRotation; }
        }

        public AIEngine(float directionWeight, float rotationWeight, List<behaviour> behaviours)
        {
            this.DIRECTION_WEIGHT = directionWeight;
            this.ROTATION_WEIGHT = rotationWeight;
            foreach(behaviour b in behaviours) {
                switch (b) {
                    case behaviour.SeekPlayer : {
                        // Add Instance
                        //SeekPlayer sp = new SeekPlayer();
                        //enabledBehaviours.Add(sp);
                        break;
                    }
                    case behaviour.AvoidObsticles:
                        {
                            // Add Instance
                            AvoidObsticles ao = new AvoidObsticles();
                            enabledBehaviours.Add(ao);
                            break;
                        }
                    case behaviour.FireOnSight:
                        {
                            // Add Instance
                            //FireOnSight fos = new FireOnSight();
                            //enabledBehaviours.Add(fos);
                            break;
                        }
                    case behaviour.Wander:
                        {
                            // Add Instance
                            break;
                        }
                    case behaviour.SeekUpgrades:
                        {
                            // Add Instance
                            break;
                        }
                    case behaviour.Hide:
                        {
                            // Add Instance
                            break;
                        }
                    case behaviour.FlankPlayer:
                        {
                            // Add Instance
                            break;
                        }
                }
            }

        }

        public void calculate(SpriteObjects.Ship mShip, Body otherShipsBody)
        {
            //loop round stores values for behaviours allowing higher weighted values
            //to be used first.
            Dictionary<int, float> possibleRotationVals = new Dictionary<int, float>();
            Dictionary<int, Vector2> possibleDirectionVals = new Dictionary<int, Vector2>();
            foreach (Behaviour b in enabledBehaviours) { //make sure behaviours are instantiated with their weights

                Vector2 thrust = b.getDirection(mShip, otherShipsBody, DIRECTION_WEIGHT);
                if(thrust.X == 0) {
                    thrust = (thrust.Y == 0) ? Vector2.Zero : mDirection + thrust;
                    possibleDirectionVals.Add(b.getWeight(), thrust);
                }

                float rotation = b.getRotation(mShip, otherShipsBody, ROTATION_WEIGHT);
                possibleRotationVals.Add(b.getWeight(), rotation);
                b.performAction(mShip, otherShipsBody);
            }

            foreach(int weight in possibleDirectionVals.Keys.OrderByDescending(x => x)) {
                if (possibleDirectionVals[weight].Y > 0)
                {
                    continue;
                }
                mDirection = possibleDirectionVals[weight];
                break;
            }

            foreach (int weight in possibleRotationVals.Keys.OrderByDescending(x => x))
            {
                if (possibleRotationVals[weight] == 99)
                {
                    continue;
                }
                mRotation = possibleRotationVals[weight];
                break;
            }

            if(mRotation == 0) {
                mShip.mSpriteBody.AngularVelocity = 0;
            }

            //possibly limit direction and rotation here back down to normal levels.

        }

        public void reset()
        {
            mDirection = new Vector2(0, 0);
            mRotation = 0f;
        }

    }
}
