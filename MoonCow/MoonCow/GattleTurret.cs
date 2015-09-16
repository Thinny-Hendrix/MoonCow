using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class GattleTurret:Turret
    {
        public GattleTurret(Vector3 pos, Vector3 targetDir, Game1 game):base(pos, targetDir,game)
        {
            col = new CircleCollider(pos, 20);
            turretModel = new TurretModel(this, game);
            game.modelManager.addObject(turretModel);
            cooldownMax = 1;
          
        }

        public override void Update()
        {
            if(cooldown > 0)
            {
                cooldown -= Utilities.deltaTime;
                if (cooldown < 0)
                    cooldown = 0;
            }
            if(state == State.idle)
            {
                //do a look around thing so they aren't completely static

                if(enemyManager.enemies.Count() > 0)
                {
                    if (enemiesInRange())
                        state = State.active;
                }
            }
            if(state == State.active)
            {
                setTarget();

                float yAngle = (float)Math.Atan2(pos.X - target.pos.X, pos.Z - target.pos.Z);
                float xAngle = (float)Math.Atan2(pos.Y - target.pos.Y, pos.Z - target.pos.Z);
                targetDir.X = (float)Math.Sin(yAngle);
                targetDir.Z = (float)Math.Cos(yAngle);
                targetDir.Y = -(float)Math.Sin(xAngle);
                targetDir.Normalize();
                /*
                targetDir.X = pos.X - target.pos.X;
                targetDir.Z = pos.Z - target.pos.Z;
                targetDir.Y = pos.Y+2 - target.pos.Y;
                targetDir.Normalize();*/
                currentDir = Vector3.Lerp(currentDir, targetDir, Utilities.deltaTime * 10);

                if (cooldown == 0)
                    fire();

                if (!enemiesInRange())
                    state = State.idle;
            }

            updateProjectiles();
        }

        public void updateProjectiles()
        {
            foreach (Projectile p in projectiles)
            {
                p.Update();
            }

            foreach (Projectile p in toDelete)
            {
                projectiles.Remove(p);
            }
            toDelete.Clear();
        }

        public override void checkRange()
        {

        }

        public override void fire()
        {
            cooldown = cooldownMax;
            projectiles.Add(new GattleProjectile(pos + new Vector3(targetDir.X * -4, 1.5f, targetDir.Z * -4), targetDir*-1, game, this));
        }

        public override void setTarget()
        {
            Vector2 nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));
            float closestDist = 100;
            try
            {
                //this loop runs through every enemy to determine which is the closest to the turret
                foreach (Enemy enemy in game.enemyManager.enemies)
                {
                    if (enemy.nodePos.X >= nodePos.X - 1 && enemy.nodePos.X <= nodePos.X + 1 &&
                        enemy.nodePos.Y >= nodePos.Y - 1 && enemy.nodePos.Y <= nodePos.Y + 1)
                    {
                        float testDist = col.distFrom(enemy.pos);
                        if(testDist < closestDist)
                        {
                            target = enemy;
                            closestDist = testDist;
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {}
        }
    }
}
