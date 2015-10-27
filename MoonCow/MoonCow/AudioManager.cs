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
        public List<DisposableSoundEffect> soundEffects;
        public List<DisposableSoundEffect> sToDelete;
        //background music
        public SoundEffectInstance bgmSpacePanic_base = AudioLibrary.bgmSpacePanic_base.CreateInstance();
        public SoundEffectInstance bgmSpacePanic_spawn = AudioLibrary.bgmSpacePanic_spawn.CreateInstance();

        public SoundEffectInstance menuMusic = AudioLibrary.menuMusic.CreateInstance();

        //ship
        public SoundEffectInstance shipSpaceEngine = AudioLibrary.shipSpaceEngine.CreateInstance();
        public SoundEffectInstance shipMetallicWallHit = AudioLibrary.shipMetallicWallHit.CreateInstance();
        public SoundEffectInstance shipMetallicWallScrape = AudioLibrary.shipMetallicWallScrape.CreateInstance();
        public SoundEffectInstance shipShootLaser = AudioLibrary.shipShootLaser.CreateInstance();
        public SoundEffectInstance shipShootLaser2 = AudioLibrary.shipShootLaser.CreateInstance();
        public SoundEffectInstance shipShootBomb = AudioLibrary.shipShootBomb.CreateInstance();
        public SoundEffectInstance shipCollectMoney = AudioLibrary.shipCollectMoney.CreateInstance();
        public SoundEffectInstance shipShootMissile = AudioLibrary.shipShootMissile.CreateInstance();
        public SoundEffectInstance shipDrill = AudioLibrary.shipDrill.CreateInstance();

        //projectile FX
        public SoundEffectInstance bombExplode = AudioLibrary.bombExplode.CreateInstance();
        public SoundEffectInstance laserHit = AudioLibrary.laserHit.CreateInstance();
        public SoundEffectInstance zap = AudioLibrary.zap.CreateInstance();

        Game1 game;


        public AudioManager(Game1 game) : base(game)
        {
            this.game = game;
            soundEffects = new List<DisposableSoundEffect>();
            sToDelete = new List<DisposableSoundEffect>();
        }

        public override void Initialize()
        {
            if (game.runState == Game1.RunState.MainGame)
            {
                SoundEffect.MasterVolume = 0.4f;

                bgmSpacePanic_base.IsLooped = true;
                bgmSpacePanic_base.Volume = 1f;
                bgmSpacePanic_base.Play();

                bgmSpacePanic_spawn.IsLooped = true;
                bgmSpacePanic_spawn.Volume = 0;
                bgmSpacePanic_spawn.Play();

                shipSpaceEngine.IsLooped = true;
                shipSpaceEngine.Volume = 0.1f;
                shipSpaceEngine.Play();

                shipMetallicWallHit.Volume = 0.5f;
                shipMetallicWallScrape.Volume = 0.5f;
                shipShootLaser.Volume = 0.1f;
                shipShootLaser2.Volume = 0.1f;
                shipShootBomb.Volume = 1f;
                shipCollectMoney.Volume = 0.5f;
                shipShootMissile.Volume = 0.1f;
                shipDrill.IsLooped = true;
                shipDrill.Volume = 1f;

                bombExplode.Volume = 1f;
                laserHit.Volume = 0.5f;
                zap.Volume = 0.5f;
            }
            else
            {
                menuMusic.IsLooped = true;
                menuMusic.Volume = 0.7f;
                menuMusic.Play();
            }

            base.Initialize();
        }

        public void play3dSound(SoundEffectInstance sound, Vector3 soundPos)
        {
            AudioListener listener = new AudioListener();
            listener.Position = game.ship.pos;
            AudioEmitter emitter = new AudioEmitter();
            emitter.Position = soundPos;
            //sound.Apply3D(listener, emitter); throws An unhandled exception of type 'System.AccessViolationException' occurred in SharpDX.XAudio2.dll
            sound.Stop();
            sound.Play();
        }

        public void addSoundEffect(SoundEffect e, float vol)
        {
            soundEffects.Add(new DisposableSoundEffect(e.CreateInstance(), vol, sToDelete));
        }

        public override void Update(GameTime gameTime)
        {
            if (game.runState == Game1.RunState.MainGame)
            {
                if (game.waveManager.spawnState == Utilities.SpawnState.deploying && bgmSpacePanic_spawn.Volume < bgmSpacePanic_base.Volume)
                {
                    try
                    {
                        bgmSpacePanic_spawn.Volume += 0.001f;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        bgmSpacePanic_spawn.Volume = 1f;
                    }
                }

                if (game.waveManager.spawnState != Utilities.SpawnState.deploying && bgmSpacePanic_spawn.Volume > 0)
                {
                    try
                    {
                        bgmSpacePanic_spawn.Volume -= 0.001f;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        bgmSpacePanic_spawn.Volume = 0;
                    }
                }
            }

            foreach (DisposableSoundEffect e in soundEffects)
            {
                e.Update();
            }
            foreach (DisposableSoundEffect e in sToDelete)
            {
                soundEffects.Remove(e);
            }
            sToDelete.Clear();
            
        }

        public void shutup()
        {
            soundEffects.Clear();
            bgmSpacePanic_base.Stop();
            bgmSpacePanic_spawn.Stop();
            menuMusic.Stop();
            shipSpaceEngine.Stop();
        }
    }
}
