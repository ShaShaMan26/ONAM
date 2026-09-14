using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ONAM;

public class Intermission : InOffice
{
    private Texture2D office, officeRtn;
    private SFXObject powerDownSFX, error;
    private GameElement e;
    private double counter;
    private float[] ogShadows;
    private int[] ogLevels;

    public override void Initialize()
    {
        office = Global.content.Load<Texture2D>("office-blackout");
        powerDownSFX = new(Global.content.Load<SoundEffect>("sfx/power_down"));
        powerDownSFX.Volume = .5f;
        error = new(Global.content.Load<SoundEffect>("sfx/error"));
        base.Initialize();
    }

    public void OnStart()
    {
        counter = 0;
        e = new("office");
        e.color = Color.Black;
        e.opacity = 0.75f;
        Global.night.office.Add(9, e);

        Global.night.office.camBar.visible = false;
        int i = 0;
        ogShadows = new float[Global.night.office.mikulingManager.mikulings.Length];
        foreach (Mikuling m in Global.night.office.mikulingManager.mikulings)
        {
            ogShadows[i] = m.shadow;
            i++;
            m.shadow = .5f;
        }
        officeRtn = Global.night.office.bg.GetTexture();
        Global.night.office.bg.SetTexture(office);
        AudioManager.PauseBGM();
        AudioManager.AddSFX(powerDownSFX);
        
        LowerAI();
    }

    protected override void CheckAction()
    {
        if (KeyboardManager.KeyPressed(Keys.A) 
            || KeyboardManager.KeyPressed(Keys.D)
            || KeyboardManager.KeyPressed(Keys.S)
            || (MouseManager.LeftButtonClicked &&
            (Global.night.office.door_button_l.GetBounds().Contains(MouseManager.Location)
            || Global.night.office.door_button_r.GetBounds().Contains(MouseManager.Location))))
        {
            AudioManager.AddSFX(error);
        }
    }

    public override State Update()
    {
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter >= 10)
        {
            ResumeAI();
            Global.night.office.bg.SetTexture(officeRtn);
            AudioManager.ResumeBGM();
            powerDownSFX.Pause();
            AudioManager.RemoveSFX(powerDownSFX);
            e.opacity = 0;
            for (int i = 0; i < Global.night.office.mikulingManager.mikulings.Length; i++)
            {
                Global.night.office.mikulingManager.mikulings[i].shadow = ogShadows[i];
            }
            return Global.night.inOffice;
        }
        UpdateView();
        CheckAction();
        return null;
    }

    private void LowerAI()
    {
        ogLevels = new int[7];

        for (int i = 0; i < Global.night.mikus.Length; i++)
        {
            ogLevels[i] = Global.night.mikus[i].level;
            Global.night.mikus[i].level = (int) (Global.night.mikus[i].level / 1.15);
        }
        ogLevels[4] = Global.night.office.mikulingManager.level;
        Global.night.office.mikulingManager.level = (int) (Global.night.office.mikulingManager.level / 1.15);
        ogLevels[5] = Global.night.shadowMiku.level;
        Global.night.shadowMiku.level = (int) (Global.night.shadowMiku.level / 1.15);
        ogLevels[6] = Global.night.shadowOffice.level;
        Global.night.shadowOffice.level = (int) (Global.night.shadowOffice.level / 1.15);
    }
    private void ResumeAI()
    {
        for (int i = 0; i < ogLevels.Length; i++)
        {
            if (i < 4)
            {
                Global.night.mikus[i].level = ogLevels[i];
            }
            else if (i == 4)
            {
                Global.night.office.mikulingManager.level = ogLevels[i];
            }
            else if (i == 5)
            {
                Global.night.shadowMiku.level = ogLevels[i];
            }
            else if (i == 6)
            {
                Global.night.shadowOffice.level = ogLevels[i];
            }
        }
    }
}
