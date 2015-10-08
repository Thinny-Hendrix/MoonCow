using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class Turret
    {
        public Vector3 pos;
        public Vector3 rot;
        public Vector3 targetDir;
        public Vector3 currentDir;
        public CircleCollider col;
        public EnemyManager enemyManager;
        public Game1 game;
        public List<Projectile> projectiles;
        public List<Projectile> toDelete;
        public TurretModel turretModel;
        public Enemy target { get; protected set; }
        public float cooldown { get; protected set; }
        public enum State { idle, active }
        public State state;
        public float cooldownMax { get; protected set; }

        public Turret(Vector3 pos, Vector3 targetDir, Game1 game)
        {
            this.game = game;
            this.pos = pos;
            this.targetDir = targetDir;
            this.currentDir = targetDir;

            state = State.idle;
            projectiles = new List<Projectile>();
            toDelete = new List<Projectile>();
            col = new CircleCollider(pos, 40);
            //turretModel = new TurretModel(this, game);
            //game.modelManager.addObject(turretModel);
            enemyManager = game.enemyManager;
        }

        public virtual void Update()
        {

        }

        public virtual bool enemiesInRange(CircleCollider col)
        {
            Vector2 nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            try
            {
                foreach (Enemy enemy in game.enemyManager.enemies)
                {
                    if (enemy.nodePos.X >= nodePos.X - 1 && enemy.nodePos.X <= nodePos.X + 1 &&
                        enemy.nodePos.Y >= nodePos.Y - 1 && enemy.nodePos.Y <= nodePos.Y + 1)
                    {
                        if (col.checkPoint(enemy.pos))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            { return false; }
            return false;
        }
        public virtual void checkRange()
        {

        }

        public virtual void setTarget()
        {

        }

        public virtual void fire()
        {
            //make projectile
            //send message to model
        }

        public virtual void Dispose()
        {
            game.modelManager.removeObject(turretModel);
        }
    }
}
