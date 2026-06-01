namespace ONAM;

public class RunHistory : Scene
{
    private Canvas canvas;
    private CamStatic camStatic;
    private TextDisplay bgTxt;
    private NavButton back, reset;
    private RunBox best, recent;

    public override void Initialize()
    {
        canvas = new();
        canvas.Initialize();

        camStatic = new(.3f, .4f);
        camStatic.Initialize();
        canvas.Add(5, camStatic);

        bgTxt = new("RUNS", "fnaf-big");
        bgTxt.SetPosition(
            Global.renderTarget.Width / 2 - bgTxt.GetWidth() / 2 + 25,
            Global.renderTarget.Height / 2 - bgTxt.GetHeight() / 2 - 50
        );
        bgTxt.opacity = .5f;
        canvas.Add(4, bgTxt);

        back = new(
            "Back",
            () =>
            {
                TransFlicker t = new(Global.mainMenu);
                t.Initialize();
                Global.sceneManager.currScene = t;
            }
        );
        back.SetPosition(
            Global.renderTarget.Width / 2 - back.GetWidth() / 2 + 5, 
            Global.renderTarget.Height - back.GetHeight() * 2
        );
        canvas.Add(6, back);

        reset = new(
            "Reset Data",
            () =>
            {
                Confirm c = new(
                    this, 
                    "Reset All Data?",
                    "(All progress will be erased.)",
                    () => 
                    {
                        Confirm b = new(
                            this,
                            "Are You Really Sure??",
                            "(Everything will be lost.)",
                            () =>
                            {
                                Global.ResetUserData();
                                TransFlicker t = new(Global.intro);
                                Global.intro.Initialize();
                                t.Initialize();
                                AudioManager.PauseBGM();
                                AudioManager.CloseSFXAll();
                                Global.sceneManager.currScene = t;
                            },
                            camStatic.Update
                        );
                        b.Initialize();
                        Global.sceneManager.currScene = b;
                    }, 
                    camStatic.Update
                );
                c.Initialize();
                Global.sceneManager.currScene = c;
            }
        );
        reset.SetPosition(
            Global.renderTarget.Width / 2 - reset.GetWidth() / 2 + 5, 
            25
        );
        canvas.Add(6, reset);
        reset.opacity = .25f;

        recent = new("Previous Run", Global.userData.recentRun);
        recent.Initialize();
        recent.SetPosition(
            80,
            100
        );
        canvas.Add(7, recent);
        best = new("Best Run", Global.userData.bestRun);
        best.Initialize();
        best.SetPosition(
            688,
            100
        );
        canvas.Add(7, best);
        recent.opacity = .75f;
        best.opacity = .75f;
    }

    public override void OnStart()
    {
        
    }

    public override Scene Update()
    {
        recent.opacity = .75f;
        best.opacity = .75f;
        if (recent.GetBounds().Contains(MouseManager.Location)) recent.opacity = 1;
        else if (best.GetBounds().Contains(MouseManager.Location)) best.opacity = 1;

        camStatic.Update();
        back.Update();
        if (reset.GetBounds().Contains(MouseManager.Location)) reset.opacity = 1;
        else reset.opacity = .15f;
        reset.Update();
        return null;
    }

    public override void Draw()
    {
        canvas.Draw();
    }
}
