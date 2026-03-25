using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class RedMiku : Miku
{
    private int prevProg;
    private double doorDelay;
    private SFXObject hum, thud;

    public RedMiku() : base("red-miku")
    {
        prevProg = 0;
        level = 6;
        startDelay = 10;
        
        hum = new(Global.content.Load<SoundEffect>("sfx/mikudayo"));
        hum.Volume = 0;
        hum.SetPan(-.6f);
        thud = new(Global.content.Load<SoundEffect>("sfx/thud"));
        thud.Volume = .8f;
        thud.SetPan(-.7f);
    }

    public override void MakeMove()
    {
        prevProg = progress;
        progress++;
        if (Global.camNum == progress || Global.camNum == prevProg) 
            Global.camView.InterruptCam(Global.camNum);
        if (progress > 3)
        {
            progress = 0;
            attacking = true;
            doorDelay = r.NextDouble() * (6 - 2) + 2;
        }
    }

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

            hum.Volume = .2f;
            Global.office.door_eyes_l.opacity = 0.1f;
        }
        if (hum.Volume < .4f)
        {
            hum.Volume += .001f;
        }
        if (Global.office.door_eyes_l.opacity < .9f)
        {
            Global.office.door_eyes_l.opacity += 0.01f;
        }
        if (hum.PlaybackClosed)
        {
            AudioManager.AddSFX(hum);
        }
        
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter >= 11)
        { 
            attacking = false;
            hum.Stop();
            Global.office.jumpscarePNG.SetTexture(texture);
            Global.jumpytime = true;
        }
        else if (counter >= 4)
        {
            if (Global.doorClose_L) 
            {
                if (counter - prevCounter < .5) return;
                attacking = false;
                hum.Volume = 0;
                Global.office.door_eyes_l.opacity = 0;
                progress = 1;
                AudioManager.AddSFX(thud);
            }
            prevCounter = counter;
        }
    }
}
