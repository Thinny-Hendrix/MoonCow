using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class BombExplosion
    {
        Vector3 pos;
        Game1 game;
        Vector2 nodePos;
        List<Enemy> hitList; //once enemy is hit, added to the list so damage is only applied once

        float damage;

        CircleCollider collider;
        bool colEnabled;


        float time;
        bool triggeredShake;
        bool triggeredParticles;

        public BombExplosion(Vector3 pos, Game1 game):base()
        {
            this.pos = pos;
            this.game = game;

            collider = new CircleCollider(pos, 5);

            triggeredShake = false;

            damage = 3;

            hitList = new List<Enemy>();
            game.modelManager.addEffect(new BombRings(pos, game, this));
        }

        public void Update()
        {
            float frameTime = Utilities.deltaTime*60;

            if(time >= 15)
            {
                if(!triggeredShake)
                {
                    colEnabled = true;
                    game.camera.makeShake();
                    game.hud.makeFlash();
                    game.modelManager.addEffect(new BombCenterParticle(game, pos));
                    triggeredShake = true;
                }
                if(!triggeredParticles && time > 15)
                {
                    for (int i = 0; i < 30; i++)
                        game.modelManager.addEffect(new DotParticle(game, pos, 30));
                    triggeredParticles = true;
                }

                if(collider.radius < 17)
                {
                    collider.radius += Utilities.deltaTime * 60;
                    if(collider.radius >= 17)
                        colEnabled = false;
                }
                //damage -= Utilities.deltaTime;
                if(colEnabled)
                    checkCollision();
            }
            time += frameTime;
            if (time > 120)
            {
                
            }
        }

        void checkCollision()
        {
            // By moving each component of the vector one at a time and seeing what causes the collision we can eliminate only that component
            // this means the ship will slide along walls rather than stick. Doing two collision checks per frame for the player seems to
            // be within tolerable limits for CPU time. This will only need to be done with the player
            bool collided = false;

            //## COLLISIONS WHOOO! ##
            // Move the bounding box to new pos
            //circleCol.Update(pos, direction);
            // Get current node co-ordinates
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            try 
            {
                foreach (Enemy enemy in game.enemyManager.enemies)
                {
                    if (enemy.nodePos.X >= nodePos.X - 1 && enemy.nodePos.X <= nodePos.X + 1 &&
                        enemy.nodePos.Y >= nodePos.Y - 1 && enemy.nodePos.Y <= nodePos.Y + 1)
                    {
                        //System.Diagnostics.Debug.WriteLine(hitList.Contains(enemy));
                        //if (hitList.Contains(enemy))
                        {
                            if (collider.checkPoint(enemy.pos))
                            {
                                enemy.health -= damage;
                                hitList.Add(enemy);
                                collided = true;
                            }
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
            }

            if (collided)
            {
            }
        }
    }
}
