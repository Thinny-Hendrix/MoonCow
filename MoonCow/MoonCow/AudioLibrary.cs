﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MoonCow
{
    public static class AudioLibrary
    {
        //background music
        public static SoundEffect bgmSpacePanic_base;
        public static SoundEffect bgmSpacePanic_spawn;

        public static SoundEffect menuMusic;

        //ship
        public static SoundEffect shipSpaceEngine;
        public static SoundEffect shipMetallicWallHit;
        public static SoundEffect shipMetallicWallScrape;
        public static SoundEffect shipShootLaser;
        public static SoundEffect shipShootBomb;
        public static SoundEffect shipCollectMoney;
        public static SoundEffect shipShootMissile;
        public static SoundEffect shipDrill;

        //projectile FX
        public static SoundEffect bombExplode;
        public static SoundEffect laserHit;
        public static SoundEffect zap;

        //sentry
        public static SoundEffect sentHit;
        public static SoundEffect sentShock;
        public static SoundEffect sentScream;

        //menu sounds
        public static SoundEffect back;
        public static SoundEffect hover;
        public static SoundEffect select;
        public static SoundEffect placeChip;
        public static SoundEffect itemCollect;
        public static SoundEffect mgGo;
        public static SoundEffect mgCount;
        public static SoundEffect consoleActivate;
        public static SoundEffect pyroTurret;

        //minigame
        public static SoundEffect miss;
        public static SoundEffect okay;
        public static SoundEffect perfect;
        public static SoundEffect end;

        public static void initialize(Game game)
        {
            //background music
            bgmSpacePanic_base = game.Content.Load<SoundEffect>(@"Audio/BGM/Space Panic (base)");
            bgmSpacePanic_spawn = game.Content.Load<SoundEffect>(@"Audio/BGM/Space Panic (spawn)");

            menuMusic = game.Content.Load<SoundEffect>(@"Audio/BGM/Celestial_Intrusion");

            //ship
            shipSpaceEngine = game.Content.Load<SoundEffect>(@"Audio/SFX/Space Engine");
            shipMetallicWallHit = game.Content.Load<SoundEffect>(@"Audio/SFX/Metallic Wall Hit");
            shipMetallicWallScrape = game.Content.Load<SoundEffect>(@"Audio/SFX/Metallic Wall Scrape");
            shipShootLaser = game.Content.Load<SoundEffect>(@"Audio/SFX/Shoot Laser");
            shipShootBomb = game.Content.Load<SoundEffect>(@"Audio/SFX/Shoot Bomb");
            shipCollectMoney = game.Content.Load<SoundEffect>(@"Audio/SFX/Collect Money");
            shipShootMissile = game.Content.Load<SoundEffect>(@"Audio/SFX/Shoot Missile");
            shipDrill = game.Content.Load<SoundEffect>(@"Audio/SFX/Drill");

            //projectile FX
            bombExplode = game.Content.Load<SoundEffect>(@"Audio/SFX/Bomb Explode");
            laserHit = game.Content.Load<SoundEffect>(@"Audio/SFX/LaserHit");
            zap = game.Content.Load<SoundEffect>(@"Audio/SFX/Zap");


            back = game.Content.Load<SoundEffect>(@"Audio/Menu/Menu Back");
            hover = game.Content.Load<SoundEffect>(@"Audio/Menu/Menu Hover");
            select = game.Content.Load<SoundEffect>(@"Audio/Menu/Menu Select");

            //minigame
            miss = game.Content.Load<SoundEffect>(@"Audio/SFX/Minigame/miss");
            okay = game.Content.Load<SoundEffect>(@"Audio/SFX/Minigame/okay");
            perfect = game.Content.Load<SoundEffect>(@"Audio/SFX/Minigame/perfect");
            end = game.Content.Load<SoundEffect>(@"Audio/SFX/Minigame/end");
            itemCollect = game.Content.Load<SoundEffect>(@"Audio/SFX/Minigame/itemCollect");
            placeChip = game.Content.Load<SoundEffect>(@"Audio/SFX/Minigame/placeChip");
            mgCount = game.Content.Load<SoundEffect>(@"Audio/SFX/Minigame/mgCount");
            mgGo = game.Content.Load<SoundEffect>(@"Audio/SFX/Minigame/mgGo");
            consoleActivate = game.Content.Load<SoundEffect>(@"Audio/SFX/Minigame/activate");
            pyroTurret = game.Content.Load<SoundEffect>(@"Audio/SFX/fireSound");


            //sentry
            sentHit = game.Content.Load<SoundEffect>(@"Audio/SFX/Enemies/Sentry/sentHit");
            sentScream = game.Content.Load<SoundEffect>(@"Audio/SFX/Enemies/Sentry/sentScream");
            sentShock = game.Content.Load<SoundEffect>(@"Audio/SFX/Enemies/Sentry/sentShock");

        }
    }
}
