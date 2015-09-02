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
        SoundEffectInstance bgm;

        public AudioManager(Game game) : base(game)
        {
            bgmSpacePanic_base = game.Content.Load<SoundEffect>(@"Audio/BGM/Space Panic (base)");
            bgm = bgmSpacePanic_base.CreateInstance();
        }

        public override void Initialize()
        {
            bgm.IsLooped = true;
            bgm.Play();
            bgm.Volume = 0.5f;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void pause() //call this when you want to pause the game
        {
            SoundEffect.MasterVolume = 0.5f;
        }

        public void resume() //call this when you want to resume the game
        {
            SoundEffect.MasterVolume = 1.0f;
        }
    }
}
