using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class ShadowMiku
{
    public int progress;
    public int prevProg, level;
    public double fadeProg;
    private double counter, spawnCount, spawnOpp, moveDelay;
    
    private SFXObject radio;

    public ShadowMiku()
    {
        RollSpawnOpp();
        level = 6;
        counter = 0;
        spawnCount = 0;
        prevProg = 0;
        progress = 0;
        fadeProg = 1;
        moveDelay = 15;

        radio = new(Global.content.Load<SoundEffect>("sfx/radio"));
        radio.Volume = 0;
    }

    private void UpdateRadio()
    {
        // direction
        // switch (progress)
        // {
        //     case 1:
        //     case 2:
        //         radio.SetPan(0);
        //         break;
        //     case 5:
        //     case 6:
        //         radio.SetPan(-.4f);
        //         break;
        //     case 3:
        //         radio.SetPan(-.2f);
        //         break;
        //     case 4:
        //         radio.SetPan(.2f);
        //         break;
        //     case 7:
        //     case 8:
        //         radio.SetPan(.4f);
        //         break;
        // }

        if (progress > 0) AudioManager.AddSFX(radio);
        else
        {
            radio.Volume = 0;
            radio.Stop();
            AudioManager.RemoveSFX(radio);
        }
        if (Global.night.jumpytime)
        {
            radio.Volume = 1;
            radio.SetPan(0);
        }
    }

    public void Update()
    {
        if (progress > 0)
        {
            if (Global.night.shadowMiku.progress == Global.night.camNum 
                && (Global.night.stateManager.currState.GetType() == typeof(InCams)
                || Global.night.stateManager.currState.GetType() == typeof(OpenCams))) 
            {
                fadeProg -= .4 * Global.gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (!Global.night.freezeTime)
            {
                fadeProg += .2 * Global.gameTime.ElapsedGameTime.TotalSeconds;
                counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
            }
            fadeProg = Math.Clamp(fadeProg, 0, 1);

            if (Global.night.freezeTime) return;

            if (radio.PlaybackClosed) AudioManager.AddSFX(radio);
            radio.Volume = (float) (.8d * (counter / moveDelay));
            AudioManager.UpdateSFXLevels(radio);
            
            if (fadeProg <= 0) 
            {
                counter = 0;
                progress = 0;
                spawnCount = 0;
                Global.night.camView.InterruptCam(Global.night.camNum);
                fadeProg = 1;
                UpdateRadio();
            }
            else if (counter >= moveDelay)
            {
                Global.night.office.jumpscarePNG.SetTexture(Global.content.Load<Texture2D>("shadow"));
                Global.night.jumpytime = true;
            }
        }
        else if (!Global.night.freezeTime) spawnCount += Global.gameTime.ElapsedGameTime.TotalSeconds;

        if (spawnCount >= spawnOpp)
        {
            if (level >= Global.random.Next(1, 21)) Spawn();
            spawnCount = 0;
            RollSpawnOpp();
        }
    }

    private void Spawn()
    {
        progress = Global.random.Next(1, 9);
        if (Global.night.camNum == progress)
        {
            Global.night.camView.InterruptCam(Global.night.camNum);
        }
        AudioManager.AddSFX(radio);
        UpdateRadio();
    }

    private void RollSpawnOpp()
    {
        spawnOpp = Global.random.Next(16, 23);
    }
}
