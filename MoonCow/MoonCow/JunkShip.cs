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
        Game1 game;
        Minigame minigame;
        public Vector3 pos;
        OOBB col;
        CircleCollider interactRange;
        public Vector3 dir;
        Ship ship;
        MgInstance instance;
        JunkShipModel model;
        int successAtActivate;
        public JunkShip(Game1 game, Vector3 pos)
        {
            this.game = game;
            this.pos = pos;
            minigame = game.minigame;
            instance = new MgInstance(game, minigame);
            ship = game.ship;
            interactRange = new CircleCollider(pos, 10);
            dir.X = Utilities.nextFloat() * 2 - 1;
            dir.Z = Utilities.nextFloat() * 2 - 1;
            dir.Normalize();
            successAtActivate = -1;

            model = new JunkShipModel(this, game);
            game.modelManager.addObject(model);
        }
        public void Update()
        {
            if(interactRange.checkPoint(ship.pos))
            {
                //display message
                if (!minigame.active && !game.camera.transitioning)
                {
                    if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        if (minigame.successCount > successAtActivate)
                        {
                            instance.generateNew();
                            successAtActivate = minigame.successCount;
                        }
                        minigame.manager.loadInstance(instance);
                        minigame.activate(pos, Vector3.Cross(dir, Vector3.Up));
                        Vector3 camPos = pos+Vector3.Cross(dir, Vector3.Up)*10;
                        camPos.Y += 5;
                        game.camera.setStaticTarg(camPos, pos);
                    }
                }
            }
        }
    }
}
