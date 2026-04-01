using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ONAM;

public static class Global
{
    private static bool RyanIsWatching = true;

    // program globals
    public static SpriteBatch spriteBatch;
    public static GraphicsDeviceManager graphics;
    public static ContentManager content;
    public static GameTime gameTime;

    // instance globals
    public static Canvas canvas, ui;
    public static StateManager stateManager;

    // canvases
    public static Office office;
    public static CamView camView;
    
    // states
    public static InOffice inOffice;
    public static InCams inCams;
    public static OpenCams openCams;
    public static CloseCams closeCams;
    public static Jumpscare jumpscare;
    public static SealingVent sealingVent;

    // instance variables
    public static bool doorClose_L, doorClose_R, jumpytime;
    public static int camNum, sealedVentNum;
    public static Miku[] mikus;

    private static Song bgm;
    private static SFXObject chime;
    private static double clockTime;
    private static int hour;
    private static TextDisplay clock, jumpClock;

    public static void Initialize()
    {
        chime = new(content.Load<SoundEffect>("sfx/clock_chime"));
        clockTime = 0;
        hour = 0;
        doorClose_L = false;
        doorClose_R = false;
        jumpytime = false;

        camNum = 3;
        sealedVentNum = 100;
        mikus = [new BlueMiku(), new RedMiku(), new YellowMiku(), new GreenMiku()];

        inOffice = new InOffice();
        inOffice.Initialize();
        inCams = new InCams();
        inCams.Initialize();
        jumpscare = new();
        jumpscare.Initialize();
        sealingVent = new();
        sealingVent.Initialize();

        office = new Office();
        office.Initialize();
        camView = new CamView();
        camView.Initialize();

        openCams = new OpenCams();
        openCams.Initialize();
        closeCams = new CloseCams();
        closeCams.Initialize();

        canvas = office;
        canvas.Initialize();
        stateManager = new StateManager(inOffice);
        stateManager.Initialize();

        ui = new();
        ui.Initialize();
        PopulateUI();

        bgm = content.Load<Song>("music/mall");
        AudioManager.MusicVolume = 0.15f;
        AudioManager.LoopingBGM = true;
        AudioManager.PlayBGM(bgm);
    }
    
    public static void UpdateMikus()
    {
        foreach (Miku m in mikus)
        {
            m.Update();
        }
    }

    public static void UpdateUI()
    {
        UpdateClock();
    }

    private static void UpdateClock()
    {
        clockTime += gameTime.ElapsedGameTime.TotalSeconds;
        if (clockTime >= 68)
        {
            if (hour < 5)
            {
                hour++;
                clock.Text = hour + " AM";
                clock.MapBoundsToTextSize();
                clock.SetPosition(graphics.PreferredBackBufferWidth - clock.GetWidth() - 24, 12);
                
                jumpClock.Text = clock.Text;
                jumpClock.MapBoundsToTextSize();
                jumpClock.SetPosition(graphics.PreferredBackBufferWidth / 2 - jumpClock.GetWidth() / 2 + 24,
                    -78);

                jumpClock.visible = true;
                clock.visible = false;

                clockTime = 0;
                AudioManager.AddSFX(chime);
            }
            else
            {
                // go to winscreen
            }
        }
        else if (jumpClock.visible && clockTime >= 1.5)
        {
            jumpClock.visible = false;
            clock.visible = true;
        }
    }
    private static void PopulateUI()
    {
        jumpClock = new("12 AM", "fnaf-big");
        jumpClock.SetPosition(0, -78);
        ui.Add(9, jumpClock);
        clock = new("12 AM", "fnaf");
        clock.SetPosition(graphics.PreferredBackBufferWidth - clock.GetWidth() - 24, 12);
        ui.Add(8, clock);
        clock.visible = false;
        AudioManager.AddSFX(chime);
    }
}
