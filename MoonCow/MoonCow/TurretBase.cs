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
        TurretHolo holo;
        bool triggeredHoloClose;
        Vector2 nodePos;
        CircleCollider col;
        bool triggeredHud;

        float coolDown;

        float gattPrice;
        float pyroPrice;
        float elecPrice;

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

            gattPrice = 1250;
            pyroPrice = 2000;
            elecPrice = 3500;

            col = new CircleCollider(pos, 5);

            coolDown = 0;

            holo = new TurretHolo(pos, game);
            game.modelManager.addEffect(holo);
            triggeredHoloClose = false;
        }

        public void Update()
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if (coolDown != 0)
                {
                    coolDown -= Utilities.deltaTime;
                    if (coolDown < 0)
                        coolDown = 0;
                }
            }

            checkCollision();

            if(turret != null)
            {
                turret.Update();
            }
        }

        void spawnEffect()
        {
            for (int i = 0; i < 10; i++)
                game.modelManager.addEffect(new GlowStreak(game, pos, new Vector2(1, 4), 1,Color.White, 1));
        }

        public bool setTurret(int i)
        {
            bool returnVal = false;

            switch(i)
            {
                default:
                    turret.Dispose();
                    turret = null;

                    switch((int)turretType)
                    {
                        default:
                            break;
                        case 1:
                            game.ship.moneyManager.addMoney(gattPrice);
                            break;
                        case 2:
                            game.ship.moneyManager.addMoney(pyroPrice);
                            break;
                        case 3:
                            game.ship.moneyManager.addMoney(elecPrice);
                            break;
                    }



                    turretType = TurretType.none;
                    returnVal = true;
                    baseModel.changeColor(turretType, game);
                    break;
                case 1:
                    if (ship.moneyManager.canPurchase(gattPrice))
                    {
                        turret = new GattleTurret(pos, dir, game);
                        turretType = TurretType.gattle;
                        baseModel.changeColor(turretType, game);
                        game.map.map[(int)nodePos.X, (int)nodePos.Y].damage += 3;
                        game.enemyManager.turretPlaced();
                        spawnEffect();
                        returnVal = true;
                    }
                    break;
                case 2:
                    if (ship.moneyManager.canPurchase(pyroPrice))
                    {
                        turret = new PyroTurret(pos, dir, game);
                        turretType = TurretType.pyro;
                        baseModel.changeColor(turretType, game);
                        game.map.map[(int)nodePos.X, (int)nodePos.Y].damage += 3;
                        game.enemyManager.turretPlaced();
                        spawnEffect();
                        returnVal = true;
                    }
                    break;
                case 3:
                    if (ship.moneyManager.canPurchase(elecPrice))
                    {
                        turret = new ElectroTurret(pos, dir, game);
                        turretType = TurretType.electro;
                        baseModel.changeColor(turretType, game);
                        game.map.map[(int)nodePos.X, (int)nodePos.Y].damage += 5;
                        game.enemyManager.turretPlaced();
                        spawnEffect();
                        returnVal = true;
                    }
                    break;
            }
            return returnVal;
        }

        void checkCollision()
        {
            if(ship.nodePos.X == nodePos.X && ship.nodePos.Y == nodePos.Y)
            {
                if (col.checkPoint(ship.pos))
                {
                    if (!triggeredHoloClose)
                    {
                        holo.close();
                        triggeredHoloClose = true;
                    }
                    if(turretType == TurretType.none)
                        game.hud.hudPrompt.activate("purchase turret");
                    else
                        game.hud.hudPrompt.activate("sell turret");
                    triggeredHud = true;

                    //future - will activate hud dialogue box
                    if (coolDown == 0)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)
                        {
                            coolDown = 1;
                            if (turretType == TurretType.none)
                                game.hud.turSelect.activate(this, 0);
                            else
                                game.hud.turSelect.activate(this, 1);
                        }
                    }
                }
                else
                {
                    if (triggeredHud)
                    {
                        game.hud.hudPrompt.close();
                        triggeredHud = false;
                    }

                    if (triggeredHoloClose && turretType == TurretType.none)
                    {
                        holo.wake();
                        triggeredHoloClose = false;
                    }
                }
            }
            else
            {
                if (triggeredHud)
                {
                    game.hud.hudPrompt.close();
                    triggeredHud = false;
                }

                if (triggeredHoloClose && turretType == TurretType.none)
                {
                    holo.wake();
                    triggeredHoloClose = false;
                }
            }
        }
            
    }
}
