using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class TsModelManager
    {
        List<TsModel> solid;

        DepthStencilState depthStencilState;
        DepthStencilState dbNoWriteEnable;

        TsCamera cam;
        Game1 game;

        float time;
        float speed;


        public TsModelManager(Game1 game)
        {
            this.game = game;

            solid = new List<TsModel>();

            cam = new TsCamera(game);

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
            solid.Add(new TsModel(0));
            solid.Add(new TsModel(1));
            solid.Add(new TsModel(2));
        }

        public void Update()
        {
            if (!Utilities.paused)
            {
                time += Utilities.deltaTime * speed;
                if (time > 1)
                {
                    time = 0;
                }
                foreach (TsModel m in solid)
                    m.Update();
            }
        }

        public void changeVisible(int type)
        {
            for (int i = 0; i < solid.Count(); i++)
            {
                if (i == type)
                    solid.ElementAt(i).activate();
                else
                    solid.ElementAt(i).disable();
            }
        }

        public void Draw()
        {
            //game.GraphicsDevice.DepthStencilState = depthStencilState;
            //game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
               // = DepthStencilState.Default;

            game.GraphicsDevice.BlendState = BlendState.Additive;
            foreach (TsModel m in solid)
                m.Draw(game.GraphicsDevice, cam);
            game.GraphicsDevice.BlendState = BlendState.Opaque;
        }
    }
}
