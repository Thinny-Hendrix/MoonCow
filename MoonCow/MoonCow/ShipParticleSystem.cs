using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class ShipParticleSystem:GameComponent
    {
        Game1 game;
        Ship ship;
        List<BasicModel> moneyParticles = new List<BasicModel>();
        public List<BasicModel> moneyToDelete = new List<BasicModel>();
        List<BasicModel> thrustParticles = new List<BasicModel>();


        public ShipParticleSystem(Game1 game, Ship ship):base(game)
        {
            this.game = game;
            this.ship = ship;
        }

        public override void Update(GameTime gameTime)
        {
            foreach(BasicModel m in moneyToDelete)
            {
                game.modelManager.removeEffect(m);
                moneyParticles.Remove(m);
            }
            moneyToDelete.Clear();
        }

        public void addMoneyParticle(Color col)
        {
            MoneyCollectParticle p = new MoneyCollectParticle(game, ship, col);
            game.modelManager.addEffect(p);
            moneyParticles.Add(p);
        }

    }
}
