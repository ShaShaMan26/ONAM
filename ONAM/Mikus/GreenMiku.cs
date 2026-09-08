using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class GreenMiku : Miku
{
    public int prevProg, targetProg;
    private SFXObject crawl, leave, move, close, auto_voice;

    public GreenMiku() : base("green-miku")
    {
        prevProg = 0;
        progress = Global.random.Next(5, 9);

        crawl = new(Global.content.Load<SoundEffect>("sfx/vent_crawl"));
        leave = new(Global.content.Load<SoundEffect>("sfx/vent_leave"));
        move = new(Global.content.Load<SoundEffect>("sfx/run"));
        move.Volume = .35f;
        close = new(Global.content.Load<SoundEffect>("sfx/vent_close"));
        close.Volume = ModifierManager.autoSeal ? .1f : .9f;
        auto_voice = new(Global.content.Load<SoundEffect>("sfx/motion_trigger"));
        auto_voice.Volume = .75f;

        ChooseTarget();
    }

    private void ChooseTarget()
    {
        do
        {
            targetProg = Global.random.Next(5, 9);
        }
        while(targetProg == Global.night.sealedVentNum + 5);
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
        if (ModifierManager.hearGreen)
        {
            if (progress == 5 || progress == 6) move.SetPan(-.8f);
            else if (progress == 2) move.SetPan(0);
            else move.SetPan(.8f);
            AudioManager.AddSFX(move);
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
            if (ModifierManager.remoteSeal && Global.night.sealedVentNum + 5 != prevProg)
            {
                Global.night.camView.SealVent(prevProg - 5);
                Global.night.camView.seal_vent_bar_active.visible = Global.night.camNum == Global.night.sealedVentNum + 5;
                Global.night.currPower -= 270;
                AudioManager.AddSFX(auto_voice);
            }
            AudioManager.AddSFX(crawl);
        }
        else
        {
            Move();
        }

        if (Global.night.camNum == progress || Global.night.camNum == prevProg) 
            Global.night.camView.InterruptCam(Global.night.camNum);
    }

    public override void UpdateAttack()
    {
        if (Global.night.sealedVentNum + 5 == prevProg)
        {
            if (counter < 3) return;
            if (tempHealth > 0) tempHealth -= Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (tempHealth > 0) return;
            
            attacking = false;
            ChooseTarget();
            progress = prevProg;
            tempHealth = health;
            if (Global.night.camNum == progress) Global.night.camView.InterruptCam(progress);
            AudioManager.AddSFX(leave);
            counter = 0;

            Global.runData.mikusDeterred++;
        }
        else
        {
            if (counter >= attackDelay)
            {
                attacking = false;
                Global.night.office.jumpscarePNG.SetTexture(texture);
                Global.night.jumpytime = true;
            }
        }
    }
}
