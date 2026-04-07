using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class BlueMiku : Miku
{
    private int enterHall, prevProg;
    private double doorDelay, attackDelay;
    private SFXObject hum, thud;

    public BlueMiku() : base("miku")
    {
        prevProg = 0;
        
        startDelay = 30;
        moveDelay = 3.5;
        health = 0;
        attackDelay = 9;
        // level = 2;
        // level = 1;
        level = 12;

        tempHealth = health;
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
        if (progress > 4 || (enterHall == 3 && progress == 4))
        {
            progress = 0;
            attacking = true;
            doorDelay = r.NextDouble() * (12 - 1) + 1;

            // if (level < 20) level++; // for demo only
        }
        if (Global.night.camNum == progress || Global.night.camNum == prevProg) 
            Global.night.camView.InterruptCam(Global.night.camNum);
    }

    // updated once a frame
    public override void UpdateAttack()
    {
        if (doorDelay > 0) doorDelay -= Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (doorDelay > 0) return;

        if (hum.Volume == 0)
        {
            counter = 0;

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
        
        if ((enterHall == 3 && Global.night.doorClose_L) 
            || (enterHall > 3 && Global.night.doorClose_R))
        {
            if (tempHealth > 0) tempHealth -= Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (tempHealth > 0) return;

            attacking = false;
            hum.Volume = 0;
            progress = 1;
            tempHealth = health;
            if (Global.night.camNum == progress) Global.night.camView.InterruptCam(progress);
            AudioManager.AddSFX(thud);
        }
        else
        {
            if (counter >= attackDelay)
            {
                attacking = false;
                hum.Stop();
                Global.night.office.jumpscarePNG.SetTexture(texture);
                Global.night.jumpytime = true;
            }
        }
    }
}
