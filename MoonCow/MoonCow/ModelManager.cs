﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class ModelManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        List<BasicModel> nodeModels = new List<BasicModel>();
        List<BasicModel> objectModels = new List<BasicModel>();
        List<BasicModel> enemyModels = new List<BasicModel>();
        List<BasicModel> effectModels = new List<BasicModel>();
        List<BasicModel> additiveModels = new List<BasicModel>();

        List<BasicModel> toDeleteEffect = new List<BasicModel>();
        List<BasicModel> toDeleteObjects = new List<BasicModel>();


        //List<SpeedCylModel> transModels = new List<Speed>(); //will need a separate list for transparent models
        SpeedCylModel speedCyl;

        DepthStencilState depthStencilState;
        DepthStencilState dbNoWriteEnable;


        public ModelManager(Game game)
            : base(game)
        {
            depthStencilState = new DepthStencilState();
            depthStencilState.DepthBufferEnable = true;
            depthStencilState.DepthBufferWriteEnable = true;

            dbNoWriteEnable = new DepthStencilState();
            depthStencilState.DepthBufferEnable = true;
            dbNoWriteEnable.DepthBufferWriteEnable = false;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
                base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
           
            foreach (BasicModel model in nodeModels)
                model.Update(gameTime);

            foreach (BasicModel model in objectModels)
                model.Update(gameTime);
            foreach (BasicModel m in toDeleteObjects)
                objectModels.Remove(m);
            toDeleteObjects.Clear();

            foreach (BasicModel model in additiveModels)
                model.Update(gameTime);

            foreach (BasicModel model in effectModels)
                model.Update(gameTime);

            foreach (BasicModel m in toDeleteEffect)
                effectModels.Remove(m);
            toDeleteEffect.Clear();

            foreach (BasicModel model in enemyModels)
                model.Update(gameTime);

            speedCyl.Update(gameTime);

            base.Update(gameTime);

            //System.Diagnostics.Debug.WriteLine(models.Count);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = depthStencilState;

            if (!((Game1)Game).ship.finishingMove)
            {
                foreach (BasicModel model in nodeModels)
                {
                    model.Draw(((Game1)Game).GraphicsDevice, ((Game1)Game).camera);
                }
            }

            foreach (BasicModel model in objectModels)
                model.Draw(((Game1)Game).GraphicsDevice, ((Game1)Game).camera);

            foreach (BasicModel model in enemyModels)
                model.Draw(((Game1)Game).GraphicsDevice, ((Game1)Game).camera);
            

            base.Draw(gameTime);

            GraphicsDevice.DepthStencilState = dbNoWriteEnable;
            foreach(BasicModel model in additiveModels)
                model.Draw(((Game1)Game).GraphicsDevice, ((Game1)Game).camera);

            if (!((Game1)Game).ship.finishingMove)
            {
                foreach (BasicModel model in nodeModels)
                {
                    model.DrawAlt(((Game1)Game).GraphicsDevice, ((Game1)Game).camera);
                }
            }

            foreach (BasicModel model in effectModels)
                model.Draw(((Game1)Game).GraphicsDevice, ((Game1)Game).camera);


            speedCyl.overrideDraw(((Game1)Game).GraphicsDevice, ((Game1)Game).camera);
        }

        public void add(BasicModel model)
        {
            nodeModels.Add(model);
        }

        public void addEffect(BasicModel model)
        {
            effectModels.Add(model);
        }

        public void removeEffect(BasicModel model)
        {
            effectModels.Remove(model);
        }

        public void addObject(BasicModel model)
        {
            objectModels.Add(model);
        }

        public void removeObject(BasicModel model)
        {
            objectModels.Remove(model);
        }

        public void addEnemy(BasicModel model)
        {
            enemyModels.Add(model);
        }

        public void removeEnemy(BasicModel model)
        {
            enemyModels.Remove(model);
        }

        public void addAdditive(BasicModel model)
        {
            additiveModels.Add(model);
        }

        public void addTransparent(SpeedCylModel model)
        {
            speedCyl = model;
        }

        public void toDeleteModel(BasicModel model)
        {
            toDeleteEffect.Add(model);
        }

        public void toDeleteObject(BasicModel model)
        {
            toDeleteObjects.Add(model);
        }

        public void makeStarField()
        {
            for (int i = 0; i < 75; i++)
            {
                nodeModels.Add(new SpaceDust(Game.Content.Load<Model>(@"Models/Misc/spacerockpoly"), ((Game1)Game).ship, 0));
            }

            for (int i = 0; i < 75; i++)
            {
                nodeModels.Add(new SpaceDust(Game.Content.Load<Model>(@"Models/Misc/spacerockpoly"), ((Game1)Game).ship, 1));
            }
        }


    }
}
