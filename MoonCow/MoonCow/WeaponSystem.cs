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
        Game game;

        Texture2D pew1;

        public List<Projectile> projectiles = new List<Projectile>();
        public List<Projectile> toDelete = new List<Projectile>();


        public WeaponSystem(Ship ship, Game game):base(game)
        {
            this.ship = ship;
            this.game = game;
            currentWeapon = 1;
            laserPos = 0;

            pew1 = Game.Content.Load<Texture2D>(@"Models/Misc/Rbow/rbowTunnelt");

        }

        public override void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Space) && cooldown == 0)
                fire();

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

            System.Console.WriteLine(cooldown);

            foreach (Projectile p in projectiles)
            {
                p.update();
            }

            foreach (Projectile p in toDelete)
            {
                projectiles.Remove(p);
            }
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
                            projectiles.Add(new Projectile(ship.pos + new Vector3(Vector3.Cross(Vector3.Up, ship.direction).X*0.25f,0, Vector3.Cross(Vector3.Up, ship.direction).Z), ship.direction, (Game1)game, pew1, this));
                            laserPos = 1;
                        }
                        else
                        {
                            projectiles.Add(new Projectile(ship.pos + new Vector3(Vector3.Cross(Vector3.Up, ship.direction).X * -0.25f, 0, Vector3.Cross(Vector3.Up, ship.direction).Z), ship.direction, (Game1)game, pew1, this));
                            laserPos = 0;
                        }

                        cooldown = 15;
                        break;
                    case 2:

                        break;

                    case 3:
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
