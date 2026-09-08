using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class ShadowMiku
{
    public int progress;
    public int prevProg, level;
    public double fadeProg;
    private double counter, spawnCount, spawnOpp;
    
    private SFXObject radio;

    public ShadowMiku()
    {
        spawnOpp = 8;
        level = 5;
        counter = 0;
        spawnCount = 0;
        prevProg = 0;
        progress = 0;
        fadeProg = 1;

        radio = new(Global.content.Load<SoundEffect>("sfx/radio"));
    }

    private void MoveBack()
    {
        if (progress == 3 || progress == 4)
        {
            progress = 2;
        }
        else if (progress == 2)
        {
            progress = Global.random.Next(5, 10);
            if (progress == 9) progress = 1;
        }
        else if (progress == 1 || progress > 4)
        {
            // go away
            progress = 0;
        }
        fadeProg = 1;
    }
    private void MoveForward()
    {
        if (progress == 1 || progress > 4)
        {
            progress = 2;
        }
        else if (progress == 2)
        {
            progress = Global.random.Next(3, 5);
        }
        else if (progress > 2)
        {
            // kill
            Global.night.office.jumpscarePNG.SetTexture(Global.content.Load<Texture2D>("shadow"));
            Global.night.jumpytime = true;
        }
        fadeProg = 1;
    }
    private void UpdateRadio()
    {
        // direction
        switch (progress)
        {
            case 1:
            case 2:
                radio.SetPan(0);
                break;
            case 5:
            case 6:
                radio.SetPan(-.4f);
                break;
            case 3:
                radio.SetPan(-.2f);
                break;
            case 4:
                radio.SetPan(.2f);
                break;
            case 7:
            case 8:
                radio.SetPan(.4f);
                break;
        }
        // volume
        switch (progress)
        {
            case 2:
                radio.Volume = .3f;
                break;
            case 3:
            case 4:
                radio.Volume = .6f;
                break;
            case 6:
            case 5:
            case 7:
            case 8:
            case 1:
                radio.Volume = .1f;
                break;
        }
        if (progress > 0) AudioManager.AddSFX(radio);
        else
        {
            radio.Stop();
            AudioManager.RemoveSFX(radio);
        }
        if (Global.night.jumpytime)
        {
            radio.Volume = 1;
            radio.SetPan(0);
        }
    }
    private void MakeMove()
    {
        prevProg = progress;

        if (fadeProg <= 0) MoveBack();
        else if (counter >= 12) MoveForward();

        if (Global.night.camNum == progress || Global.night.camNum == prevProg) 
            Global.night.camView.InterruptCam(Global.night.camNum);
        counter = 0;

        UpdateRadio();
    }

    public void Update()
    {
        if (progress > 0)
        {
            if (radio.PlaybackClosed) AudioManager.AddSFX(radio);

            if (Global.night.shadowMiku.progress == Global.night.camNum 
                && (Global.night.stateManager.currState.GetType() == typeof(InCams)
                || Global.night.stateManager.currState.GetType() == typeof(OpenCams))) 
            {
                fadeProg -= .25 * Global.gameTime.ElapsedGameTime.TotalSeconds;
            }
            else 
            {
                fadeProg += .2 * Global.gameTime.ElapsedGameTime.TotalSeconds;
                counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
            }
            fadeProg = Math.Clamp(fadeProg, 0, 1);

            if (counter >= 12 || fadeProg <= 0) MakeMove();
        }
        else spawnCount += Global.gameTime.ElapsedGameTime.TotalSeconds;

        if (spawnCount >= spawnOpp)
        {
            if (level >= Global.random.Next(0, 21)) Spawn();
            spawnCount = 0;
        }
    }

    private void Spawn()
    {
        progress = Global.random.Next(3, 5);
        AudioManager.AddSFX(radio);
        UpdateRadio();
    }
}
