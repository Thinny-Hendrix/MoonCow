using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace MoonCow
{
    public class JunkShip
    {
        protected Game1 game;
        protected Minigame minigame;
        public Vector3 pos;
        public OOBB col;
        protected CircleCollider interactRange;
        public Vector3 dir;
        protected Ship ship;
        protected MgInstance instance;
        protected JunkShipModel model;
        protected int successAtActivate;
        public Vector2 nodePos;
        public bool destroying;
        public float time;
        protected float moneyVal;
        protected bool triggeredMessage;
        public List<OOBB> cols;

        public JunkShip() { }
        public JunkShip(Game1 game, Vector3 pos)
        {
            this.game = game;
            this.pos = pos;
            minigame = game.minigame;
            instance = new MgInstance(game, minigame);
            ship = game.ship;
            dir.X = Utilities.nextFloat() * 2 - 1;
            dir.Z = Utilities.nextFloat() * 2 - 1;
            dir.Normalize();
            successAtActivate = -1;

            interactRange = new CircleCollider(this.pos + dir*4, 2);

            cols = new List<OOBB>();
            cols.Add(new OOBB(this.pos + dir * 1.5f, dir, 1, 4, dir));
            cols.Add(new OOBB(this.pos + dir * -1.5f, dir, 1, 4, -dir));
            cols.Add(new OOBB(this.pos + Vector3.Cross(dir, Vector3.Up) * -1.5f, dir, 1, 4, Vector3.Cross(-dir, Vector3.Up)));
            cols.Add(new OOBB(this.pos + Vector3.Cross(-dir, Vector3.Up) * -1.5f, dir, 1, 4, Vector3.Cross(dir, Vector3.Up)));


            col = new OOBB(this.pos+dir*1.5f, dir, 1, 4, dir);

            model = new JunkShipModel(this, game);
            game.modelManager.addObject(model);
            nodePos = new Vector2((int)((this.pos.X / 30) + 0.5f), (int)((this.pos.Z / 30) + 0.5f));

            destroying = false;
        }

        public virtual void beatMinigame(float money)
        {
            this.moneyVal = money;
            destroying = true;
        }

        public virtual void Update()
        {
            if (destroying)
            {
                time += Utilities.deltaTime;
                if (time >= 1)
                {
                    if (game.hud.hudCollectable.spawnedChips < 4)
                    {
                        game.hud.hudCollectable.spawnedChips++;
                        game.modelManager.addObject(new CollectableChip(pos, game));
                    }

                    for (int i = 0; i < 10; i++)
                        game.modelManager.addEffect(new GlowStreak(game, pos, new Vector2(2, 7), 2, Color.White, 0, -1));
                    game.modelManager.addEffect(new GlowStreakCenter(game, pos, 3, 2, -1));

                    game.modelManager.addEffect(new ImpactParticleModel(game, pos, 0.5f));
                    game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.White, 1, BlendState.Additive));

                    game.ship.moneyManager.makeMoney(moneyVal, 4, pos);
                    game.asteroidManager.jToDelete.Add(this);
                    model.Dispose();
                    game.modelManager.removeObject(model);
                }
            }
            else
            {
                if (interactRange.checkPoint(ship.pos))
                {
                    //display message
                    if (!minigame.active && !game.camera.transitioning)
                    {
                        game.hud.hudPrompt.activate("Unlock cache");
                        triggeredMessage = true;
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            if (minigame.successCount > successAtActivate)
                            {
                                instance.generateNew();
                                successAtActivate = minigame.successCount;
                            }
                            minigame.manager.loadInstance(instance);
                            minigame.activate(pos, dir, this);
                            Vector3 camPos = pos + dir * 10;
                            camPos.Y += 5;
                            game.camera.setStaticTarg(camPos, pos);
                        }
                    }
                    else
                    {
                        if (triggeredMessage)
                        {
                            triggeredMessage = false;
                            game.hud.hudPrompt.close();
                        }
                    }
                }
                else
                {
                    if (triggeredMessage)
                    {
                        triggeredMessage = false;
                        game.hud.hudPrompt.close();
                    }
                }
            }
        }
    }
}
