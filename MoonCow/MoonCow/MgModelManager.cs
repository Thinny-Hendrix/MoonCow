using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class MgModelManager
    {
        List<MgModel> additive;
        List<MgModel> solid;

        DepthStencilState depthStencilState;
        DepthStencilState dbNoWriteEnable;

        MgCamera cam;
        Game1 game;
        Minigame minigame;


        public MgModelManager(Game1 game, Minigame minigame)
        {
            this.game = game;
            this.minigame = minigame;

            additive = new List<MgModel>();
            solid = new List<MgModel>();

            cam = new MgCamera(game);

            depthStencilState = new DepthStencilState();
            depthStencilState.DepthBufferEnable = true;
            depthStencilState.DepthBufferWriteEnable = true;

            dbNoWriteEnable = new DepthStencilState();
            depthStencilState.DepthBufferEnable = true;
            dbNoWriteEnable.DepthBufferWriteEnable = false;

            addModels();
        }

        void addModels()
        {
            solid.Add(new MgPolyBg());
        }

        public void Update()
        {
            game.GraphicsDevice.BlendState = BlendState.Opaque;
            foreach (MgModel m in solid)
                m.Update();
            foreach (MgModel m in additive)
                m.Update();
        }

        public void Draw()
        {
            //game.GraphicsDevice.DepthStencilState = depthStencilState;

            foreach (MgModel m in solid)
                m.Draw(game.GraphicsDevice, cam);
            foreach (MgModel m in additive)
                m.Draw(game.GraphicsDevice, cam);
        }

    }
}
