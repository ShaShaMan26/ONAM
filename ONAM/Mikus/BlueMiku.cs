using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class BlueMiku : Miku
{
    private int enterHall, prevProg;
    private double doorDelay;
    private SFXObject hum, thud;

    public BlueMiku() : base("miku")
    {
        prevProg = 0;
        level = 1;
        moveDelay = 3.5;
        startDelay = 30;

        hum = new(Global.content.Load<SoundEffect>("sfx/sega"));
        hum.Volume = 0;
        thud = new(Global.content.Load<SoundEffect>("sfx/thud"));
        thud.Volume = .8f;
    }

    public override void MakeMove()
    {
        prevProg = progress;
        if (progress == 2)
        {
            enterHall = r.Next(3, 5);
            progress = enterHall;
        }
        else progress++;
        if (Global.camNum == progress || Global.camNum == prevProg) 
            Global.camView.InterruptCam(Global.camNum);
        if (progress > 4 || (enterHall == 3 && progress == 4))
        {
            progress = 0;
            attacking = true;
            doorDelay = r.NextDouble() * (12 - 1) + 1;
        }
    }

    // updated once a frame
    public override void UpdateAttack()
    {
        if (doorDelay > 0)
        {
            doorDelay -= Global.gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }

        if (hum.Volume == 0)
        {
            counter = 0;
            prevCounter = 0;

            if (enterHall == 3)
            {
                hum.SetPan(-.6f);
                thud.SetPan(-.7f);
            }
            else 
            {
                hum.SetPan(.6f);
                thud.SetPan(.7f);
            }
            hum.Volume = .8f;
            AudioManager.AddSFX(hum);
        }
        // if (hum.Volume < .3f)
        // {
        //     hum.Volume += .01f;
        // }
        // if (hum.PlaybackClosed)
        // {
        //     AudioManager.AddSFX(hum);
        // }
        
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter >= 9)
        { 
            attacking = false;
            hum.Stop();
            Global.office.jumpscarePNG.SetTexture(texture);
            Global.jumpytime = true;
        }
        else if (counter >= 3)
        {
            if ((enterHall == 3 && Global.doorClose_L) 
                || (enterHall > 3 && Global.doorClose_R))
            {
                if (counter - prevCounter < .5) return;
                attacking = false;
                hum.Volume = 0;
                progress = 1;
                AudioManager.AddSFX(thud);
            }
            prevCounter = counter;
        }
    }
}
