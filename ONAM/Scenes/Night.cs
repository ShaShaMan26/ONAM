using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ONAM;

public class Night : Scene
{
    public StateManager stateManager;

    // canvases
    public Canvas canvas, ui;
    public Office office;
    public CamView camView;
    
    // states
    public InOffice inOffice;
    public InCams inCams;
    public OpenCams openCams;
    public CloseCams closeCams;
    public Jumpscare jumpscare;
    public SealingVent sealingVent;

    // instance variables
    public bool doorClose_L, doorClose_R, jumpytime;
    public int camNum, sealedVentNum;
    public Miku[] mikus;

    private Song bgm;
    private SFXObject chime;
    private double clockTime;
    private int hour;
    private TextDisplay clock, jumpClock;

    public override void Initialize()
    {
        chime = new(Global.content.Load<SoundEffect>("sfx/clock_chime"));
        clockTime = 0;
        hour = 0;
        doorClose_L = false;
        doorClose_R = false;
        jumpytime = false;

        camNum = 3;
        sealedVentNum = 100;
        mikus = [new BlueMiku(), new RedMiku(), new YellowMiku(), new GreenMiku()];

        office = new Office();
        office.Initialize();
        camView = new CamView();
        camView.Initialize();
        ui = new();
        ui.Initialize();
        PopulateUI();

        inOffice = new InOffice();
        inOffice.Initialize();
        inCams = new InCams();
        inCams.Initialize();
        jumpscare = new();
        jumpscare.Initialize();
        sealingVent = new();
        sealingVent.Initialize();

        openCams = new OpenCams();
        openCams.Initialize();
        closeCams = new CloseCams();
        closeCams.Initialize();

        canvas = office;
        canvas.Initialize();
        stateManager = new StateManager(inOffice);
        stateManager.Initialize();

        bgm = Global.content.Load<Song>("music/mall");
        AudioManager.MusicVolume = 0.15f;
        AudioManager.LoopingBGM = true;
        base.Initialize();
    }

    public override void OnStart()
    {
        AudioManager.PlayBGM(bgm);
        AudioManager.AddSFX(chime);
    }

    public override Scene Update()
    {
        if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape) && stateManager.currState.GetType() != typeof(Jumpscare))
        {
            Global.pause = new(this);
            Global.pause.Initialize();
            return Global.pause;
        }

        UpdateUI();
        stateManager.Update();
        office.door_L.Update();
        office.door_R.Update();
        if (!jumpytime) {
            UpdateMikus();
            office.mikulingManager.Update();
        }
        // if (!Global.jumpytime) Global.mikus[3].Update();
        if (jumpytime)
        {
            if (stateManager.currState.GetType() == typeof(InOffice))
            {
                jumpytime = false;
                stateManager.currState = jumpscare;
            }
            else if (stateManager.currState.GetType() == typeof(InCams))
            {
                stateManager.currState = closeCams;
            }
        }
        camView.UpdateAnimations();
        return null;
    }

    public override void Draw()
    {
        canvas.Draw();
        ui.Draw();
    }

    public void UpdateMikus()
    {
        foreach (Miku m in mikus)
        {
            m.Update();
        }
    }

    public void UpdateUI()
    {
        UpdateClock();
    }

    private void UpdateClock()
    {
        clockTime += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (clockTime >= 68)
        {
            if (hour < 5)
            {
                hour++;
                clock.Text = hour + " AM";
                clock.MapBoundsToTextSize();
                clock.SetPosition(Global.renderTarget.Width - clock.GetWidth() - 24, 12);
                
                jumpClock.Text = clock.Text;
                jumpClock.MapBoundsToTextSize();
                jumpClock.SetPosition(Global.renderTarget.Width / 2 - jumpClock.GetWidth() / 2 + 24,
                    -78);

                jumpClock.visible = true;
                clock.visible = false;

                clockTime = 0;
                AudioManager.AddSFX(chime);
            }
            else
            {
                // go to winscreen
                jumpytime = true;
                office.jumpscarePNG.SetTexture(Global.content.Load<Texture2D>("shadow"));
                AudioManager.AddSFX(new(Global.content.Load<SoundEffect>("sfx/yay")));
            }
        }
        else if (jumpClock.visible && clockTime >= 1.5)
        {
            jumpClock.visible = false;
            clock.visible = true;
        }
    }
    private void PopulateUI()
    {
        jumpClock = new("12 AM", "fnaf-big");
        jumpClock.SetPosition(0, -78);
        ui.Add(9, jumpClock);
        clock = new("12 AM", "fnaf");
        clock.SetPosition(Global.renderTarget.Width - clock.GetWidth() - 24, 12);
        ui.Add(8, clock);
        clock.visible = false;
    }
}
