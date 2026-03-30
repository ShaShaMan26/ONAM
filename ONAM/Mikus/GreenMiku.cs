using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class GreenMiku : Miku
{
    public int prevProg, baseMoveDelay;
    private double leaveDelay;
    private SFXObject crawl, leave;

    public GreenMiku() : base("green-miku")
    {
        prevProg = 0;
        progress = r.Next(5, 9);
        // level = 10;
        level = 10;
        startDelay = 6;
        baseMoveDelay = 4;
        moveDelay = baseMoveDelay;
        leaveDelay = -1;

        crawl = new(Global.content.Load<SoundEffect>("sfx/vent_crawl"));
        leave = new(Global.content.Load<SoundEffect>("sfx/vent_leave"));
    }

    private void Move()
    {
        if (progress == 2)
        {
            progress = r.Next(5, 8);
        }
        else if (progress == 7)
        {
            if (r.Next(0, 2) > 0)
            {
                progress = 8;
            }
            else
            {
                progress = 2;
            }
        }
        else if (progress == 8)
        {
            progress = 7;
        }
        else
        {
            progress = 2;
        }
    }
    public override void MakeMove()
    {
        prevProg = progress;
        if (progress == 2 || r.Next(1, 4) > 1)
        {
            Move();
                moveDelay = baseMoveDelay;
            if (progress == 2) moveDelay /= 2;
        }
        else
        {
            progress = 0;
            attacking = true;
            counter = 0;
            AudioManager.AddSFX(crawl);
        }

        if (Global.camNum == progress || Global.camNum == prevProg) 
            Global.camView.InterruptCam(Global.camNum);
    }

    public override void UpdateAttack()
    {
        if (leaveDelay > 0)
        {
            leaveDelay -= Global.gameTime.ElapsedGameTime.TotalSeconds;
        }
        else if (leaveDelay > -1)
        {
            attacking = false;
            progress = prevProg;
            if (Global.camNum == progress) Global.camView.InterruptCam(progress);
            AudioManager.AddSFX(leave);
            leaveDelay = -1;
            counter = 0;
        }
        else if (counter >= 18)
        {
            attacking = false;
            Global.office.jumpscarePNG.SetTexture(texture);
            Global.jumpytime = true;
        }
        else if (counter > 1.5)
        {
            if (Global.sealedVentNum + 5 == prevProg)
            {
                leaveDelay = 2;
            }
        }
    }
}
