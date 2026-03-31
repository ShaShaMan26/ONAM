using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class GreenMiku : Miku
{
    public int prevProg, targetProg;
    private double attackDelay;
    private SFXObject crawl, leave;

    public GreenMiku() : base("green-miku")
    {
        prevProg = 0;
        progress = r.Next(5, 9);

        // startDelay = 6;
        moveDelay = 7;
        health = 1.5;
        attackDelay = 10;
        // level = 10;
        // level = 0;
        level = 20;

        tempHealth = health;
        crawl = new(Global.content.Load<SoundEffect>("sfx/vent_crawl"));
        leave = new(Global.content.Load<SoundEffect>("sfx/vent_leave"));

        ChooseTarget();
    }

    private void ChooseTarget()
    {
        do
        {
            targetProg = r.Next(5, 9);
        }
        while(targetProg == Global.sealedVentNum + 5);
    }

    private void Move()
    {
        if (progress == 2)
        {
            progress = targetProg;
            if (targetProg == 8) progress--;
        }
        else if (progress == 7 && targetProg == 8)
        {
            progress = 8;
        }
        else if (progress < 8)
        {
            progress = 2;
        }
        else if (progress == 8)
        {
            progress = 7;
        }
    }
    public override void MakeMove()
    {
        prevProg = progress;
        if (targetProg == progress)
        {
            progress = 0;
            attacking = true;
            counter = 0;
            AudioManager.AddSFX(crawl);
        }
        else
        {
            Move();
        }

        if (Global.camNum == progress || Global.camNum == prevProg) 
            Global.camView.InterruptCam(Global.camNum);
    }

    public override void UpdateAttack()
    {
        if (Global.sealedVentNum + 5 == prevProg)
        {
            if (counter < 3) return;
            if (tempHealth > 0) tempHealth -= Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (tempHealth > 0) return;
            
            attacking = false;
            ChooseTarget();
            progress = prevProg;
            tempHealth = health;
            if (Global.camNum == progress) Global.camView.InterruptCam(progress);
            AudioManager.AddSFX(leave);
            counter = 0;
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
