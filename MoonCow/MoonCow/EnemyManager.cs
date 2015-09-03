using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class EnemyManager : Microsoft.Xna.Framework.GameComponent
    {
        public List<Enemy> enemies = new List<Enemy>();
        public List<Enemy> toDelete = new List<Enemy>();

        public EnemyManager(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);
            }

            foreach (Enemy enemy in toDelete)
            {
                enemies.Remove(enemy);
            }
        }

        public void addEnemy(Enemy newEnemy)
        {
            enemies.Add(newEnemy);
        }

        public void removeEnemy(Enemy enemy)
        {
            enemies.Remove(enemy);
        }
    }
}
