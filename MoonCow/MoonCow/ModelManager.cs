using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class ModelManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        List<BasicModel> models = new List<BasicModel>();

        public ModelManager(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/corner1bigProto"), new Vector3(30, 0, 30), Vector3.Zero, Vector3.One));
            models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/corner1smallProto"), new Vector3(0, 0, 30), Vector3.Zero, Vector3.One));
            models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/corner2smallProto"), new Vector3(-30, 0, 30), Vector3.Zero, Vector3.One));

            models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/cornerProto"), new Vector3(30,0,0), Vector3.Zero, Vector3.One));
            models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/corstFlipProto"), Vector3.Zero, Vector3.Zero, Vector3.One)); //broke
            models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/corstProto"), new Vector3(-30,0,0), Vector3.Zero, Vector3.One)); //broke
            
            models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/dendProto"), new Vector3(30, 0, -30), Vector3.Zero, Vector3.One));
            models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/straight1Proto"), new Vector3(0, 0, -30), Vector3.Zero, Vector3.One));
            models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/straightProto"), new Vector3(-30, 0, -30), Vector3.Zero, Vector3.One));

            models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/tint3Proto"), new Vector3(30, 0, -60), Vector3.Zero, Vector3.One)); //bloke
            models.Add(new BasicModel(Game.Content.Load<Model>(@"Models/TempRails/tint4Proto"), new Vector3(0, 0, -60), Vector3.Zero, Vector3.One)); //broke
 



            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (BasicModel model in models)
            {
                model.Draw(((Game1)Game).GraphicsDevice, ((Game1)Game).camera);
            }



            base.Draw(gameTime);
        }

    }
}
