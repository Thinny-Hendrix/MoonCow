using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MoonCow
{
    public class DisposableSoundEffect
    {
        SoundEffectInstance instance;
        List<DisposableSoundEffect> toDelete;
        public DisposableSoundEffect(SoundEffectInstance instance, float volume, List<DisposableSoundEffect> toDelete)
        {
            this.instance = instance;
            this.toDelete = toDelete;
            instance.IsLooped = false;
            instance.Volume = volume;
            instance.Play();
        }

        public DisposableSoundEffect(SoundEffectInstance instance, float volume, List<DisposableSoundEffect> toDelete, float pitch)
        {
            this.instance = instance;
            this.toDelete = toDelete;
            instance.IsLooped = false;
            instance.Volume = volume;
            instance.Pitch = pitch;
            instance.Play();
        }

        public void Update()
        {
            if(instance.State == SoundState.Stopped)
            {
                toDelete.Add(this);
                instance.Dispose();
            }
        }
    }
}
