﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class ShipParticleSystem:GameComponent
    {
        Game1 game;
        Ship ship;
        List<BasicModel> moneyParticles = new List<BasicModel>();
        public List<BasicModel> moneyToDelete = new List<BasicModel>();
        List<BasicModel> thrustParticles = new List<BasicModel>();
        public List<BasicModel> thrustToDelete = new List<BasicModel>();
        public bool wPress;

        float thrustGenTime;
        int thrustParticle;
        bool currentlyBoosting;
        bool currentlyMoving;


        public ShipParticleSystem(Game1 game, Ship ship):base(game)
        {
            this.game = game;
            this.ship = ship;

            addThrustParticle(0);
            thrustGenTime = 0;
            currentlyBoosting = false;
            currentlyMoving = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if (ship.boosting)
                {
                    if (!currentlyBoosting)
                    {
                        currentlyBoosting = true;
                        addThrustParticle(5);
                        addThrustParticle(4);
                        addThrustParticle(4);
                    }
                }
                else
                    currentlyBoosting = false;

                if (ship.moving)
                {
                    if (!currentlyMoving)
                    {
                        currentlyMoving = true;
                        addThrustParticle(2);
                        addThrustParticle(2);
                        addThrustParticle(1);
                        addThrustParticle(1);
                    }
                }
                else
                    currentlyMoving = false;

                bool newThrustParticle = ship.moving || ship.inUTurn;
                if (newThrustParticle)
                {
                    thrustGenTime += Utilities.deltaTime * 60;
                    if (thrustGenTime >= 2)
                    {
                        if (thrustParticle == 0)
                        {
                            if (currentlyBoosting)
                            {
                                addThrustParticle(-1);
                                addThrustParticle(4);
                                addThrustParticle(4);
                            }
                            else
                                addThrustParticle(1);

                            thrustParticle = 1;
                        }
                        else
                        {
                            if (currentlyBoosting)
                            {
                                addThrustParticle(4);
                                addThrustParticle(4);
                                addThrustParticle(3);

                            }
                            else
                                addThrustParticle(2);
                            thrustParticle = 0;
                        }
                        thrustGenTime = 0;
                    }
                }
                else
                    thrustGenTime = 2;

                deleteFromList(moneyParticles, moneyToDelete);
                deleteFromList(thrustParticles, thrustToDelete);
            }
        }

        void deleteFromList(List<BasicModel> source, List<BasicModel> toDelete)
        {
            foreach(BasicModel m in toDelete)
            {
                game.modelManager.removeEffect(m);
                source.Remove(m);
            }
            toDelete.Clear();
        }

        void addThrustParticle(int type)
        {
            ShipThrustParticle p = new ShipThrustParticle(game, ship, this, type);
            game.modelManager.addEffect(p);
            thrustParticles.Add(p);

        }

        public void addMoneyParticle(Color col)
        {
            MoneyCollectParticle p = new MoneyCollectParticle(game, ship, col);
            game.modelManager.addEffect(p);
            moneyParticles.Add(p);
        }

    }
}
