using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

namespace ONAM;

public static class AudioManager
{
    public static Song BackgroundMusic { get; private set; }
    public static bool PlayingBGM { get; private set; }
    public static bool LoopingBGM
    {
        get
        {
            return MediaPlayer.IsRepeating;
        }
        set
        {
            MediaPlayer.IsRepeating = value;
        }
    }
    public static bool Muted
    {
        get
        {
            return MediaPlayer.IsMuted;
        }
        set
        {
            MediaPlayer.IsMuted = value;
        }
    }
    public static float MusicVolume { get; set; } = 1;
    public static float SFXVolume { get; set; } = 1;

    public static List<SFXObject> SFXObjects { get; } = [];
    private static List<SFXObject> QueuedSFXObjects { get; } = [];
    private static List<SFXObject> ls = [];

    public static void SetBGM(Song bgm)
    {
        BackgroundMusic = bgm;
        ResetBGM();
    }
    public static void PlayBGM(Song bgm)
    {
        MediaPlayer.Volume = MusicVolume;
        MediaPlayer.Play(bgm);
        BackgroundMusic = bgm;
        PlayingBGM = true;
    }
    public static void ResumeBGM()
    {
        MediaPlayer.Resume();
        PlayingBGM = true;
    }
    public static void PauseBGM()
    {
        MediaPlayer.Pause();
        PlayingBGM = false;
    }
    public static void ResetBGM()
    {
        MediaPlayer.Play(BackgroundMusic);
    }

    public static void AddSFX(SFXObject sfxObject)
    {
        sfxObject.Stop();
        SFXObjects.Add(sfxObject);
        QueuedSFXObjects.Add(sfxObject);
    }
    public static void RemoveSFX(SFXObject sfxObject)
    {
        QueuedSFXObjects.Remove(sfxObject);
        SFXObjects.Remove(sfxObject);
    }

    public static void PauseSFXAll()
    {
        foreach (SFXObject sfXObject in SFXObjects)
        {
            sfXObject.Pause();
        }
    }
    public static void CloseSFXAll()
    {
        foreach (SFXObject sfXObject in SFXObjects)
        {
            sfXObject.Pause();
        }
        QueuedSFXObjects.Clear();
        SFXObjects.Clear();
    }
    public static void PauseSFX(SFXObject sfxObject)
    {
        sfxObject.Pause();
    }
    public static void PlaySFXAll()
    {
        foreach (SFXObject sfXObject in SFXObjects)
        {
            PlaySFX(sfXObject);
        }
    }
    public static void PlaySFX(SFXObject sfxObject)
    {
        sfxObject.soundEffectInst.Volume = sfxObject.Volume * SFXVolume;
        sfxObject.Play();
    }
    public static void UpdateSFXLevels(SFXObject sfxObject)
    {
        sfxObject.soundEffectInst.Volume = sfxObject.Volume * SFXVolume;
    }

    public static void Update()
    {
        QueuedSFXObjects.ForEach(PlaySFX);
        QueuedSFXObjects.Clear();
        SFXObjects.ForEach(s => { if (s.PlaybackClosed) ls.Add(s); });
        ls.ForEach(RemoveSFX);
        ls.Clear();
    }
}