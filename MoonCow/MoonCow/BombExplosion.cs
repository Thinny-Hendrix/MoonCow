﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class BombExplosion:BasicModel
    {
        Game1 game;
        Model ripple;
        SpriteBatch sb;
        RenderTarget2D rippleTex;
        Vector2 texPos;
        Vector2 nodePos;
        List<Enemy> hitList; //once enemy is hit, added to the list so damage is only applied once

        float damage;

        CircleCollider collider;
        bool colEnabled;

        float ringRot;
        float ringRot2;
        float ringRot3;
        float ringRot4;
        float rippleScale;
        float time;
        float ringAlpha;
        bool triggeredShake;

        public BombExplosion(Vector3 pos, Game1 game):base()
        {
            this.pos = pos;
            this.game = game;
            scale = new Vector3(0.35f);
            model = ModelLibrary.bombRings;
            ripple = ModelLibrary.bombRipples;

            collider = new CircleCollider(pos, 5);

            ringRot = Utilities.nextFloat() * MathHelper.Pi * 2;
            ringRot2 = Utilities.nextFloat() * MathHelper.Pi * 2;
            ringRot3 = Utilities.nextFloat() * MathHelper.Pi * 2;
            ringRot4 = Utilities.nextFloat() * MathHelper.Pi * 2;

            rippleScale = 0.35f;
            ringAlpha = 1;
            triggeredShake = false;

            damage = 3;

            hitList = new List<Enemy>();

            sb = new SpriteBatch(game.GraphicsDevice);
            rippleTex = new RenderTarget2D(game.GraphicsDevice, 512, 128);
            texPos = new Vector2(0, -128);

            rot = new Vector3(Utilities.nextFloat() * MathHelper.PiOver4 - MathHelper.PiOver4/2, 0, Utilities.nextFloat() * MathHelper.PiOver4 - MathHelper.PiOver4/2);
        }

        void updateRingRots()
        {
            ringRot += Utilities.deltaTime * MathHelper.Pi * 5;
            if (ringRot > MathHelper.Pi * 2)
                ringRot -= MathHelper.Pi * 2;

            ringRot2 += Utilities.deltaTime * MathHelper.Pi * 5.4f;
            if (ringRot2 > MathHelper.Pi * 2)
                ringRot2 -= MathHelper.Pi * 2;

            ringRot3 += Utilities.deltaTime * MathHelper.Pi * 5.7f;
            if (ringRot3 > MathHelper.Pi * 2)
                ringRot3 -= MathHelper.Pi * 2;

            ringRot4 += Utilities.deltaTime * MathHelper.Pi * 6;
            if (ringRot4 > MathHelper.Pi * 2)
                ringRot4 -= MathHelper.Pi * 2;
        }

        void updateRipple()
        {
            texPos.Y = (float)((Math.Cos((time / 15) * MathHelper.PiOver2*3))+1) * 64-128;
            game.GraphicsDevice.SetRenderTarget(rippleTex);
            sb.Begin();
            sb.Draw(TextureManager.bombRipple, texPos, Color.White);
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Update(GameTime gameTime)
        {
            float frameTime = Utilities.deltaTime*60;

            if(time < 15)
            {
                scale -= new Vector3(0.025f)*frameTime;
                updateRipple();
            }
            else
            {
                if(!triggeredShake)
                {
                    colEnabled = true;
                    game.camera.makeShake();
                    game.hud.makeFlash();
                    triggeredShake = true;
                }
                if(collider.radius < 17)
                {
                    collider.radius += Utilities.deltaTime * 60;
                    if(collider.radius >= 17)
                        colEnabled = false;
                }
                //damage -= Utilities.deltaTime;
                if(colEnabled)
                    checkCollision();
                scale += new Vector3(0.05f)*frameTime;
                if (ringAlpha != 0)
                {
                    ringAlpha -= Utilities.deltaTime * 1.3f;
                    if (ringAlpha < 0)
                        ringAlpha = 0;
                }
            }
            updateRingRots();
            time += frameTime;
            if (time > 120)
            {
                game.modelManager.toDeleteModel(this);
                sb.Dispose();
                rippleTex.Dispose();
            }
        }

        void checkCollision()
        {
            // By moving each component of the vector one at a time and seeing what causes the collision we can eliminate only that component
            // this means the ship will slide along walls rather than stick. Doing two collision checks per frame for the player seems to
            // be within tolerable limits for CPU time. This will only need to be done with the player
            bool collided = false;

            //## COLLISIONS WHOOO! ##
            // Move the bounding box to new pos
            //circleCol.Update(pos, direction);
            // Get current node co-ordinates
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            try 
            {
                foreach (Enemy enemy in game.enemyManager.enemies)
                {
                    if (enemy.nodePos.X >= nodePos.X - 1 && enemy.nodePos.X <= nodePos.X + 1 &&
                        enemy.nodePos.Y >= nodePos.Y - 1 && enemy.nodePos.Y <= nodePos.Y + 1)
                    {
                        System.Diagnostics.Debug.WriteLine(hitList.Contains(enemy));
                        //if (hitList.Contains(enemy))
                        {
                            if (collider.checkPoint(enemy.pos))
                            {
                                enemy.health -= damage;
                                hitList.Add(enemy);
                                collided = true;
                            }
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
            }

            if (collided)
            {
            }
        }

        void drawRings(Texture2D tex, Camera camera)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name.Equals("ring1"))
                    {
                        effect.World = Matrix.CreateRotationY(ringRot2) * mesh.ParentBone.Transform * GetWorld();
                    }
                    else if (mesh.Name.Equals("ring2"))
                    {
                        effect.World = Matrix.CreateRotationY(ringRot4) * mesh.ParentBone.Transform * GetWorld();
                    }
                    else if (mesh.Name.Equals("ring3"))
                    {
                        effect.World = Matrix.CreateRotationY(ringRot3) * mesh.ParentBone.Transform * GetWorld();
                    }
                    else if (mesh.Name.Equals("ring8"))
                    {
                        effect.World = Matrix.CreateRotationY(ringRot2) * mesh.ParentBone.Transform * GetWorld();
                    }
                    else if (mesh.Name.Equals("ring5"))
                    {
                        effect.World = Matrix.CreateRotationY(ringRot) * mesh.ParentBone.Transform * GetWorld();
                    }
                    else if (mesh.Name.Equals("ring6"))
                    {
                        effect.World = Matrix.CreateRotationY(ringRot3) * mesh.ParentBone.Transform * GetWorld();
                    }
                    else
                    {
                        effect.World = Matrix.CreateRotationY(ringRot4) * mesh.ParentBone.Transform * GetWorld();
                    }

                    effect.View = camera.view;
                    effect.Projection = camera.projection;


                    effect.TextureEnabled = true;

                    effect.Texture = tex;



                    effect.Alpha = ringAlpha;

                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = Vector3.One;
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
                mesh.Draw();
            }
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            device.BlendState = BlendState.Additive;
            device.DepthStencilState = DepthStencilState.DepthRead;

            drawRings(TextureManager.bombRing, camera);
            if (time < 15)
            {
                drawRings(TextureManager.bombBlip, camera);

                transforms = new Matrix[ripple.Bones.Count];
                ripple.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in ripple.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        {
                            effect.World = Matrix.CreateScale(rippleScale) * Matrix.CreateRotationX(MathHelper.PiOver2 * 3) * Matrix.CreateBillboard(pos, camera.cameraPosition, camera.tiltUp, null);
                            //effect.World = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.PiOver2) * Matrix.CreateConstrainedBillboard(projectile.pos, game.camera.cameraPosition, Vector3.Cross(Vector3.Left, projectile.direction), null, null);
                        }

                        effect.View = camera.view;
                        effect.Projection = camera.projection;


                        effect.TextureEnabled = true;

                        effect.Texture = (Texture2D)rippleTex;



                        effect.Alpha = 1;

                        effect.LightingEnabled = true;
                        effect.AmbientLightColor = Vector3.One;
                        effect.PreferPerPixelLighting = true;

                    }
                    mesh.Draw();
                }
            }

            //base.Draw(device, camera);
        }
    }
}