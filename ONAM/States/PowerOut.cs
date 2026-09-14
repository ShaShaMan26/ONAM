using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ONAM;

public class PowerOut : InOffice
{
    private Texture2D office;
    private SFXObject powerDownSFX, error;
    private double counter;

    public override void Initialize()
    {
        office = Global.content.Load<Texture2D>("office-blackout");
        powerDownSFX = new(Global.content.Load<SoundEffect>("sfx/power_down"));
        error = new(Global.content.Load<SoundEffect>("sfx/error"));
        base.Initialize();
    }

    public void OnStart()
    {
        counter = 0;
        GameElement e = new("office");
        e.color = Color.Black;
        e.opacity = 0.75f;
        Global.night.office.Add(9, e);

        Global.night.office.camBar.visible = false;
        foreach (Mikuling m in Global.night.office.mikulingManager.mikulings)
        {
            m.shadow = .75f;
        }

        foreach (Miku m in Global.night.mikus)
        {
            m.level = (int) (m.level / 1.25);
        }
        Global.night.office.mikulingManager.level = (int) (Global.night.office.mikulingManager.level / 1.25);
        Global.night.shadowMiku.level = (int) (Global.night.shadowMiku.level / 1.25);
        Global.night.shadowOffice.level = (int) (Global.night.shadowOffice.level / 1.25);

        Global.night.office.bg.SetTexture(office);
        AudioManager.PauseBGM();
        AudioManager.AddSFX(powerDownSFX);
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
        if (Global.night.office.door_R.doorState == DoorState.CLOSED) Global.night.office.door_R.Toggle();
        if (Global.night.office.door_L.doorState == DoorState.CLOSED) Global.night.office.door_L.Toggle();

        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter >= 34)
        {
            Global.night.office.jumpscarePNG.SetTexture(Global.content.Load<Texture2D>("shadow"));
            Global.night.jumpytime = true;
        }
        UpdateView();
        CheckAction();
        return null;
    }
}
