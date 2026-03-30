using Microsoft.Xna.Framework;
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
    public static Canvas canvas;
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

    public static void Initialize()
    {
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
}
