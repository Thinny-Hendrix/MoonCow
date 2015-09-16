﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public static class ModelLibrary
    {
        //map nodes
        public static Model stationStraight;
        public static Model stationCorner;
        public static Model stationTInt3;
        public static Model stationTInt4;
        public static Model stationDend;

        //rails
        public static Model railStraight;
        public static Model railStraight1;
        public static Model railCorner;
        public static Model railTInt3;
        public static Model railTInt4;
        public static Model railCorner1big;
        public static Model railCorner1small;
        public static Model railCorner2small;
        public static Model railCorst;
        public static Model railCorstFlip;
        public static Model railDend;

        //Base models
        public static Model core;

        //Deco Nodes
        public static Model node60;

        //Weapons
        public static Model projectile;
        public static Model bombRipples;
        public static Model bombRings;
        public static Model bombProjectile;

        //turrets
        public static Model turretBase;
        public static Model turretGatt;

        //Enemies
        public static Model swarmer;


        //Ships




        public static void initialize(Game game)
        {
            //nodes
            stationStraight = game.Content.Load<Model>(@"Models/StationTiles/straightRound2");
            stationCorner = game.Content.Load<Model>(@"Models/StationTiles/cornerRound2");
            stationTInt3 = game.Content.Load<Model>(@"Models/StationTiles/tint3Round2");
            stationTInt4 = game.Content.Load<Model>(@"Models/StationTiles/tint4Round");
            stationDend = game.Content.Load<Model>(@"Models/StationTiles/dendRound2");

            //rails
            railStraight = game.Content.Load<Model>(@"Models/Rails/straight");
            railStraight1 = game.Content.Load<Model>(@"Models/Rails/straight1");
            railCorner = game.Content.Load<Model>(@"Models/Rails/corner");
            railTInt3 = game.Content.Load<Model>(@"Models/Rails/tInt3");
            railTInt4 = game.Content.Load<Model>(@"Models/Rails/tInt4");
            railCorner1big = game.Content.Load<Model>(@"Models/Rails/corner1big");
            railCorner1small = game.Content.Load<Model>(@"Models/Rails/corner1small");
            railCorner2small = game.Content.Load<Model>(@"Models/Rails/corner2small");
            railCorst = game.Content.Load<Model>(@"Models/Rails/corst");
            railCorstFlip = game.Content.Load<Model>(@"Models/Rails/corstflip");
            railDend = game.Content.Load<Model>(@"Models/Rails/dend");

            //Base models
            core = game.Content.Load<Model>(@"Models/Base/coreSphere");

            //Deco Nodes
            node60 = game.Content.Load<Model>(@"Models/BgTiles/node60");

            //Weapons
            projectile = game.Content.Load<Model>(@"Models/Weapons/shotEffectNew");
            bombRipples = game.Content.Load<Model>(@"Models/Weapons/bombImplode");
            bombRings = game.Content.Load<Model>(@"Models/Weapons/bombSploRings");
            bombProjectile = game.Content.Load<Model>(@"Models/Weapons/bombModel");

            //Turrets
            turretBase = game.Content.Load<Model>(@"Models/Turrets/turretBase");
            turretGatt = game.Content.Load<Model>(@"Models/Turrets/turretGatt");


            //Enemies
            swarmer = game.Content.Load<Model>(@"Models/Enemies/Swarmer/swarmerAlpha");

        }
    }
}
