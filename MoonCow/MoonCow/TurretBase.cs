using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class TurretBase
    {
        public Turret turret;
        Vector3 pos;
        Vector3 dir;
        Game1 game;
        Ship ship;
        TurretBaseModel baseModel;
        int time = 0;
        Vector2 nodePos;
        CircleCollider col;

        public TurretBase(Vector3 pos, Vector3 dir, Game1 game)
        {
            this.pos = pos;
            this.dir = dir;
            this.game = game;
            baseModel = new TurretBaseModel(pos);
            game.modelManager.addObject(baseModel);
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));
            ship = game.ship;

            col = new CircleCollider(pos, 5);
        }

        public void timer()
        {
            //incredibly dirty hack to prevent null enemyManager, will become obsolete once player needs to manually create turrets
            time++;
            if(time >= 10)
                turret = new GattleTurret(pos, dir, game);
        }

        public void Update()
        {
            //if (time < 10)
                //timer();

            checkCollision();

            if(turret != null)
            {
                turret.Update();
            }
        }

        void checkCollision()
        {
            if(ship.nodePos.X == nodePos.X && ship.nodePos.Y == nodePos.Y)
            {
                if(col.checkPoint(ship.pos))
                {
                    //future - will activate hud dialogue box

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        if (turret == null)
                        {
                            if(ship.moneyManager.canPurchase(200))
                                turret = new GattleTurret(pos, dir, game);
                        }
                    }
                }
            }
        }
            
    }
}
