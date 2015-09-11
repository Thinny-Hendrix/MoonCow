using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class WeaponSystem:GameComponent
    {
        Ship ship;
        int currentWeapon; //1=lasers, 2=missiles, 3=bomb, 4=shockwave, 5=drill 
        int laserPos; //0 = left, 1 = right
        float cooldown;
        bool hasDrill;
        Game1 game;

        Texture2D pew1;
        Texture2D pew2;
        Texture2D pew3;


        public List<Projectile> projectiles = new List<Projectile>();
        public List<Projectile> toDelete = new List<Projectile>();


        public WeaponSystem(Ship ship, Game game):base(game)
        {
            this.ship = ship;
            this.game = (Game1)game;
            currentWeapon = 1;
            laserPos = 0;

            pew1 = Game.Content.Load<Texture2D>(@"Models/Effects/tex1");
            pew2 = Game.Content.Load<Texture2D>(@"Models/Effects/tex2");
            pew3 = Game.Content.Load<Texture2D>(@"Models/Effects/tex3");


        }

        public override void Update(GameTime gameTime)
        {
            if ((Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed) && cooldown == 0)
                fire();

            if(GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)
            {
                if (currentWeapon != 5)
                    currentWeapon++;
                else
                    currentWeapon = 1;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
                currentWeapon = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
                currentWeapon = 2;
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
                currentWeapon = 3;
            if (Keyboard.GetState().IsKeyDown(Keys.D4))
                currentWeapon = 4;
            if (Keyboard.GetState().IsKeyDown(Keys.D5) && hasDrill)
                currentWeapon = 5;

            if (cooldown > 0)
            {
                cooldown -= Utilities.deltaTime * 60;
                if (cooldown < 0)
                    cooldown = 0;
            }
 
            foreach (Projectile p in projectiles)
            {
                p.update();
            }

            foreach (Projectile p in toDelete)
            {
                projectiles.Remove(p);
            }
            toDelete.Clear();
        }


        public void fire()
        {
            if (!ship.boosting && cooldown <= 0)
            {
                switch (currentWeapon)
                {
                    default:
                    case 1:
                        if (laserPos == 0)
                        {
                            projectiles.Add(new Projectile(ship.pos + new Vector3(Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction, (Game1)game, this, 1));
                            laserPos = 1;
                        }
                        else
                        {
                            projectiles.Add(new Projectile(ship.pos + new Vector3(-Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, -Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction, (Game1)game, this, 1));
                            laserPos = 0;
                        }
                        game.audioManager.shootLaser();
                        cooldown = 15;
                        break;
                    case 2:
                        if (laserPos == 0)
                        {
                            projectiles.Add(new Projectile(ship.pos + new Vector3(Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction, (Game1)game, this, 0));
                            laserPos = 1;
                        }
                        else
                        {
                            projectiles.Add(new Projectile(ship.pos + new Vector3(-Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, -Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction, (Game1)game, this, 0));
                            laserPos = 0;
                        }
                        cooldown = 15;
                        break;

                    case 3:
                        if (laserPos == 0)
                        {
                            projectiles.Add(new Projectile(ship.pos + new Vector3(Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction, (Game1)game, this, 2));
                            laserPos = 1;
                        }
                        else
                        {
                            projectiles.Add(new Projectile(ship.pos + new Vector3(-Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, -Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction, (Game1)game, this, 2));
                            laserPos = 0;
                        }
                        cooldown = 15;
                        break;

                    case 4:

                        break;
                        
                    case 5:

                        break;
                }
            }

        }
    
    }
}
