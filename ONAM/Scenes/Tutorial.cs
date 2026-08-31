using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class Tutorial : Night
{
    private Welcome startState;

    public override void Initialize()
    {
        r = new();

        chime = new(Global.content.Load<SoundEffect>("sfx/clock_chime"));

        clockTime = 0;
        prevClockTime = 0;
        hour = 0;

        camNum = 3;
        sealedVentNum = 100;
        mikus = [new BlueMiku(), new RedMiku(), new YellowMiku(), new GreenMiku()];

        office = new Office();
        office.Initialize();
        camView = new CamView();
        camView.Initialize();
        camView.camBar.visible = false;
        ui = new();
        ui.Initialize();
        PopulateUI();

        // inOffice = new InOffice();
        // inOffice.Initialize();
        // inCams = new InCams();
        // inCams.Initialize();
        sealingVent = new TSV();
        sealingVent.Initialize();

        // openCams = new OpenCams();
        // openCams.Initialize();
        // closeCams = new CloseCams();
        // closeCams.Initialize();

        // Welcome welcome = new();
        // welcome.Initialize();

        canvas = office;
        canvas.Initialize();
        startState = new();
        stateManager = new StateManager(startState);
        stateManager.Initialize();

        // bgm = Global.content.Load<Song>("music/mall");
        // AudioManager.LoopingBGM = true;

        // pause = new(this);
        // pause.Initialize();
        // gameWin = new();
        // gameWin.Initialize();

        Global.runData.difficultyManager.Apply(this);

        currPower = totalPower;
        powerCounter = 0;
        tokens = 0;

        firstCycle = true;
    }

    public override void OnStart()
    {
        clockTime = 0;
        hour = 0;
        currPower = totalPower;

        startState.OnStart();

        // Global.RollFun();
    }

    public override Scene Update()
    {
        if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape) && stateManager.currState.GetType() != typeof(Jumpscare))
        {
            Confirm c = new(
                    this, 
                    "Skip Tutorial?",
                    "(Not recommended for new players.)", 
                    () => 
                    {
                        AudioManager.CloseSFXAll();
                        AudioManager.PauseBGM();
                        LoadNight l = new();
                        l.Initialize();
                        Global.sceneManager.currScene = l;
                    }, 
                    () => {}
                );
                c.Initialize();
                return c;
            // pause.OnStart();
            // return pause;
        }

        if (currPower >= 0) UpdateUI();

        stateManager.Update();

        office.door_L.Update();
        office.door_R.Update();
        
        // UpdateMikus();
        // office.mikulingManager.Update();

        camView.UpdateAnimations();
        return null;
    }

    public override void Draw()
    {
        canvas.Draw();
        if (Global.sceneManager.currScene.GetType() != typeof(Pause) 
            && Global.sceneManager.currScene.GetType() != typeof(GameWin) 
            && Global.sceneManager.currScene.GetType() != typeof(Confirm)
            && stateManager.currState.GetType() != typeof(Jumpscare)
            && stateManager.currState.GetType() != typeof(PowerOut)) ui.Draw();
    }

    public new void UpdateMikus()
    {
        foreach (Miku m in mikus)
        {
            m.Update();
        }
    }

    public new void UpdateUI()
    {
        UpdateClock();
        UpdatePower();
    }

    private void UpdatePower()
    {
        int i = 0;
        double j = 0;
        if (office.door_L.DrainingPower())
        {
            i++;
            j += ModifierManager.doorDrainLess ? .75 : 1;
        }
        if (office.door_R.DrainingPower())
        {
            i++;
            j += ModifierManager.doorDrainLess ? .75 : 1;
        }
        // if (stateManager.currState.GetType() != typeof(CloseCams) && stateManager.currState.GetType() != typeof(InOffice)
        //         && stateManager.currState.GetType() != typeof(Intermission))
        // {
        //     i++;
        //     j++;
        // }

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
            // currPower -= j;
            if (currPower < 0) currPower = 0;
            powerPercent.Text = (currPower / totalPower * 100).ToString("F0") + "%";
            powerCounter = 0;
        }
    }
    private void UpdateClock()
    {
        clockTime += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (firstCycle)
        {
            // AudioManager.PlayBGM(bgm);
            AudioManager.AddSFX(chime);
            clockTime = 0;
            firstCycle = false;
        }

        if (jumpClock.visible && clockTime >= 1.5)
        {
            jumpClock.visible = false;
            clock.visible = true;
        }
    }

    private void PopulateUI()
    {
        modView = new();
        modView.Initialize();
        modView.visible = false;
        ui.Add(9, modView);

        jumpClock = new("11 PM", "fnaf-big");
        jumpClock.MapBoundsToTextSize();
        jumpClock.SetPosition(Global.renderTarget.Width / 2 - jumpClock.GetWidth() / 2 + 24,
            -78);
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
        clock = new("11 PM", "fnaf");
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
    }
}
