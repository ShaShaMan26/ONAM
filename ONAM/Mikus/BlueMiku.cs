using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class BlueMiku : Miku
{
    private int enterHall, prevProg;
    private double doorDelay;
    private SFXObject hum, thud, run, fake;

    public BlueMiku() : base("miku")
    {
        prevProg = 0;
        
        run = new(Global.content.Load<SoundEffect>("sfx/sega"));
        run.Volume = 0;
        fake = new(Global.content.Load<SoundEffect>("sfx/sega_scream"));
        fake.Volume = 0;
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
            enterHall = Global.random.Next(3, 5);
            if (enterHall == 3)
            {
                run.SetPan(-.4f);
                thud.SetPan(-.7f);
            }
            else 
            {
                run.SetPan(.4f);
                thud.SetPan(.7f);
            }
            fake.SetPan(run.GetPan() * -1);
            progress = enterHall;
        }
        else 
        {
            progress++;
            run.Volume += .2f;
        }
        if (progress > 4 || (enterHall == 3 && progress == 4))
        {
            progress = 0;
            attacking = true;
            doorDelay = Global.random.NextDouble() * (12 - 1) + 1;
        }
        else 
        {
            if (progress > 2 && ModifierManager.tricky && Global.random.Next(0, 2) > 0)
            {
                fake.Volume = run.Volume;
                AudioManager.AddSFX(fake);
            }
            else AudioManager.AddSFX(run);
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
            hum.Volume = .85f;
            AudioManager.AddSFX(hum);
        }
        
        if ((enterHall == 3 && !Global.night.office.door_L.IsEnterable()) 
            || (enterHall > 3 && !Global.night.office.door_R.IsEnterable()))
        {
            if (tempHealth > 0) tempHealth -= Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (tempHealth > 0) return;

            attacking = false;
            hum.Volume = 0;
            progress = 1;
            run.SetPan(0);
            run.Volume = 0;
            counter = 0;
            tempHealth = health;
            if (Global.night.camNum == progress) Global.night.camView.InterruptCam(progress);
            AudioManager.AddSFX(thud);
            AudioManager.RemoveSFX(hum);
            hum.Pause();

            Global.runData.mikusDeterred++;
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
