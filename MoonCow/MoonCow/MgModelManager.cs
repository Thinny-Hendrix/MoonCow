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

        float time;
        float speed;


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

        public void setSpeed(float speed)
        {
            time = 0;
            this.speed = speed;
            foreach (MgModel m in solid)
                m.setSpeed(speed);
            foreach (MgModel m in additive)
                m.setSpeed(speed);
        }

        public void Pulse(float amount)
        {
            foreach (MgModel m in solid)
                m.Pulse(amount);
            foreach (MgModel m in additive)
                m.Pulse(amount);
        }

        void addModels()
        {
            solid.Add(new MgBackModel());
            additive.Add(new MgPolyBg());
            additive.Add(new MgGridModel());
        }

        public void Update()
        {
            time += Utilities.deltaTime * speed;
            if(time > 1)
            {
                time = 0;
                Pulse(4);
            }
            foreach (MgModel m in solid)
                m.Update();
            foreach (MgModel m in additive)
                m.Update();
        }

        public void Draw()
        {
            game.GraphicsDevice.DepthStencilState = depthStencilState;
               // = DepthStencilState.Default;

            game.GraphicsDevice.BlendState = BlendState.Opaque;
            foreach (MgModel m in solid)
                m.Draw(game.GraphicsDevice, cam);
            game.GraphicsDevice.BlendState = BlendState.Additive;
            foreach (MgModel m in additive)
                m.Draw(game.GraphicsDevice, cam);
        }

    }
}
