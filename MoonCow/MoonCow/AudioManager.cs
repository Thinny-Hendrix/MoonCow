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
        SoundEffect sfxShootLaser2;
        SoundEffect sfxSpaceEngine;

        SoundEffectInstance bgm;
        SoundEffectInstance sfxiWallHit;
        SoundEffectInstance sfxiWallScrape;
        SoundEffectInstance sfxiShootLaser;
        SoundEffectInstance sfxiShootLaser2;
        SoundEffectInstance sfxiSpaceEngine;

        Ship ship;

        public AudioManager(Game1 game) : base(game)
        {
            this.ship = game.ship;

            bgmSpacePanic_base = game.Content.Load<SoundEffect>(@"Audio/BGM/Space Panic (base)");
            sfxWallHit = game.Content.Load<SoundEffect>(@"Audio/SFX/Metallic Wall Hit");
            sfxWallScrape = game.Content.Load<SoundEffect>(@"Audio/SFX/Metallic Wall Scrape");
            sfxShootLaser = game.Content.Load<SoundEffect>(@"Audio/SFX/Shoot Laser");
            sfxShootLaser2 = game.Content.Load<SoundEffect>(@"Audio/SFX/Shoot Laser");
            sfxSpaceEngine = game.Content.Load<SoundEffect>(@"Audio/SFX/Space Engine");

            bgm = bgmSpacePanic_base.CreateInstance();
            sfxiWallHit = sfxWallHit.CreateInstance();
            sfxiWallScrape = sfxWallScrape.CreateInstance();
            sfxiShootLaser = sfxShootLaser.CreateInstance();
            sfxiShootLaser2 = sfxShootLaser2.CreateInstance();
            sfxiSpaceEngine = sfxSpaceEngine.CreateInstance();
        }

        public override void Initialize()
        {
            bgm.IsLooped = true;
            bgm.Volume = 0.4f;
            bgm.Play();

            sfxiWallHit.Volume = 0.08f;
            sfxiWallScrape.Volume = 0.08f;
            sfxiShootLaser.Volume = 0.04f;
            sfxiShootLaser2.Volume = 0.04f;

            sfxiSpaceEngine.IsLooped = true;
            sfxiSpaceEngine.Volume = 0.02f;
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

        public void shootLaser2()
        {
            sfxiShootLaser2.Stop();
            sfxiShootLaser2.Play();
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
