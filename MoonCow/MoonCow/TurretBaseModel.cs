using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class TurretBaseModel:BasicModel
    {
        SpriteBatch sb;
        RenderTarget2D rTarg;
        Color col;
        public TurretBaseModel(Vector3 pos, Game1 game):base()
        {
            this.pos = pos;
            model = ModelLibrary.turretBase;
            rTarg = new RenderTarget2D(game.GraphicsDevice, 1, 1);
            sb = new SpriteBatch(game.GraphicsDevice);

            col = Color.LightGoldenrodYellow;
            changeColor(TurretBase.TurretType.none, game);
        }

        public void changeColor(TurretBase.TurretType type, Game1 game)
        {
            switch((int)type)
            {
                default: //none
                    col = Color.Aqua;
                    break;
                case 1:
                    col = Color.Red;
                    break;
                case 2:
                    col = Color.Orange;
                    break;
                case 3:
                    col = Color.SeaGreen;
                    break;
            }
            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(TextureManager.pureWhite, Vector2.Zero, col);
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = mesh.ParentBone.Transform * GetWorld();
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.Alpha = 1;

                    effect.LightingEnabled = true;

                    if (mesh.Name.Contains("glow"))
                    {
                        effect.Texture = (Texture2D)rTarg;
                        effect.AmbientLightColor = Vector3.One;
                    }
                    else
                    {
                        effect.Texture = TextureManager.turBase;
                        effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                        effect.DirectionalLight0.SpecularColor = Vector3.One;
                        effect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
                        effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
                    }

                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }
    }
}
