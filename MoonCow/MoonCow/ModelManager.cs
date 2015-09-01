using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class ModelManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        List<BasicModel> models = new List<BasicModel>();
        List<BasicModel> enemyModels = new List<BasicModel>();
        List<BasicModel> effectModels = new List<BasicModel>();
        List<BasicModel> additiveModels = new List<BasicModel>();

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
            
                //models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/Ship/shipBlock"), new Vector3(30, 0, 30), 0.0f, 1.0f));

                /*
                models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/corner1bigProto"), new Vector3(30, 0, 30), 0.0f, 1.0f));
                models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/corner1smallProto"), new Vector3(0, 0, 30), 0.0f, 1.0f));
                models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/corner2smallProto"), new Vector3(-30, 0, 30), 0.0f, 1.0f));

                models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/cornerProto"), new Vector3(30,0,0), 0.0f, 1.0f));
                models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/corstFlipProto"), Vector3.Zero, 0.0f, 1.0f)); //broke
                models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/corstProto"), new Vector3(-30,0,0), 0.0f, 1.0f)); //broke
            
                models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/dendProto"), new Vector3(30, 0, -30), 0.0f, 1.0f));
                models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/straight1Proto"), new Vector3(0, 0, -30), 0.0f, 1.0f));
                models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/straightProto"), new Vector3(-30, 0, -30), 0.0f, 1.0f));

                models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/tint3Proto"), new Vector3(30, 0, -60), 0.0f, 1.0f)); //bloke
                models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/tint4Proto"), new Vector3(0, 0, -60), 0.0f, 1.0f)); //broke
                */

                base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
           
            foreach (BasicModel model in models)
                model.Update(gameTime);

            foreach (BasicModel model in additiveModels)
                model.Update(gameTime);

            speedCyl.Update(gameTime);

            base.Update(gameTime);

            //System.Diagnostics.Debug.WriteLine(models.Count);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = depthStencilState;

            foreach (BasicModel model in models)
            {
                model.Draw(((Game1)Game).GraphicsDevice, ((Game1)Game).camera);
            }

            base.Draw(gameTime);

            GraphicsDevice.DepthStencilState = dbNoWriteEnable;
            foreach(BasicModel model in additiveModels)
                model.Draw(((Game1)Game).GraphicsDevice, ((Game1)Game).camera);
            speedCyl.overrideDraw(((Game1)Game).GraphicsDevice, ((Game1)Game).camera);
        }

        public void add(BasicModel model)
        {
            models.Add(model);
        }

        public void addEffect(BasicModel model)
        {
            effectModels.Add(model);
        }

        public void addAdditive(BasicModel model)
        {
            additiveModels.Add(model);
        }

        public void addTransparent(SpeedCylModel model)
        {
            speedCyl = model;
        }

        public void makeStarField()
        {
            for (int i = 0; i < 75; i++)
            {
                models.Add(new SpaceDust(Game.Content.Load<Model>(@"Models/Misc/octahedron"), ((Game1)Game).ship, 0));
            }

            for (int i = 0; i < 75; i++)
            {
                models.Add(new SpaceDust(Game.Content.Load<Model>(@"Models/Misc/octahedron"), ((Game1)Game).ship, 1));
            }
        }


    }
}
