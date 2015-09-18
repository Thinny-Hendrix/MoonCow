using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class ElectroShot:Projectile
    {
        public Enemy target;
        Turret turret;
        public float targetDist;
        public float time;
        bool triggered;
        public float length;
        public ElectroShot(Turret turret, Vector3 pos, Enemy enemy, Game1 game):base()
        {
            this.turret = turret;
            this.game = game;
            this.pos = pos;
            target = enemy;
            targetDist = 0;
            time = 0;
            triggered = false;

            direction = Vector3.Up;

            model = new ElectroShotModel(this, game);

            game.modelManager.addEffect(model);
        }

        public override void Update()
        {
            time += Utilities.deltaTime * MathHelper.Pi*4;

            targetDist = (float)Math.Sin(time);
            if(!triggered && time > MathHelper.PiOver2)
            {
                target.addElectroDamage(20);
                triggered = true;
            }

            direction.X = pos.X - target.pos.X;
            direction.Y = pos.Y - target.pos.Y;
            direction.Z = pos.Z - target.pos.Z;
            direction.Normalize();

            length = Utilities.tdTrig(pos, target.pos);

            if(time > MathHelper.Pi)
            {
                turret.toDelete.Add(this);
                model.Dispose();
            }

        }
    }
}
