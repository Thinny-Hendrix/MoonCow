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

        public enum TurretType { none, gattle, pyro, electro}
        public TurretType turretType;

        public TurretBase(Vector3 pos, Vector3 dir, Game1 game)
        {
            this.pos = pos;
            this.dir = dir;
            this.game = game;
            baseModel = new TurretBaseModel(pos, game);
            game.modelManager.addObject(baseModel);
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));
            ship = game.ship;
            turretType = TurretType.none;

            col = new CircleCollider(pos, 5);
        }

        public void Update()
        {
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
                            if (ship.moneyManager.canPurchase(0))
                            {
                                turret = new GattleTurret(pos, dir, game);
                                turretType = TurretType.gattle;
                                baseModel.changeColor(turretType, game);
                                game.map.map[(int)nodePos.X, (int)nodePos.Y].damage += 3;
                                game.enemyManager.turretPlaced();
                            }
                        }
                    }

                    if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                    {
                        if (turret == null)
                        {
                            if (ship.moneyManager.canPurchase(0))
                            {
                                turret = new ElectroTurret(pos, dir, game);
                                turretType = TurretType.electro;
                                baseModel.changeColor(turretType, game);
                                game.map.map[(int)nodePos.X, (int)nodePos.Y].damage += 5;
                                game.enemyManager.turretPlaced();
                            }
                        }
                    }

                    if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.OemComma))
                    {
                        if (turret == null)
                        {
                            if (ship.moneyManager.canPurchase(0))
                            {
                                turret = new PyroTurret(pos, dir, game);
                                turretType = TurretType.pyro;
                                baseModel.changeColor(turretType, game);
                                game.map.map[(int)nodePos.X, (int)nodePos.Y].damage += 3;
                                game.enemyManager.turretPlaced();
                            }
                        }
                    }
                }
            }
        }
            
    }
}
