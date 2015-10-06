using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class AudioManager : Microsoft.Xna.Framework.GameComponent
    {
        //background music
        public SoundEffectInstance bgmSpacePanic_base = AudioLibrary.bgmSpacePanic_base.CreateInstance();
        public SoundEffectInstance bgmSpacePanic_spawn = AudioLibrary.bgmSpacePanic_spawn.CreateInstance();

        //ship
        public SoundEffectInstance shipSpaceEngine = AudioLibrary.shipSpaceEngine.CreateInstance();
        public SoundEffectInstance shipMetallicWallHit = AudioLibrary.shipMetallicWallHit.CreateInstance();
        public SoundEffectInstance shipMetallicWallScrape = AudioLibrary.shipMetallicWallScrape.CreateInstance();
        public SoundEffectInstance shipShootLaser = AudioLibrary.shipShootLaser.CreateInstance();
        public SoundEffectInstance shipShootLaser2 = AudioLibrary.shipShootLaser.CreateInstance();

        WaveManager waveManager;


        public AudioManager(Game1 game) : base(game)
        {
            waveManager = game.waveManager;
        }

        public override void Initialize()
        {
            SoundEffect.MasterVolume = 0.4f;

            bgmSpacePanic_base.IsLooped = true;
            bgmSpacePanic_base.Volume = 0.5f;
            bgmSpacePanic_base.Play();

            bgmSpacePanic_spawn.IsLooped = true;
            bgmSpacePanic_spawn.Volume = 0;
            bgmSpacePanic_spawn.Play();

            shipSpaceEngine.IsLooped = true;
            shipSpaceEngine.Volume = 0.1f;
            shipSpaceEngine.Play();

            shipMetallicWallHit.Volume = 0.1f;
            shipMetallicWallScrape.Volume = 0.1f;
            shipShootLaser.Volume = 0.1f;
            shipShootLaser2.Volume = 0.1f;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (waveManager.spawnState == Utilities.SpawnState.deploying && bgmSpacePanic_spawn.Volume < 0.5f)
            {
                bgmSpacePanic_spawn.Volume += 0.001f;
            }

            if (waveManager.spawnState != Utilities.SpawnState.deploying && bgmSpacePanic_spawn.Volume > 0)
            {
                try
                {
                    bgmSpacePanic_spawn.Volume -= 0.001f;
                } catch (ArgumentOutOfRangeException)
                {
                    bgmSpacePanic_spawn.Volume = 0;
                }
            }

            base.Update(gameTime);
        }
    }
}
