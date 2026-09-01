using System;

namespace ONAM;

public class Credits : Scene
{
    private Canvas canvas;

    private CamStatic camStatic;
    private NavButton back;
    private GameElement miku;

    private double counter;

    public override void Initialize()
    {
        counter = 0;

        canvas = new();
        canvas.Initialize();

        camStatic = new(.3f, .4f);
        camStatic.Initialize();
        canvas.Add(5, camStatic);

        back = new(
            "Back", 
            () =>
            {
                Global.SaveUserData();

                TransFlicker t = new(Global.mainMenu);
                t.Initialize();
                Global.sceneManager.currScene = t;
            }
        );
        back.SetPosition(
            Global.renderTarget.Width / 2 - back.GetWidth() / 2, 
            Global.renderTarget.Height - back.GetHeight() * 1.5f
        );
        canvas.Add(6, back);

        miku = new("miku");
        miku.opacity = 0;
        miku.visible = false;
        miku.SetDimensions(1100, 1099);
        miku.SetPosition(Global.renderTarget.Width / 2 - miku.GetWidth() / 2, 
            -200);
        canvas.Add(1, miku);

        // title
        TextDisplay title = new("One* Night at Miku's", "Consolas");
        title.SetPosition(Global.renderTarget.Width / 2 - title.GetWidth() / 2,
            40);
        canvas.Add(6, title);

        // creator
        TextDisplay creatorTitle = new("Created By:", "fnaf");
        creatorTitle.SetPosition(Global.renderTarget.Width / 2 - creatorTitle.GetWidth() / 2,
            title.GetBounds().Bottom + 12);
        canvas.Add(6, creatorTitle);
        TextDisplay creator = new("SusieNotDeltarune", "fnaf-slight-big");
        creator.SetPosition(Global.renderTarget.Width / 2 - creator.GetWidth() / 2,
            creatorTitle.GetBounds().Bottom - 5);
        canvas.Add(6, creator);

        // playtesters
        TextDisplay playtestersTitle = new("Playtesters:", "fnaf");
        playtestersTitle.SetPosition(Global.renderTarget.Width / 2 - playtestersTitle.GetWidth() / 2,
            creator.GetBounds().Bottom + 20);
        canvas.Add(6, playtestersTitle);
        TextDisplay playtesters = new("Spark, Noah, Ryan, BirraBoss", "fnaf-slight-big");
        playtesters.SetPosition(Global.renderTarget.Width / 2 - playtesters.GetWidth() / 2,
            playtestersTitle.GetBounds().Bottom - 5);
        canvas.Add(6, playtesters);
        
        // Assets
        TextDisplay assetsTitle = new("Assets Taken From:", "fnaf");
        assetsTitle.SetPosition(Global.renderTarget.Width / 2 - assetsTitle.GetWidth() / 2,
            playtesters.GetBounds().Bottom + 20);
        canvas.Add(6, assetsTitle);
        TextDisplay assets = new("FNaF 1-UCN, Undertale/Deltarune, Petscop,", "fnaf-slight-big");
        assets.SetPosition(Global.renderTarget.Width / 2 - assets.GetWidth() / 2,
            assetsTitle.GetBounds().Bottom - 5);
        canvas.Add(6, assets);
        TextDisplay assets2 = new("Ultrakill, Timesplitters, Random Images on Google", "fnaf-slight-big");
        assets2.SetPosition(Global.renderTarget.Width / 2 - assets2.GetWidth() / 2,
            assets.GetBounds().Bottom - 12);
        canvas.Add(6, assets2);

        // thank you
        TextDisplay thank = new("Thank You For Playing!", "Consolas");
        thank.SetPosition(Global.renderTarget.Width / 2 - thank.GetWidth() / 2,
            assets2.GetBounds().Bottom + 42);
        canvas.Add(6, thank);
    }

    public override void OnStart()
    {
        
    }

    public override Scene Update()
    {
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (miku.visible && miku.opacity < .5)
        {
            miku.opacity = Math.Clamp((float) (miku.opacity + counter / 8), 0, .5f);
            counter = 0;
        }
        if (!miku.visible && counter >= 90)
        {
            miku.visible = true;
            counter = 0;
        }

        camStatic.Update();
        back.Update();
        return null;
    }

    public override void Draw()
    {
        canvas.Draw();
    }
}
