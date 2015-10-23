using System;
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
        public static Model stationBase;
        public static Model forceField;

        //Deco Nodes
        public static Model node60;

        //Weapons
        public static Model projectile;
        public static Model bombRipples;
        public static Model bombRings;
        public static Model bombProjectile;
        public static Model shockWave;

        //turrets
        public static Model turretBase;
        public static Model turretGatt;
        public static Model turretPyro;
        public static Model turretElec;
        public static Model turretHolo;

        //Enemies
        public static Model swarmer;
        public static Model sentry;
        public static Model sneaker;
        public static Model gunner;
        public static Model heavy;

        //swa
        public static Model swarmerAnim;
        public static Model swaFly1;
        public static Model swaFly2;
        public static Model swaNotice;
        public static Model swaAttack;
        public static Model swaHit;
        public static Model swaElec;
        public static Model swaIdle;

        //sneaker
        public static Model sneFly;
        public static Model sneStart;
        public static Model sneSpin;
        public static Model sneEnd;
        public static Model sneHit;
        public static Model sneElec;

        //gunner
        public static Model gunFly1;
        public static Model gunTrans;
        public static Model gunTrans2;
        public static Model gunShoot;
        public static Model gunRel;
        public static Model gunIdle;
        public static Model gunHit1;
        public static Model gunHit2;
        public static Model gunElec1;
        public static Model gunElec2;
        public static Model gunAttack1;
        public static Model gunAttack2;


        //hev
        public static Model hevFly;
        public static Model hevAttack;
        public static Model hevElec;
        public static Model hevHit;

        //Asteroid field
        public static Model ast1;
        public static Model mgScreen;

        //Minigame
        public static Model mgBgPoly;
        public static Model artefact;

        //Ships
        public static Model pewShip;
        public static Model drillShip;
        public static Model drillDome;

        //Money gibs
        public static Model oreGib1;
        public static Model drillItem;
        public static Model polyGib;
        public static Model chip;



        public static void initialize(Game game)
        {
            //nodes
            stationStraight = game.Content.Load<Model>(@"Models/StationTiles/straightRb");
            stationCorner = game.Content.Load<Model>(@"Models/StationTiles/cornerRb");
            stationTInt3 = game.Content.Load<Model>(@"Models/StationTiles/tint3Rb");
            stationTInt4 = game.Content.Load<Model>(@"Models/StationTiles/tint4Rb");
            stationDend = game.Content.Load<Model>(@"Models/StationTiles/dendRb");

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
            stationBase = game.Content.Load<Model>(@"Models/Base/hullProto");


            //Deco Nodes
            node60 = game.Content.Load<Model>(@"Models/BgTiles/node60");

            //Weapons
            projectile = game.Content.Load<Model>(@"Models/Weapons/shotEffectNew");
            bombRipples = game.Content.Load<Model>(@"Models/Weapons/bombImplode");
            bombRings = game.Content.Load<Model>(@"Models/Weapons/bombSploRings");
            bombProjectile = game.Content.Load<Model>(@"Models/Weapons/bombModel");
            shockWave = game.Content.Load<Model>(@"Models/Weapons/shockWave");


            //Turrets
            turretBase = game.Content.Load<Model>(@"Models/Turrets/turretBase");
            turretGatt = game.Content.Load<Model>(@"Models/Turrets/turretGatt");
            turretPyro = game.Content.Load<Model>(@"Models/Turrets/pyroTurret");
            turretElec = game.Content.Load<Model>(@"Models/Turrets/electroTurret");
            turretHolo = game.Content.Load<Model>(@"Models/Turrets/turrHolo");



            //Enemies
            swarmer = game.Content.Load<Model>(@"Models/Enemies/Swarmer/swarmProto2");
            sentry = game.Content.Load<Model>(@"Models/Enemies/Sentry/sentry");
            sneaker = game.Content.Load<Model>(@"Models/Enemies/Sneaker/sneakProto");
            gunner = game.Content.Load<Model>(@"Models/Enemies/Gunner/gunProto");
            heavy = game.Content.Load<Model>(@"Models/Enemies/Heavy/hevProto3");

            //swarmer
            swarmerAnim = game.Content.Load<Model>(@"Models/Enemies/Swarmer/swarmAnimProto");
            swaFly1 = game.Content.Load<Model>(@"Models/Enemies/Swarmer/swaFly1");
            swaFly2 = game.Content.Load<Model>(@"Models/Enemies/Swarmer/swaFly2");
            swaNotice = game.Content.Load<Model>(@"Models/Enemies/Swarmer/swaNotice");
            swaHit = game.Content.Load<Model>(@"Models/Enemies/Swarmer/swaHit");
            swaIdle = game.Content.Load<Model>(@"Models/Enemies/Swarmer/swaIdle");
            swaElec = game.Content.Load<Model>(@"Models/Enemies/Swarmer/swaElec");
            swaAttack = game.Content.Load<Model>(@"Models/Enemies/Swarmer/swaAttack");

            //sneaker
            sneFly = game.Content.Load<Model>(@"Models/Enemies/Sneaker/sneFly");
            sneStart = game.Content.Load<Model>(@"Models/Enemies/Sneaker/sneStart");
            sneSpin = game.Content.Load<Model>(@"Models/Enemies/Sneaker/sneSpin");
            sneEnd = game.Content.Load<Model>(@"Models/Enemies/Sneaker/sneEnd");
            sneHit = game.Content.Load<Model>(@"Models/Enemies/Sneaker/sneHit");
            sneElec = game.Content.Load<Model>(@"Models/Enemies/Sneaker/sneElec");

            //gunner
            gunFly1 = game.Content.Load<Model>(@"Models/Enemies/Gunner/gunFly1");
            gunTrans = game.Content.Load<Model>(@"Models/Enemies/Gunner/gunTrans");
            gunTrans2 = game.Content.Load<Model>(@"Models/Enemies/Gunner/gunTrans2");
            gunShoot = game.Content.Load<Model>(@"Models/Enemies/Gunner/gunShoot");
            gunRel = game.Content.Load<Model>(@"Models/Enemies/Gunner/gunRel");
            gunIdle = game.Content.Load<Model>(@"Models/Enemies/Gunner/gunIdle");
            gunHit1 = game.Content.Load<Model>(@"Models/Enemies/Gunner/gunHit1");
            gunHit2 = game.Content.Load<Model>(@"Models/Enemies/Gunner/gunHit2");
            gunElec1 = game.Content.Load<Model>(@"Models/Enemies/Gunner/gunElec1");
            gunElec2 = game.Content.Load<Model>(@"Models/Enemies/Gunner/gunElec2");
            gunAttack1 = game.Content.Load<Model>(@"Models/Enemies/Gunner/gunAttack1");
            gunAttack2 = game.Content.Load<Model>(@"Models/Enemies/Gunner/gunAttack2");


            //heavy
            hevFly = game.Content.Load<Model>(@"Models/Enemies/Heavy/hevFly");
            hevAttack = game.Content.Load<Model>(@"Models/Enemies/Heavy/hevAttack");
            hevHit = game.Content.Load<Model>(@"Models/Enemies/Heavy/hevHit");
            hevElec = game.Content.Load<Model>(@"Models/Enemies/Heavy/hevElec");

            //Asteroid field
            ast1 = game.Content.Load<Model>(@"Models/AstField/ast1");
            mgScreen = game.Content.Load<Model>(@"Models/Misc/mgScreen");

            //minigame
            mgBgPoly = game.Content.Load<Model>(@"Minigame/mgBgModel");
            artefact = game.Content.Load<Model>(@"Minigame/artefact");

            //ship
            pewShip = game.Content.Load<Model>(@"Models/Ship/PewProto/shipPewfbx");
            drillShip = game.Content.Load<Model>(@"Models/Ship/drillShipTEMP");
            drillDome = game.Content.Load<Model>(@"Models/Weapons/Drill/drillDome");


            //money gibs
            oreGib1 = game.Content.Load<Model>(@"Models/MoneyGibs/ore1");
            drillItem = game.Content.Load<Model>(@"Models/Weapons/Drill/drillCollect");
            polyGib = game.Content.Load<Model>(@"Models/MoneyGibs/polygib");
            chip = game.Content.Load<Model>(@"Minigame/chip");
        }
    }
}
