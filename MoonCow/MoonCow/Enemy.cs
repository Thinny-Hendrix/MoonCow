using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class Enemy
    {
        public Matrix transform;
        public Vector3 pos;
        public Vector3 rot;
        public Vector3 scale;
        public Vector3 direction;

        public float moveSpeed;
        public float maxSpeed;
        protected float time;

        //All to do with turning;
        public float currentTurnSpeed;
        public float maxTurnSpeed;
        protected bool atCore = false;
        protected Vector3 nodeDirection = new Vector3(0, 0, 1);
        protected bool turnDirection = false; //false = left, true = right
        protected char isFacing = 'S'; //North, South, East, West
        protected char wasFacing = 'S';

        public OOBB boundingBox;
        public CircleCollider agroSphere;
        public int health;

        public Vector2 nodePos;

        protected Game1 game;

        protected Vector2 prevPosition;
        protected Vector2 nextPosition;
        //Vector2 futurePosition; - May need in future

        protected int pathPosition = 0;

        public EnemyModel enemyModel;
        //WeaponSystem weapons;

        public Enemy(Game1 game)
        {

        }

        protected float makeCentreCoordinate(float c)
        {
            return (c) * 30f;
        }

        protected float makePointCoordinate(float c)
        {
            return (c)/30f;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        protected void death()
        {
            for (int i = 0; i < 4; i++)
                game.ship.moneyManager.addGib(5, pos);

            game.modelManager.removeEnemy(enemyModel);
            game.enemyManager.toDelete.Add(this);
        }

        protected void evenOut(bool invert)
        {
            if (!atCore)
            {
                if (invert)
                {
                    if (turnDirection)
                        rot.Y += currentTurnSpeed;
                    else
                        rot.Y -= currentTurnSpeed;
                }
                else
                {
                    if (turnDirection)
                        rot.Y -= currentTurnSpeed;
                    else
                        rot.Y += currentTurnSpeed;
                }

                direction.X = -(float)Math.Sin(rot.Y);
                direction.Z = -(float)Math.Cos(rot.Y);
                direction.Normalize();
            }
        }

        public virtual void updatePath()
        {

        }
    }
}