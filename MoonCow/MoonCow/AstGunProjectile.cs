using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class AstGunProjectile:Projectile
    {
        WeaponMissiles wep;
        int type;
        Color c1;
        Color c2;
        float emitterTime;
        public bool active;
        BasicModel trailModel;

        public AstGunProjectile(Vector3 pos, Vector3 direction, Game1 game, WeaponMissiles weapons, int type)
            : base()
        {
            this.direction = direction;
            this.game = game;
            this.pos = pos;
            this.rot.Y = (float)Math.Atan2(direction.X, direction.Z);
            this.wep = weapons;
            this.type = type;

            speed = 50;
            life = 120;
            damage = 5;
            active = true;

            col = new CircleCollider(pos, 0.2f);
            if (type == 1)
            {
                c1 = Color.White;
                c2 = Color.Aquamarine;
                model = new AstGunTip(this, game, c2);

            }
            else if (type == 2)
            {
                c1 = Color.White;
                c2 = Color.Purple;
                model = new AstGunTip(this, game, c2);
            }
            else
            {
                c1 = Color.White;
                c2 = Color.Red;
                model = new AstGunTip(this, game, c2);
                trailModel = new AstGunElecTrail(this, game, c1, c2);
                game.modelManager.addEffect(trailModel);
            }

            
            game.modelManager.addEffect(model);
        }

        public override void Update()
        {
            frameDiff = Vector3.Zero;

            frameDiff += direction * speed * Utilities.deltaTime;
            col.Update(pos);
            checkCollision();

            emitterTime -= Utilities.deltaTime;
            if (emitterTime <= 0)
            {
                emitterTime = 0.05f;
                game.modelManager.addEffect(new AstGunRing(this, game, c1, c2));
            }

            life -= Utilities.deltaTime * 60;
            if(life <=0)
            {
                deleteProjectile();
            }
        }

        void onImpact()
        {
            for (int i = 0; i < 10; i++)
                game.modelManager.addEffect(new DotParticle(game, pos));

            game.modelManager.addEffect(new LaserHitEffect(game, pos, c2));
            if (type == 3)
            {
                game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.White));
                for (int i = 0; i < 5; i++)
                    game.modelManager.addEffect(new ElecParticle(pos, game, c1, c2));
            }
        }

        protected override void checkCollision()
        {
            // By moving each component of the vector one at a time and seeing what causes the collision we can eliminate only that component
            // this means the ship will slide along walls rather than stick. Doing two collision checks per frame for the player seems to
            // be within tolerable limits for CPU time. This will only need to be done with the player
            pos += frameDiff;
            bool collided = false;

            //## COLLISIONS WHOOO! ##
            // Move the bounding box to new pos
            // Get current node co-ordinates
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            //For the current node check if your X component will make you collide with wall
            try
            {
                foreach (OOBB box in game.map.map[(int)nodePos.X, (int)nodePos.Y].collisionBoxes)
                {
                    if (col.checkOOBB(box))
                    {
                        pos.X -= frameDiff.X;
                        collided = true;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                deleteProjectile();
            }

            try
            {
                foreach (Enemy enemy in game.enemyManager.enemies)
                {
                    if (nodePos.X == enemy.nodePos.X && nodePos.Y == enemy.nodePos.Y)
                    {
                        //System.Diagnostics.Debug.WriteLine("Bullet in same node as enemy");
                        if (col.checkOOBB(enemy.boundingBox))
                        {
                            enemy.health -= damage*0.5f;
                            //game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                            collided = true;
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                deleteProjectile();
            }

            try
            {
                foreach (Sentry s in game.enemyManager.sentries)
                {
                    if (nodePos.X == s.nodePos.X && nodePos.Y == s.nodePos.Y)
                    {
                        //System.Diagnostics.Debug.WriteLine("Bullet in same node as enemy");
                        if (s.col.checkPoint(pos))
                        {
                            s.damage(damage, direction);
                            game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                            collided = true;
                            wep.addExp(damage);
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                deleteProjectile();
            }

            try
            {
                foreach (Asteroid a in game.asteroidManager.asteroids)
                {
                    if (nodePos.X == a.nodePos.X && nodePos.Y == a.nodePos.Y)
                    {
                        //System.Diagnostics.Debug.WriteLine("Bullet in same node as enemy");
                        if (a.col.checkPoint(pos))
                        {
                            a.damage(damage, pos);
                            game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                            collided = true;
                            wep.addExp(damage/4);
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException) { }

            foreach (JunkShip j in game.asteroidManager.junkShips)
            {
                bool checkCollide = false;
                foreach (OOBB o in j.cols)
                {
                    if (col.checkOOBB(o))
                    {
                        checkCollide = true;
                    }
                }
                if (checkCollide)
                    collided = true;
            }

            if (col.checkCircle(game.core.col))
                collided = true;

            if (collided)
            {
                onImpact();
                deleteProjectile();
            }
        }

        protected override void deleteProjectile()
        {
            game.modelManager.removeEffect(model);
            wep.toDelete.Add(this);
            model.Dispose();
            if (trailModel != null)
            {
                trailModel.Dispose();
                game.modelManager.removeEffect(trailModel);
            }
            active = false;
        }
    }
}
