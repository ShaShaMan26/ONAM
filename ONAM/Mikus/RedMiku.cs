using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class RedMiku : Miku
{
    private int prevProg;
    private double doorDelay;
    private SFXObject thud;

    public RedMiku() : base("red-miku")
    {
        prevProg = 0;
        
        thud = new(Global.content.Load<SoundEffect>("sfx/thud"));
        thud.SetPan(-.7f);
    }

    public override void MakeMove()
    {
        prevProg = progress;
        progress++;
        if (progress > 3)
        {
            progress = 0;
            attacking = true;
            doorDelay = Global.random.NextDouble() * (4 - 1) + 1;
        }
        if (Global.night.camNum == progress || Global.night.camNum == prevProg) 
            Global.night.camView.InterruptCam(Global.night.camNum);
    }

    public override void UpdateAttack()
    {
        if (doorDelay > 0) doorDelay -= Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (doorDelay > 0) return;

        if (Global.night.office.door_eyes_l.opacity == 0)
        {
            counter = 0;

            Global.night.office.door_eyes_l.opacity = 0.1f;

            if (ModifierManager.letsGoGambling) Global.night.office.sign.OnArrive(true);
        }
        else if (Global.night.office.door_eyes_l.opacity < .9f)
        {
            Global.night.office.door_eyes_l.opacity += 0.01f;
        }
        
        if (!Global.night.office.door_L.IsEnterable())
        {
            if (tempHealth > 0) tempHealth -= Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (tempHealth > 0) return;

            attacking = false;
            Global.night.office.door_eyes_l.opacity = 0;
            progress = 1;
            counter = 0;
            tempHealth = health;
            if (Global.night.camNum == progress) Global.night.camView.InterruptCam(progress);
            AudioManager.AddSFX(thud);

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
