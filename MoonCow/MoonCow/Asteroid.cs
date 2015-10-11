using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class Asteroid
    {
        public AsteroidManager manager;
        public Vector3 pos;
        public Vector3 rot;
        public Vector3 dir;
        public Vector2 nodePos;
        public float moveSpeed;
        public float mass { get; protected set; }
        public float health { get; protected set; }
        public CircleCollider col { get; protected set; }
        public Game1 game { get; protected set; }
        public AsteroidModel model { get; protected set; }

        public Asteroid(Vector3 pos, Game1 game)
        {
            this.pos = pos;
            this.game = game;
            manager = game.asteroidManager;
            Random rng = Utilities.random;
            dir = Vector3.Zero;
            moveSpeed = 0f;
            
        }
        public virtual void Update()
        {
            // if movespeed drops below a certain threshold stop doing movement and collision checks
            updateNodePos();

            if(moveSpeed >= 0.1 && dir != Vector3.Zero)
            {
                updateMovement();
            }    
        }

        public virtual void updateMovement()
        {
            moveSpeed -= moveSpeed * (0.05f * Utilities.deltaTime);
            Vector3 frameDiff = Vector3.Zero;
            frameDiff += dir * (moveSpeed * Utilities.deltaTime);

            pos.X += frameDiff.X;   // Update the ship movment with X component of direction * speed
            if (checkCollisions())   // See if that caused a collision
            {
                pos.X -= frameDiff.X;   // Undo movment if so
            }
            pos.Z += frameDiff.Z;   // Now update the Z component
            if (checkCollisions())   // See if that caused collision
            {
                pos.Z -= frameDiff.Z;   // Undo movement if so
            }
            col.Update(pos);
        }

        public virtual bool checkCollisions()
        {

            col.Update(pos);
            bool collision = false;

            // Make a list containing current node and all neighbor nodes including diagonals
            List<MapNode> currentNodes = new List<MapNode>();

            for(int y = (int)nodePos.Y - 1; y <= nodePos.Y + 1; y++)
            {
                for(int x = (int)nodePos.X - 1; x <= nodePos.X + 1; x++)
                {
                    if(x >= 0 && x < game.map.getWidth() && y >= 0 && y < game.map.getHeight())
                    {
                        if(game.map.map[x,y] != null)
                        {
                            currentNodes.Add(game.map.map[x, y]);
                        }
                    }
                }
            }

            // For the current node check if you collide with anything
            foreach (MapNode node in currentNodes)
            {
                foreach (OOBB box in node.collisionBoxes)
                {
                    if (col.checkOOBB(box))
                    {
                        collision = true;
                        push(moveSpeed, dir * -1.5f, mass);
                        //System.Diagnostics.Debug.WriteLine("I am colliding with a wall");
                    }
                }
                if (node.asteroidBox != null)
                {
                    if (col.checkOOBB(node.asteroidBox))
                    {
                        collision = true;
                        push(moveSpeed, dir * -1.5f, mass);
                        //System.Diagnostics.Debug.WriteLine("I am colliding with a force field");
                    }
                }
                foreach (Asteroid a in game.asteroidManager.asteroids)
                {
                    if (!a.Equals(this)) // Do not check collisions with yourself
                    {
                        if (node.position.X == a.nodePos.X && node.position.Y == a.nodePos.Y)
                        {
                            if (a.col.checkPoint(pos))
                            {
                                a.push(moveSpeed, dir, mass);
                                push(moveSpeed, dir * -1.5f, mass);
                                //game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                                collision = true;
                                //System.Diagnostics.Debug.WriteLine("I am colliding with an asteroid");
                            }
                        }
                    }
                }
            } 
            return collision;
        }

        public virtual void push(float speed, Vector3 direction, float objectMass)
        {
            // Modify dir and moveSpeed, please ensure movespeed has a max value else things get icky
            dir += direction;
            dir.Normalize();

            float force = speed * objectMass;
            moveSpeed += force;
            if(moveSpeed > 15 - mass)
            {
                moveSpeed = 15 - mass;
            }
        }

        public virtual void damage(float value, Vector3 point)
        {
            health -= value;

            if (health <= 0)
                onDeath();
            //add force based on point of damage (and maybe amount of damage?)
            //maybe damage and push should be entirely separate, and shooting an ast calls both since different weps will push at different speeds not dependent on damage?
        }

        public virtual void onDeath()
        {
            //either create new asteroids, create money or both
            game.modelManager.removeObject(model);
            manager.addToDelete(this);
        }

        public virtual void updateNodePos()
        {
            nodePos = new Vector2((int)((pos.X / 30f) + 0.5f), (int)((pos.Z / 30f) + 0.5f));

            // If asteroid is out of map - delete it - should never happen, but causes indexRangeException when it does, so remove before it can cause such problems
            if(nodePos.X < 0 || nodePos.X > game.map.getWidth() || nodePos.Y < 0 || nodePos.Y > game.map.getHeight())
            {
                onDeath();
                System.Diagnostics.Debug.WriteLine("Asteroid outside of map? " + nodePos + " " + pos + " " + moveSpeed + " " + dir);
            }
        }
    }
}
