using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class YellowMiku : Miku
{
    private int prevProg;
    private double doorDelay, attackDelay;
    private SFXObject thud;

    public YellowMiku() : base("yellow-miku")
    {
        prevProg = 0;

        startDelay = 12;
        health = .15;
        attackDelay = 11;
        // level = 5;
        level = 10;
        // level = 0;
        
        tempHealth = health;
        thud = new(Global.content.Load<SoundEffect>("sfx/thud"));
        thud.Volume = .8f;
        thud.SetPan(.7f);
    }

    public override void MakeMove()
    {
        prevProg = progress;
        progress++;
        if (progress == 3) progress = 4;
        if (progress > 4)
        {
            progress = 0;
            attacking = true;
            doorDelay = r.NextDouble() * (6 - 2) + 2;
        }
        if (Global.camNum == progress || Global.camNum == prevProg) 
            Global.camView.InterruptCam(Global.camNum);
    }

    public override void UpdateAttack()
    {
        if (doorDelay > 0) doorDelay -= Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (doorDelay > 0) return;

        if (Global.office.door_eyes_r.opacity == 0)
        {
            counter = 0;

            Global.office.door_eyes_r.opacity = 0.1f;
        }
        else if (Global.office.door_eyes_r.opacity < .9f)
        {
            Global.office.door_eyes_r.opacity += 0.01f;
        }
        

        if (Global.doorClose_R)
        {
            if (tempHealth > 0) tempHealth -= Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (tempHealth > 0) return;

            attacking = false;
            Global.office.door_eyes_r.opacity = 0;
            progress = 1;
            tempHealth = health;
            if (Global.camNum == progress) Global.camView.InterruptCam(progress);
            AudioManager.AddSFX(thud);
        }
        else
        {
            if (counter >= attackDelay)
            {
                attacking = false;
                Global.office.jumpscarePNG.SetTexture(texture);
                Global.jumpytime = true;
            }
        }
    }
}
