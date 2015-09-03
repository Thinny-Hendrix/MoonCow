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
        SoundEffect bgmSpacePanic_base;
        SoundEffect sfxWallHit;
        SoundEffect sfxWallScrape;
        SoundEffect sfxShootLaser;
        SoundEffect sfxSpaceEngine;

        SoundEffectInstance bgm;
        SoundEffectInstance sfxiWallHit;
        SoundEffectInstance sfxiWallScrape;
        SoundEffectInstance sfxiShootLaser;
        SoundEffectInstance sfxiSpaceEngine;

        Ship ship;

        public AudioManager(Game1 game) : base(game)
        {
            this.ship = game.ship;

            bgmSpacePanic_base = game.Content.Load<SoundEffect>(@"Audio/BGM/Space Panic (base)");
            sfxWallHit = game.Content.Load<SoundEffect>(@"Audio/SFX/Metallic Wall Hit");
            sfxWallScrape = game.Content.Load<SoundEffect>(@"Audio/SFX/Metallic Wall Scrape");
            sfxShootLaser = game.Content.Load<SoundEffect>(@"Audio/SFX/Shoot Laser");
            sfxSpaceEngine = game.Content.Load<SoundEffect>(@"Audio/SFX/Space Engine");

            bgm = bgmSpacePanic_base.CreateInstance();
            sfxiWallHit = sfxWallHit.CreateInstance();
            sfxiWallScrape = sfxWallScrape.CreateInstance();
            sfxiShootLaser = sfxShootLaser.CreateInstance();
            sfxiSpaceEngine = sfxSpaceEngine.CreateInstance();
        }

        public override void Initialize()
        {
            bgm.IsLooped = true;
            bgm.Volume = 0.1f;
            //bgm.Play();

            sfxiWallHit.Volume = 0.02f;
            sfxiWallScrape.Volume = 0.02f;
            sfxiShootLaser.Volume = 0.008f;

            sfxiSpaceEngine.IsLooped = true;
            sfxiSpaceEngine.Volume = 0.01f;
            sfxiSpaceEngine.Play();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            sfxiSpaceEngine.Pitch = ship.moveSpeed * 3.3f - 1.0f;

            base.Update(gameTime);
        }

        public void volume(float volume)
        {
            SoundEffect.MasterVolume = volume;
        }

        public void shootLaser()
        {
            sfxiShootLaser.Stop();
            sfxiShootLaser.Play();
        }

        public void wallHit()
        {
            sfxiWallHit.Stop();
            sfxiWallHit.Play();
        }

        public void wallScrape()
        {
            sfxiWallScrape.Play();
        }

        public void wallScrapeStop()
        {
            sfxiWallScrape.Stop();
        }
    }
}
