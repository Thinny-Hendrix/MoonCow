using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class TurretManager:GameComponent
    {
        public List<TurretBase> turrets;
        Game1 game;

        public TurretManager(Game1 game):base(game)
        {
            this.game = game;
            turrets = new List<TurretBase>();
            turrets.Add(new TurretBase(new Vector3(150,4.5f,400), Vector3.Left, game));
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                foreach (TurretBase t in turrets)
                    t.Update();
            }
        }

        public void addTurret(Vector3 pos, Vector3 dir)
        {
            turrets.Add(new TurretBase(pos, dir, game));
        }
    }
}
