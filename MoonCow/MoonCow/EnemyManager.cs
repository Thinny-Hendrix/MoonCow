using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class EnemyManager : Microsoft.Xna.Framework.GameComponent
    {
        public List<Enemy> enemies = new List<Enemy>();
        public List<Enemy> toDelete = new List<Enemy>();
        public List<Sentry> sentries = new List<Sentry>();
        public List<Sentry> sToDelete = new List<Sentry>();

        public List<Projectile> projectiles = new List<Projectile>();
        public List<Projectile> pToDelete = new List<Projectile>();

        Game1 game;

        public EnemyManager(Game1 game) : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            //addEnemy(new Enemy(this.game));
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                foreach (Enemy enemy in enemies)
                    enemy.Update(gameTime);

                foreach (Enemy enemy in toDelete)
                    enemies.Remove(enemy);
                toDelete.Clear();

                foreach (Sentry s in sentries)
                    s.Update();

                foreach (Sentry s in sToDelete)
                    sentries.Remove(s);
                sToDelete.Clear();

                foreach (Projectile p in projectiles)
                    p.Update();

                foreach (Projectile p in pToDelete)
                    projectiles.Remove(p);
                pToDelete.Clear();
            }
        }

        public void addEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
        }

        public void removeEnemy(Enemy enemy)
        {
            enemies.Remove(enemy);
        }

        public void addSentry(Vector3 pos)
        {
            sentries.Add(new Sentry(game, this, pos));
        }

        public void turretPlaced()
        {
            foreach(Enemy enemy in enemies)
            {
                if(enemy.enemyType == 2)
                {
                    enemy.updatePath();
                }
            }
        }
    }
}
