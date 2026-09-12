using System;
using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class ShadowOffice : GameElement
{
    private bool active;
    private double fadeProg, spawnCount, spawnOpp;
    public int level;
    
    private SFXObject breath, hum;

    public ShadowOffice() : base("shadow")
    {
        level = 4;
        spawnOpp = 8;
        spawnCount = 0;
        fadeProg = 0;
        opacity = 0;
        active = false;

        breath = new(Global.content.Load<SoundEffect>("sfx/smth_evil"));
        breath.Volume = .05f;
        hum = new(Global.content.Load<SoundEffect>("sfx/hum_loop"));
        hum.Volume = 0;
    }

    public void Update()
    {
        if (active)
        {
            if (Global.night.stateManager.currState.GetType() == typeof(InOffice))
            {
                if (fadeProg >= 1)
                {
                    Despawn();
                    Global.night.office.jumpscarePNG.SetTexture(texture);
                    Global.night.jumpytime = true;
                }
                else fadeProg += .15 * Global.gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (Global.night.stateManager.currState.GetType() == typeof(InCams))
            {
                fadeProg -= .2 * Global.gameTime.ElapsedGameTime.TotalSeconds;
                if (fadeProg <= 0)
                {
                    Despawn();
                }
            }
            fadeProg = Math.Clamp(fadeProg, 0, 1);
            opacity = .9f * (float) fadeProg;
            hum.Volume = (float) fadeProg;
            if (hum.PlaybackClosed) AudioManager.AddSFX(hum);
            else AudioManager.UpdateSFXLevels(hum);
        }
        else 
        {
            spawnCount += Global.gameTime.ElapsedGameTime.TotalSeconds;
            
            if (spawnCount >= spawnOpp)
            {
                if (Global.night.stateManager.currState.GetType() != typeof(InCams) && level >= Global.random.Next(1, 21)) Spawn();
                spawnCount = 0;
            }
        }
        
    }

    private void Spawn()
    {
        active = true;
        AudioManager.AddSFX(hum);
    }
    private void Despawn()
    {
        fadeProg = 0;
        opacity = 0;
        active = false;
        AudioManager.AddSFX(breath);
        AudioManager.PauseSFX(hum);
        AudioManager.RemoveSFX(hum);
        hum.Volume = 0;
    }
}
