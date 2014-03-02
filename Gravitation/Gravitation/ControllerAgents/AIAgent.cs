using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using FarseerPhysics.DebugViews;
using FarseerPhysics.Dynamics;
using Gravitation.AIEngine;
using Gravitation.AIEngine.AIBehaviours;
using Gravitation.Utils;

namespace Gravitation.ControllerAgents
{
    public class AIAgent : IControllerInterface
    {
        #region members

        public SpriteObjects.Ship mShip;
        private Vector2 mDirection; // X = unless you want weard stuff to happen this should be 0 (could have a powerup that sets this value to non 0)
        // Y = how mutch forward

        private float mRotatation = 0;
        private World mWorld;

        private const float DIRECTION_WEIGHT = 0.05f; //2.5
        private const float ROTATION_WEIGHT = 0.5f;

        public Vector2 myPosition
        {
            get { return mShip.ShipPosition; }
        }
        public float Rotation
        {
            get { return this.mRotatation; }
        }

        private ContentManager cm;

        private Body otherShipBody;

        private AIEngine.AIEngine ai;

        #endregion

        public AIAgent(SpriteObjects.Ship ship, World world)
        {
            this.mShip = ship;
            this.mDirection = new Vector2(0, 0);
            this.mWorld = world;

            List<behaviour> behaviours = new List<behaviour>();
            //behaviours.Add(behaviour.SeekPlayer);
            //behaviours.Add(behaviour.FireOnSight);
            //behaviours.Add(behaviour.AvoidObsticles);
            ai = new AIEngine.AIEngine(DIRECTION_WEIGHT, ROTATION_WEIGHT, behaviours);
        }


        #region IControlInterfaceMembers

        public void applyMovement()
        {
        /*
         * TODO: for this class
         * > Have "SinglePlayer" pass in the world (retrievable through the Ship)
         * 
         * > Use what's required from world and pass it to the AI behaviour engine.
         * 
         * > The AI engine will influence mRotation and mDirection in a similar way to the player input handlers.
         * 
         * > Will need boolean "instructions" to be passed back to the AIAgent as well as simple
         *  directional input, as the fire() and altFire() functions need to be called somehow. 
         *  will monitor current AI state instructions by calling a check constantly within the 
         *  "Update()" function of the SinglePlayer Screen, (or whatever screen we happen to want AI on in future).  
         * 
         * 
         * 
         * 
         * AI ENGINE:
         * 
         * > Multi layered Behavioir state machine.
         * 
         * > Input will be from the "world" passed in from the ship
         * 
         * > Will also take input from the Ship itself in the form of:
         *         -Health reading
         *         -Distance Sensors (obstical detection aides)
         *         -Current Statuses (weapon, passive, negative etc.)
         *         -Current Alt Shot status (count, Type)
         *         -Current Speed and Direction
         * 
         * > It then passes the input through the current active behaviour states
         * which each influence the current roational and directional vectors differently.
         * 
         * > Each of these behaviours should be easily "layerable" on top of a given previous behaviour.
         * Also each of the behaviours will have a dynamic "weight" associated with it.
         * 
         * > This weight will effect which outputs are more prominent when the final directional and rotational 
         * outputs are normalised and added together. 
         * 
         * > The weights and active behaviours should be dynamically altered by the current status of
         *      the game. (such as how much health either player has, or how many times they've died. etc.)
         *      
         * 
         * > Behaviours will be variations of an abstract "Behaviour" class.
         * 
         * > Basic starting behaviours should be [Obsticle Avoidance, Wandering, PlayerSeeking, FireOnSight]
         * 
         * > More advanced behaviours will be added in later, however these basic behaviours should be MORE than
         *    enough to include along with the implementation of the enrire new AI framework.
         *    
         * > Input for Obsticle Avoidance will mainly be retireved from "Sensors" attatched to the Ship object.
         * 
         * > Sensors will function as follows: either 
         *      -have a forward Senor object which has a "Body"
         *      then upon a "collision" get the point of collision and raycast from the outer edges of the Ship
         *      and have those values infuence the strength and direction of the turn.
         *  
         *      Or
         * 
         *      -have the Ship outputting a constant optimal-spread of Raycasts (optimal to be discovered later)
         *      and use any fluxuation in those values to inluence the turn of the Ship.
         *      
         * Would rather use the second method as it seems simpler, however not sure about the performance impact
         * of constant raystreams. 
         * 
         * 
         * > Increase and decrease the "size" of the rayCasts/Sensors depending on the current speed of the Ship.
         * 
         * 
         * > Wandering will be achieved by using the same style of wandering dictated in the notes from peter king.
         * 
         * > Basic Player Seeking will use input from the "world" object to get the player's exact xy location
         *   then simply try to orientate the Ship towards them. (by default, this should be attempting to engage thrust with 
         *   other behaviours disengaging if necessary)
         * 
         * 
         */

            //AIEngine.calculate(mShip, otherShipBody);
            //mDirection = AIEngine.getDirection();
            //mRotatation = AIEngine.AIEngine();
            ai.calculate(mShip, otherShipBody);
            mDirection += ai.Direction;
            mRotatation = ai.Rotation;

            if (mDirection == Vector2.Zero && mRotatation == 0) // optimisation
            {
                return;
            }

            float shipAngle;


            //following if satement is to limit the rotation velocity so we don't spin LIGHT SPPPEEEED
            if (mShip.mSpriteBody.AngularVelocity > 5)
            {
                mShip.mSpriteBody.AngularVelocity = 5;
                //do nothing
            }
            else if (mShip.mSpriteBody.AngularVelocity < -5)
            {
                mShip.mSpriteBody.AngularVelocity = -5;
                //do nothing
            }
            else
            {
                mShip.mSpriteBody.ApplyTorque(mRotatation);
            }

            shipAngle = mShip.mSpriteBody.Rotation;


            float Yvel = mShip.mSpriteBody.LinearVelocity.Y;
            float Xvel = mShip.mSpriteBody.LinearVelocity.X;

            int posSpd = (int)Math.Max(Math.Sqrt(Yvel * Yvel), Math.Sqrt(Xvel * Xvel));

            int totalSpeed = posSpd;
            int speedLimit = 50;//10

            float excessX = 0;
            float excessY = 0;

            if (Xvel > speedLimit)
            {
                excessX = Xvel - speedLimit;
            }
            else if (Xvel < -speedLimit)
            {
                excessX = Xvel + speedLimit;
            }

            if (Yvel > speedLimit)
            {
                excessY = Yvel - speedLimit;
            }
            else if (Yvel < -speedLimit)
            {
                excessY = Yvel + speedLimit;
            }

            if (totalSpeed > speedLimit) //15
            {
                mShip.mSpriteBody.ApplyLinearImpulse(new Vector2(-excessX, -excessY));
            }
            else
            {
                if (mShip.currentNegativeState != SpriteObjects.Ship.negativeState.Emped)
                {
                    mShip.mSpriteBody.ApplyForce(GravitationUtils.rotateVector(mDirection, shipAngle));
                }
            }
            resetParams();
            
        }

        public void updateShip(GameTime gameTime, Matrix _view)
        {
            if (otherShipBody == null)
            {
                foreach (Body mBody in mWorld.BodyList)
                {
                    if (Convert.ToString(mBody.FixtureList[0].UserData).StartsWith("ship"))
                    {
                        String otherShipId = Convert.ToString(mBody.FixtureList[0].UserData).Split(':')[1];

                        if (!otherShipId.Equals(this.mShip.ShipId))
                        {
                            otherShipBody = mBody;
                        }

                    }
                }
            }

            mShip.updateShot(gameTime, _view);
            mShip.updateAltShot(gameTime, _view);
            mShip.thrust(gameTime, _view);
            mShip.updatePassiveShipState(gameTime, _view);
            mShip.updateNegativeShipState(gameTime, _view);
        }

        public void loadShip(Microsoft.Xna.Framework.Content.ContentManager cm, Microsoft.Xna.Framework.GraphicsDeviceManager graphics)
        {
            this.cm = cm;

            mShip.LoadContent(cm, "Ship", graphics);
            mShip.isAIControlled = true;
        }


        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sBatch, DebugViewXNA debugView, Matrix projection, Matrix view)
        {
            mShip.Draw(sBatch, debugView, projection, view);
        }

        #endregion

        public void reset2(Vector2 spawnpoint)
        {
            mShip.mSpriteBody.Position = (spawnpoint * 0.015625f); //FIGURE OUT WHY THIS IS!!
            mShip.mSpriteBody.Rotation = 0;
            mShip.mSpriteBody.AngularVelocity = 0.0f;
            mShip.mSpriteBody.LinearVelocity = new Vector2(0, 0);
            mDirection = Vector2.Zero;
            mRotatation = 0f;
            ai.reset();

        }

        public void reset()
        {
            mShip.mSpriteBody.Position = new Vector2(4.25f, 3.75f);
            mShip.mSpriteBody.Rotation = 0;
            mShip.mSpriteBody.AngularVelocity = 0.0f;
            mShip.mSpriteBody.LinearVelocity = new Vector2(0, 0);
            mDirection = Vector2.Zero;
            mRotatation = 0f;
            ai.reset();

        }

        private void resetParams()
        {
            this.mDirection = Vector2.Zero;
            this.mRotatation = 0f;
        }

    }
}
