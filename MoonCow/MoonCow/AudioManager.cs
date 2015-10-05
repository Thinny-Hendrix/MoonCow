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

        //ship
        public SoundEffectInstance shipSpaceEngine = AudioLibrary.shipSpaceEngine.CreateInstance();
        public SoundEffectInstance shipMetallicWallHit = AudioLibrary.shipMetallicWallHit.CreateInstance();
        public SoundEffectInstance shipMetallicWallScrape = AudioLibrary.shipMetallicWallScrape.CreateInstance();
        public SoundEffectInstance shipShootLaser = AudioLibrary.shipShootLaser.CreateInstance();
        public SoundEffectInstance shipShootLaser2 = AudioLibrary.shipShootLaser.CreateInstance();


        public AudioManager(Game1 game) : base(game)
        {

        }

        public override void Initialize()
        {
            SoundEffect.MasterVolume = 0.4f;

            bgmSpacePanic_base.IsLooped = true;
            bgmSpacePanic_base.Volume = 0.5f;
            bgmSpacePanic_base.Play();

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
            base.Update(gameTime);
        }
    }
}
