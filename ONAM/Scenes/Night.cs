using System;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ONAM;

public class Night : Scene
{
    public StateManager stateManager;
    private Random r;

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
    public ClearingGas clearingGas;
    public PowerOut powerOut;
    public Intermission intermission;

    // instance variables
    public bool doorClose_L, doorClose_R, jumpytime, intermissionTime;
    public int camNum, sealedVentNum, totalPower;
    public Miku[] mikus;
    public ShadowMiku shadowMiku;
    public ShadowOffice shadowOffice;

    private Song bgm;
    private SFXObject chime, gas, coin;
    private double powerCounter;
    private int hour;
    public double currPower, clockTime;
    private TextDisplay clock, loop, jumpClock, jumpLoop, jumpLoopNum, powerPercent;
    private GameElement powerIndicator;
    private Texture2D[] powerIndicatorTextures;
    private GameElement coinIcon;
    private TextDisplay coinCounter;

    public override void Initialize()
    {
        Global.runData.tokens = 0;

        r = new();

        chime = new(Global.content.Load<SoundEffect>("sfx/clock_chime"));
        gas = new(Global.content.Load<SoundEffect>("sfx/gas"));
        coin = new(Global.content.Load<SoundEffect>("sfx/get_coin"));
        coin.Volume = .3f;

        clockTime = 0;
        hour = 0;
        doorClose_L = false;
        doorClose_R = false;
        jumpytime = false;

        camNum = 3;
        sealedVentNum = 100;
        mikus = [new BlueMiku(), new RedMiku(), new YellowMiku(), new GreenMiku()];
        shadowMiku = new();
        shadowOffice = new();

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
        clearingGas = new();
        clearingGas.Initialize();
        powerOut = new();
        powerOut.Initialize();
        intermission = new();
        intermission.Initialize();

        openCams = new OpenCams();
        openCams.Initialize();
        closeCams = new CloseCams();
        closeCams.Initialize();

        canvas = office;
        canvas.Initialize();
        stateManager = new StateManager(inOffice);
        stateManager.Initialize();

        canvas.Add(8, shadowOffice);
        shadowOffice.SetDimensions(1400, 1399);
        shadowOffice.SetPosition(Global.renderTarget.Width / 2 - shadowOffice.GetWidth() / 2,
            Global.renderTarget.Height / 2 - shadowOffice.GetHeight() / 2);

        bgm = Global.content.Load<Song>("music/mall");
        AudioManager.LoopingBGM = true;

        pause = new(this);
        pause.Initialize();
        gameWin = new();
        gameWin.Initialize();

        // Global.difficultyManager.Apply(this);
        // if (Global.userData.oneMoreMikuling) office.mikulingManager.numTillDeath++;
        Global.runData.difficultyManager.Apply(this);
        if (ModifierManager.oneMoreMikuling) office.mikulingManager.numTillDeath++;

        currPower = totalPower;
        powerCounter = 0;
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
        else if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.P))
        {
            intermissionTime = true;
            intermission.OnStart();
        }
        else if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.M))
        {
            if (ModifierManager.letsGoGambling) office.sign.Win();
        }
        else if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.B))
        {
            if (ModifierManager.letsGoGambling) office.sign.Lose();
        }
        else if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.C))
        {
            AddTokens(1);
        }
        // end debug

        if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape) && stateManager.currState.GetType() != typeof(Jumpscare))
        {
            pause.OnStart();
            return pause;
        }

        if (stateManager.currState.GetType() != typeof(Jumpscare))
        {
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
        }
        if (intermissionTime)
        {
            if (stateManager.currState.GetType() != typeof(Intermission))
            {
                if (stateManager.currState.GetType() != typeof(InOffice)) {
                    stateManager.currState = closeCams;
                }
                else
                {
                    stateManager.currState = intermission;
                    intermissionTime = false;
                }
            }
        }
        stateManager.Update();
        office.door_L.Update();
        office.door_R.Update();
        if (ModifierManager.letsGoGambling) Global.night.office.sign.Update();
        if (!jumpytime) {
            UpdateMikus();
            office.mikulingManager.Update();
        }
        if (jumpytime)
        {
            if (stateManager.currState.GetType() == typeof(InOffice)
                || stateManager.currState.GetType() == typeof(PowerOut)
                || stateManager.currState.GetType() == typeof(Intermission))
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
        if (ModifierManager.shadowMiku) shadowMiku.Update();
        if (ModifierManager.shadowOffice) shadowOffice.Update();
    }

    private void ReleaseTheGas()
    {
        int i = r.Next(0, 4);
        while (camView.camButtons[i].activeWarning)
        {
            i++;
            if (i > 3) i = 0;
        }
        camView.camButtons[i].activeWarning = true;
        AudioManager.AddSFX(gas);
        if (Global.night.camNum == i + 1) 
            Global.night.camView.InterruptCam(Global.night.camNum);
    }

    public void UpdateUI()
    {
        UpdateClock();
        UpdatePower();
        // UpdateTokenUI();
    }

    private void UpdatePower()
    {
        int i = 0;
        double j = 0;
        if (doorClose_L || office.door_L.closing || office.door_L.stuckCount > 0)
        {
            i++;
            j += ModifierManager.doorDrainLess ? .75 : 1;
        }
        if (doorClose_R || office.door_R.closing || office.door_R.stuckCount > 0)
        {
            i++;
            j += ModifierManager.doorDrainLess ? .75 : 1;
        }
        if (stateManager.currState.GetType() != typeof(CloseCams) && stateManager.currState.GetType() != typeof(InOffice)
                && stateManager.currState.GetType() != typeof(Intermission))
        {
            i++;
            j++;
        }

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
            currPower -= j;
            if (currPower < 0) currPower = 0;
            powerPercent.Text = (currPower / totalPower * 100).ToString("F0") + "%";
            powerCounter = 0;
        }
    }
    private void UpdateClock()
    {
        clockTime += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (hour == 3 && ModifierManager.intermission && clockTime >= 10 && !intermissionTime 
            && stateManager.currState.GetType() != typeof(Intermission) && clockTime <= 15)
        {
            intermission.OnStart();
            intermissionTime = true;
        }
        if (clockTime >= (ModifierManager.fastNight ? 62 : 68))
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
                loop.visible = false;

                clockTime = 0;
                AudioManager.AddSFX(chime);
            }
            else
            {
                if (camView.camButtons.Any(c => c.activeWarning))
                {
                    Global.night.office.jumpscarePNG.SetTexture(Global.content.Load<Texture2D>("cloud"));
                    Global.night.jumpytime = true;
                }
                else
                {
                    gameWin.OnStart();
                    Global.sceneManager.currScene = gameWin;
                }
            }
        }
        else if (hour < 1 && (jumpClock.visible || jumpLoop.visible || jumpLoopNum.visible) && (!Global.userData.firstTime || Global.customNight))
        {
            if (jumpLoopNum.opacity < 1)
            {
                jumpLoopNum.opacity -= (float) Global.gameTime.ElapsedGameTime.TotalSeconds;
                if (jumpLoopNum.opacity < 0) 
                {
                    jumpLoopNum.visible = false;
                    clock.visible = true;
                    if (!Global.userData.firstTime || Global.customNight) loop.visible = true;
                }
            }
            else if (clockTime >= 2.25)
            {
                jumpLoopNum.opacity = .99f;
            }
            else if (clockTime >= 1.5 && !jumpLoopNum.visible)
            {
                AudioManager.AddSFX(chime);
                jumpLoop.visible = false;
                jumpLoopNum.visible = true;
            }
            else if (clockTime >= .75 && !jumpLoop.visible && !jumpLoopNum.visible)
            {
                AudioManager.AddSFX(chime);
                jumpLoop.visible = true;
                jumpClock.visible = false;
            }
        }
        else if (jumpClock.visible && clockTime >= 1.5)
        {
            jumpClock.visible = false;
            clock.visible = true;
            loop.visible = !Global.userData.firstTime || Global.customNight;

            if (ModifierManager.theGas && hour > 0 && hour < 5) ReleaseTheGas();
        }
    }
    private void UpdateTokenUI()
    {
        coinCounter.Text = ": " + (Global.runData.tokens < 10 ? "0" : "") + Global.runData.tokens;
    }
    private void PopulateUI()
    {
        jumpClock = new("12 AM", "fnaf-big");
        jumpClock.SetPosition(0, -78);
        ui.Add(9, jumpClock);
        jumpLoop = new("LOOP", "fnaf-big");
        jumpLoop.SetPosition(Global.renderTarget.Width / 2 - jumpLoop.GetWidth() / 2 + 25, -78);
        ui.Add(9, jumpLoop);
        jumpLoop.visible = false;
        if (Global.customNight)
        {
            jumpLoopNum = new("INF", "fnaf-big");
        }
        else
        {
            jumpLoopNum = new(Global.runData.loop.ToString(), "fnaf-big");
        }
        jumpLoopNum.SetPosition(Global.renderTarget.Width / 2 - jumpLoopNum.GetWidth() / 2 + 25, -78);
        ui.Add(9, jumpLoopNum);
        jumpLoopNum.visible = false;
        clock = new("12 AM", "fnaf");
        clock.SetPosition(Global.renderTarget.Width - clock.GetWidth() - 24, 12);
        ui.Add(8, clock);
        clock.visible = false;
        if (Global.customNight) loop = new("Loop INF", "fnaf-small");
        else loop = new("Loop " + Global.runData.loop, "fnaf-small");
        loop.SetPosition(Global.renderTarget.Width - loop.GetWidth() - 25, 44);
        ui.Add(8, loop);
        loop.visible = false;

        TextDisplay t = new("Usage:", "fnaf-small");
        t.MapBoundsToTextSize();
        t.SetPosition(27, Global.renderTarget.Height - t.GetHeight() - 20);
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

        coinIcon = new("coin");
        coinIcon.SetPosition(Global.renderTarget.Width - 130,
            Global.renderTarget.Height - coinIcon.GetHeight() - 32);
        ui.Add(8, coinIcon);

        coinCounter = new(": 99", "fnaf");
        coinCounter.SetPosition(coinIcon.GetPosition().X + coinIcon.GetWidth(),
            coinIcon.GetPosition().Y);
        ui.Add(8, coinCounter);
        UpdateTokenUI();
        
        if (!Global.runData.shopAccessible)
        {
            coinIcon.visible = false;
            coinCounter.visible = false;
        }
    }

    public void AddTokens(int i)
    {
        if (!Global.runData.shopAccessible)
        {
            Global.runData.shopAccessible = true;
            coinCounter.visible = true;
            coinIcon.visible = true;
        }
        Global.runData.tokens += i;
        UpdateTokenUI();
        AudioManager.AddSFX(coin);
    }
}
