using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class JsConsole:JunkShip
    {
        /*
        protected Game1 game;
        protected Minigame minigame;
        public Vector3 pos;
        public OOBB col;
        protected CircleCollider interactRange;
        public Vector3 dir;
        protected Ship ship;
        protected MgInstance instance;
        protected JunkShipModel model;
        int successAtActivate;
        public Vector2 nodePos;
        public bool destroying;
        public float time;
        protected float moneyVal;
        protected bool triggeredMessage;
        public List<OOBB> cols;*/
        public int cardCount;
        public CircleCollider interestRange;
        Vector3 consolePos;
        bool triggeredMoney;
        public JsConsole(Game1 game, Vector3 pos, Vector3 dir):base()
        {
            this.game = game;
            this.pos = pos;
            minigame = game.minigame;
            instance = new MgInstance(game, minigame);
            ship = game.ship;
            this.dir = dir;
            successAtActivate = -1;

            interactRange = new CircleCollider(this.pos + dir * 16, 3);
            interestRange = new CircleCollider(this.pos + dir * 16, 20);

            model = new JsConsoleModel(this, game);
            game.modelManager.addObject(model);
            nodePos = new Vector2((int)((this.pos.X / 30) + 0.5f), (int)((this.pos.Z / 30) + 0.5f));

            destroying = false;

            cardCount = 0;
            consolePos = pos + dir * 15;
            consolePos.Y = 4.5f;

            cols = new List<OOBB>();
            cols.Add(new OOBB(consolePos + dir*1, dir, 5, 1, dir));
            cols.Add(new OOBB(consolePos + dir*-1, dir, 5, 1, -dir));
            cols.Add(new OOBB(consolePos + Vector3.Cross(dir, Vector3.Up) * -2f, dir, 2, 1, Vector3.Cross(-dir, Vector3.Up)));
            cols.Add(new OOBB(consolePos + Vector3.Cross(-dir, Vector3.Up) * -2f, dir, 2, 1, Vector3.Cross(dir, Vector3.Up)));

            cols.Add(new OOBB(pos + dir * 13, dir, 30, 1, dir));

        }

        public override void beatMinigame(float money)
        {
            this.moneyVal = money;
            destroying = true;
        }

        public override void Update()
        {
            if (destroying)
            {
                if (!triggeredMoney)
                {
                    time += Utilities.deltaTime;
                    if (time >= 1)
                    {
                        game.ship.moneyManager.makeMoney(moneyVal, 4, consolePos + new Vector3(0,-4,0));
                        //game.asteroidManager.jToDelete.Add(this);
                        //model.Dispose();
                        //game.modelManager.removeObject(model);
                        triggeredMoney = true;
                    }

                    cols.Clear();
                }
            }
            else
            {
                if(interestRange.checkPoint(ship.pos))
                {
                    game.hud.hudCollectable.Wake();
                }
                if (interactRange.checkPoint(ship.pos))
                {
                    //display message
                    if (cardCount != 4)
                    {
                        if (game.hud.hudCollectable.displayCount > 0)
                        {
                            cardCount += game.hud.hudCollectable.displayCount;
                            game.hud.hudCollectable.releaseCards();
                        }
                        else
                        {
                            game.hud.hudMessage.drillDoorCheck();
                        }
                    }
                    if (cardCount == 4 && !minigame.active && !game.camera.transitioning)
                    {
                        game.hud.hudPrompt.activate("Unlock door");
                        triggeredMessage = true;
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            if (minigame.successCount > successAtActivate)
                            {
                                instance.generateNewHard();
                                successAtActivate = minigame.successCount;
                            }
                            minigame.manager.loadInstance(instance);
                            minigame.activateHard(consolePos, dir, this);
                            Vector3 camPos = consolePos + dir * 10;
                            camPos.Y += 5;
                            game.camera.setStaticTarg(camPos, consolePos);
                        }
                    }
                    else
                    {
                        if(triggeredMessage)
                            game.hud.hudPrompt.close();
                    }
                }
                else
                {
                    if (triggeredMessage)
                        game.hud.hudPrompt.close();
                }
            }
        }
    }
}
