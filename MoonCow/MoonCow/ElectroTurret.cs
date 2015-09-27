﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class ElectroTurret:Turret
    {
        public enum ChargeState { charging, idle }
        public ChargeState chargeState;
        public float chargeTime;
        List<Enemy> targets;
        CircleCollider wakeRange;
        Vector3 shotPos;
        public ElectroTurret(Vector3 pos, Vector3 targetDir, Game1 game):base(pos, targetDir,game)
        {
            col = new CircleCollider(pos, 20);
            wakeRange = new CircleCollider(pos, 25);
            game.modelManager.addObject(new ElectroTurretModel(this, game));
            cooldownMax = 0.25f;
            chargeState = ChargeState.idle;
            chargeTime = 2;
            targets = new List<Enemy>();
            shotPos = pos;
            shotPos.Y += 5;
          
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
                if(enemyManager.enemies.Count() > 0)
                {
                    if (enemiesInRange(wakeRange))
                        state = State.active;
                }
            }
            if (state == State.active)
            {
                if (cooldown == 0)
                    chargeState = ChargeState.charging;

                if (chargeState == ChargeState.charging)
                {
                    chargeTime += Utilities.deltaTime;
                    if (chargeTime > 2)
                    {
                        if (enemiesInRange(col))
                        {
                            fire();
                            chargeState = ChargeState.idle;
                        }
                    }

                    if (!enemiesInRange(wakeRange))
                    {
                        state = State.idle;
                        chargeTime = 0;
                        chargeState = ChargeState.idle;
                        cooldown = 2;
                    }
                }
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

        public override void fire()
        {
            setTarget();
            cooldown = cooldownMax;
            chargeTime = 0;
            foreach(Enemy e in targets)
            {
                //e.health -= 20;
                projectiles.Add(new ElectroShot(this, shotPos, e, game));
                //electroshot needs starting position and enemy
            }
            targets.Clear();
            //projectiles.Add(new GattleProjectile(pos + new Vector3(targetDir.X * -4, 1.5f, targetDir.Z * -4), targetDir*-1, game, this));
        }

        public override void setTarget()
        {
            Vector2 nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));
            try
            {
                //this loop runs through every enemy to get a max 10 enemies which are in range
                foreach (Enemy enemy in game.enemyManager.enemies)
                {
                    if (enemy.nodePos.X >= nodePos.X - 1 && enemy.nodePos.X <= nodePos.X + 1 &&
                        enemy.nodePos.Y >= nodePos.Y - 1 && enemy.nodePos.Y <= nodePos.Y + 1)
                    {
                        if(targets.Count() < 10 && col.checkPoint(enemy.pos))
                            targets.Add(enemy);
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {}
        }
    }
}