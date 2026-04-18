using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class SFXObject
{
    public float Volume { get; set; } = 1;
    public bool PlaybackClosed
    {
        get
        {
            return soundEffectInst.State.Equals(SoundState.Stopped);
        }
    }

    public readonly string fileName;
    public readonly double duration;
    public readonly SoundEffectInstance soundEffectInst;
    public float Pitch
    {
        get
        {
            return soundEffectInst.Pitch;
        }
        set
        {
            soundEffectInst.Pitch = value;
        }
    }
    private double playbackTimeElapsed = 0;

    public SFXObject(SoundEffect soundEffect) : base()
    {
        fileName = soundEffect.Name;
        duration = soundEffect.Duration.TotalSeconds;
        soundEffectInst = soundEffect.CreateInstance();

    }

    public void Update()
    {
        playbackTimeElapsed += Global.gameTime.TotalGameTime.TotalSeconds;

        if (soundEffectInst.State.Equals(SoundState.Stopped))
        {
            AudioManager.RemoveSFX(this);
        }
    }

    public void Play()
    {
        soundEffectInst.Play();
    }
    public void Pause()
    {
        soundEffectInst.Pause();
    }
    public void Stop()
    {
        soundEffectInst.Stop();
    }

    public void SetPan(float pan)
    {
        soundEffectInst.Pan = pan;
    }

    public override string ToString()
    {
        return fileName + " (" + playbackTimeElapsed.ToString("00.00") + "/" + duration.ToString("00.00") + ")";
    }
}