using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class Door_R : GameElement
{
    private SFXObject door_move, error, unjam;
    private bool stop;
    public bool closing, opening, open;
    private Texture2D[] frames;
    private double counter;
    private int i;
    public int stuckCount;
    private Random r;

    public Door_R() : base("ani_door_r/0")
    {
        error = new(Global.content.Load<SoundEffect>("sfx/error"));
        error.SetPan(.5f);
        door_move = new(Global.content.Load<SoundEffect>("sfx/door_move"));
        door_move.Volume = .75f;
        door_move.SetPan(.5f);
        unjam = new(Global.content.Load<SoundEffect>("sfx/thud1"));
        unjam.SetPan(.5f);

        open = true;
        stop = true;
        stuckCount = 0;
        counter = 0;
        i = 0;

        frames = new Texture2D[16];
        for (int i = 0; i < frames.Length; i ++)
        {
            frames[i] = Global.content.Load<Texture2D>("ani_door_r/" + i);
        }
        
        r = new();
    }

    public void Toggle()
    {
        if (ModifierManager.doorStuck && !stop)
        {
            if (stuckCount > 0) 
            {
                stuckCount--;
                AudioManager.AddSFX(unjam);
            }
            if (stuckCount < 1)
            {
                door_move.Play();
                Global.night.office.door_button_r.visible = !opening;
                stop = true;
                Global.night.doorClose_R = !closing;
            }
        }

        if (ModifierManager.oneDoorAtATime && (Global.night.doorClose_L || Global.night.office.door_L.closing))
        {
            AudioManager.AddSFX(error);
        }
        else if (!closing && !opening)
        {   
            if (ModifierManager.doorStuck && r.Next(0, 10) > 8) stuckCount = 4;

            if (open)
            {
                closing = true;
                Global.night.office.door_button_r.visible = true;
            }
            else
            {
                opening = true;
                if (stuckCount < 1) Global.night.office.door_button_r.visible = false;
            }
            AudioManager.AddSFX(door_move);
        }
    }

    public void Update()
    {
        if (stuckCount < 1 || i != frames.Length / 2)
        {
            if (opening)
            {
                Global.night.doorClose_R = false;
                counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
                if (counter > Global.aniDelay || i == frames.Length)
                {
                    SetTexture(frames[i]);
                    i--;
                    counter = 0;
                    if(i < 0)
                    {
                        i = 0;
                        open = true;
                        opening = false;
                    }
                }
            }
            else if (closing)
            {
                counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
                if (stuckCount < 1) Global.night.doorClose_R = true;
                if (counter > Global.aniDelay || i == 0)
                {
                    SetTexture(frames[i]);
                    i++;
                    counter = 0;
                    if(i >= frames.Length)
                    {
                        i = frames.Length - 1;
                        open = false;
                        closing = false;
                    }
                }
            }
        }
        else if (i == frames.Length / 2 && stop)
        {
            door_move.Pause();
            AudioManager.AddSFX(error);
            stop = false;
        }
    }
}
