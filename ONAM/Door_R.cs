using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class Door_R : GameElement
{
    private SFXObject door_move, error, unjam;
    private Texture2D[] frames;
    private double counter;
    private int i;
    public DoorState doorState;
    private DoorState jamState;
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

        doorState = DoorState.OPEN;
        jamState = DoorState.CLOSING;
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
        if (doorState == DoorState.JAMMED)
        {
            if (stuckCount > 0) 
            {
                stuckCount--;
                AudioManager.AddSFX(unjam);
            }
            if (stuckCount < 1)
            {
                door_move.Play();
                doorState = jamState;
                Global.night.office.door_button_r.visible = doorState != DoorState.OPENING;
            }
        }

        if (ModifierManager.oneDoorAtATime && 
            !(Global.night.office.door_L.doorState == DoorState.OPENING && Global.night.office.door_L.stuckCount < 1)
                && Global.night.office.door_L.doorState != DoorState.OPEN)
        {
            AudioManager.AddSFX(error);
        }
        else if (doorState == DoorState.OPEN || doorState == DoorState.CLOSED)
        {   
            if (ModifierManager.doorStuck && r.Next(0, 10) > 8) stuckCount = 4;

            if (doorState == DoorState.OPEN)
            {
                doorState = DoorState.CLOSING;
                Global.night.office.door_button_r.visible = true;
                Global.runData.doorsClosed++;
            }
            else
            {
                doorState = DoorState.OPENING;
                if (stuckCount < 1) Global.night.office.door_button_r.visible = false;
            }
            AudioManager.AddSFX(door_move);
        }
    }

    public void Update()
    {
        if (stuckCount < 1 || i != frames.Length / 2)
        {
            if (doorState == DoorState.OPENING)
            {
                counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
                if (counter > Global.aniDelay || i == frames.Length)
                {
                    SetTexture(frames[i]);
                    i--;
                    counter = 0;
                    if(i < 0)
                    {
                        i = 0;
                        doorState = DoorState.OPEN;
                    }
                }
            }
            else if (doorState == DoorState.CLOSING)
            {
                counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
                if (counter > Global.aniDelay || i == 0)
                {
                    SetTexture(frames[i]);
                    i++;
                    counter = 0;
                    if(i >= frames.Length)
                    {
                        i = frames.Length - 1;
                        doorState = DoorState.CLOSED;
                    }
                }
            }
        }
        else if (i == frames.Length / 2 && doorState != DoorState.JAMMED)
        {
            door_move.Pause();
            AudioManager.AddSFX(error);
            jamState = doorState;
            doorState = DoorState.JAMMED;
        }
    }

    public bool IsEnterable()
    {
        return doorState != DoorState.CLOSED && doorState != DoorState.CLOSING;
    }
    public bool DrainingPower()
    {
        return doorState == DoorState.CLOSED || doorState == DoorState.CLOSING || doorState == DoorState.JAMMED || stuckCount > 0;
    }
}
