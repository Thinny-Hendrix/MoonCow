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
        }
        public virtual void Update()
        {
            updateNodePos();
        }

        public virtual void checkCollisions()
        {

        }

        public virtual void push(float speed, Vector3 point)
        {

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
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));
        }
    }
}
