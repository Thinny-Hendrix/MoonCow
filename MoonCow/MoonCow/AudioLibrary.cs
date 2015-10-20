using System;
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

        //ship
        public static SoundEffect shipSpaceEngine;
        public static SoundEffect shipMetallicWallHit;
        public static SoundEffect shipMetallicWallScrape;
        public static SoundEffect shipShootLaser;
        public static SoundEffect shipShootBomb;
        public static SoundEffect shipCollectMoney;

        //projectile FX
        public static SoundEffect bombExplode;
        public static SoundEffect laserHit;
        public static SoundEffect zap;

        //sentry
        public static SoundEffect sentryHit0;

        public static void initialize(Game game)
        {
            //background music
            bgmSpacePanic_base = game.Content.Load<SoundEffect>(@"Audio/BGM/Space Panic (base)");
            bgmSpacePanic_spawn = game.Content.Load<SoundEffect>(@"Audio/BGM/Space Panic (spawn)");

            //ship
            shipSpaceEngine = game.Content.Load<SoundEffect>(@"Audio/SFX/Space Engine");
            shipMetallicWallHit = game.Content.Load<SoundEffect>(@"Audio/SFX/Metallic Wall Hit");
            shipMetallicWallScrape = game.Content.Load<SoundEffect>(@"Audio/SFX/Metallic Wall Scrape");
            shipShootLaser = game.Content.Load<SoundEffect>(@"Audio/SFX/Shoot Laser");
            shipShootBomb = game.Content.Load<SoundEffect>(@"Audio/SFX/Shoot Bomb");
            shipCollectMoney = game.Content.Load<SoundEffect>(@"Audio/SFX/Collect Money");

            //projectile FX
            bombExplode = game.Content.Load<SoundEffect>(@"Audio/SFX/Bomb Explode");
            laserHit = game.Content.Load<SoundEffect>(@"Audio/SFX/LaserHit");
            zap = game.Content.Load<SoundEffect>(@"Audio/SFX/Zap");

            //sentry
            //sentryHit0 = game.Content.Load<SoundEffect>(@"Audio/SFX/Enemies/Sentry/sentHit1");

        }
    }
}
