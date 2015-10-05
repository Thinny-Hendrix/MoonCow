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

        public static void initialize(Game game)
        {
            //background music
            bgmSpacePanic_base = game.Content.Load<SoundEffect>(@"Audio/BGM/Space Panic (base)");

            //ship
            shipSpaceEngine = game.Content.Load<SoundEffect>(@"Audio/SFX/Space Engine");
            shipMetallicWallHit = game.Content.Load<SoundEffect>(@"Audio/SFX/Metallic Wall Hit");
            shipMetallicWallScrape = game.Content.Load<SoundEffect>(@"Audio/SFX/Metallic Wall Scrape");
            shipShootLaser = game.Content.Load<SoundEffect>(@"Audio/SFX/Shoot Laser");
        }
    }
}
