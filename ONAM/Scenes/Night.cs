using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ONAM;

public class Night : Scene
{
    public StateManager stateManager;

    // subscenes
    private Pause pause;
    private GameWin gameWin;

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
    public PowerOut powerOut;

    // instance variables
    public bool doorClose_L, doorClose_R, jumpytime;
    public int camNum, sealedVentNum;
    public Miku[] mikus;

    private Song bgm;
    private SFXObject chime;
    private double clockTime, powerCounter;
    private int hour, totalPower, currPower;
    private TextDisplay clock, jumpClock, powerPercent;
    private GameElement powerIndicator;
    private Texture2D[] powerIndicatorTextures;

    public override void Initialize()
    {
        totalPower = 11220;
        currPower = totalPower;
        powerCounter = 0;

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
        powerOut = new();
        powerOut.Initialize();

        openCams = new OpenCams();
        openCams.Initialize();
        closeCams = new CloseCams();
        closeCams.Initialize();

        canvas = office;
        canvas.Initialize();
        stateManager = new StateManager(inOffice);
        stateManager.Initialize();

        bgm = Global.content.Load<Song>("music/mall");
        AudioManager.LoopingBGM = true;

        pause = new(this);
        pause.Initialize();
        gameWin = new();
        gameWin.Initialize();
        base.Initialize();
    }

    public override void OnStart()
    {
        AudioManager.PlayBGM(bgm);
        AudioManager.AddSFX(chime);
    }

    public override Scene Update()
    {
        // testing puroposes only REMOVE later
        if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.OemPlus))
        {
            clockTime = 100;
        }
        else if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.OemMinus))
        {
            currPower -= 68 * 30;
            if (currPower < 0) currPower = 1;
        }

        if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape) && stateManager.currState.GetType() != typeof(Jumpscare))
        {
            pause.OnStart();
            return pause;
        }

        if (currPower >= 0) UpdateUI();
        if (currPower <= 0 && stateManager.currState.GetType() != typeof(PowerOut))
        {
            if (currPower > -1) 
            {
                currPower = -1;
                powerOut.OnStart();
                if (doorClose_R || office.door_R.closing) office.door_R.Toggle();
                if (doorClose_L || office.door_L.closing) office.door_L.Toggle();
            }
            if (stateManager.currState.GetType() != typeof(InOffice)) stateManager.currState = closeCams;
            else
            {
                stateManager.currState = powerOut;
            }
        }
        stateManager.Update();
        office.door_L.Update();
        office.door_R.Update();
        if (!jumpytime) {
            UpdateMikus();
            office.mikulingManager.Update();
        }
        if (jumpytime)
        {
            if (stateManager.currState.GetType() == typeof(InOffice)
                || stateManager.currState.GetType() == typeof(PowerOut))
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
        if (Global.sceneManager.currScene.GetType() != typeof(Pause) 
            && Global.sceneManager.currScene.GetType() != typeof(GameWin) 
            && stateManager.currState.GetType() != typeof(Jumpscare)
            && stateManager.currState.GetType() != typeof(PowerOut)) ui.Draw();
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
        UpdatePower();
    }

    private void UpdatePower()
    {
        int i = 0;
        if (doorClose_L) i++;
        if (doorClose_R) i++;
        if (stateManager.currState.GetType() == typeof(InCams) || stateManager.currState.GetType() == typeof(SealingVent)) i++;

        if (i == 0)
        {
            powerIndicator.visible = false;
        }
        else
        {
            powerIndicator.visible = true;
            powerIndicator.SetTexture(powerIndicatorTextures[i - 1]);
        }

        powerCounter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (powerCounter >= Global.aniDelay)
        {
            currPower -= i;
            if (currPower < 0) currPower = 0;
            powerPercent.Text = (currPower / (double) totalPower * 100).ToString("F0") + "%";
            powerCounter = 0;
        }
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
                gameWin.OnStart();
                Global.sceneManager.currScene = gameWin;
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

        TextDisplay t = new("Usage:", "fnaf-small");
        t.MapBoundsToTextSize();
        t.SetPosition(26, Global.renderTarget.Height - t.GetHeight() - 18);
        ui.Add(8, t);

        powerIndicatorTextures = new Texture2D[3];
        for (int i = 1; i <= powerIndicatorTextures.Length; i++)
        {
            powerIndicatorTextures[i - 1] = Global.content.Load<Texture2D>("power" + i);
        }
        powerIndicator = new("power1");
        powerIndicator.visible = false;
        powerIndicator.SetPosition(t.GetPosition().X + t.GetWidth() + 6, t.GetPosition().Y + 8);
        ui.Add(8, powerIndicator);

        TextDisplay j = new("Power Left: ", "fnaf-small");
        j.MapBoundsToTextSize();
        j.SetPosition(t.GetPosition().X, t.GetPosition().Y - j.GetHeight() + 10);
        ui.Add(8, j);
        
        powerPercent = new("100%", "fnaf");
        powerPercent.SetPosition(j.GetPosition().X + j.GetWidth() - 2, j.GetPosition().Y - 4);
        ui.Add(8, powerPercent);
    }
}
