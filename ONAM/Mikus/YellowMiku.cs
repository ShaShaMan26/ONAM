using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class YellowMiku : Miku
{
    private int prevProg;
    private double doorDelay;
    private SFXObject thud;

    public YellowMiku() : base("yellow-miku")
    {
        prevProg = 0;

        thud = new(Global.content.Load<SoundEffect>("sfx/thud"));
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
        if (Global.night.camNum == progress || Global.night.camNum == prevProg) 
            Global.night.camView.InterruptCam(Global.night.camNum);
    }

    public override void UpdateAttack()
    {
        if (doorDelay > 0) doorDelay -= Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (doorDelay > 0) return;

        if (Global.night.office.door_eyes_r.opacity == 0)
        {
            counter = 0;

            Global.night.office.door_eyes_r.opacity = 0.1f;

            if (ModifierManager.letsGoGambling)
            {
                if (Global.night.office.sign.isRed) Global.night.office.sign.Lose();
                else Global.night.office.sign.Win();
            }
        }
        else if (Global.night.office.door_eyes_r.opacity < .9f)
        {
            Global.night.office.door_eyes_r.opacity += 0.01f;
        }
        

        if (!Global.night.office.door_R.IsEnterable())
        {
            if (tempHealth > 0) tempHealth -= Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (tempHealth > 0) return;

            attacking = false;
            Global.night.office.door_eyes_r.opacity = 0;
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
