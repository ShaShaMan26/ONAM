using System;

namespace ONAM;

public class LoadTutorial : Scene
{
    private DateTime startTime;
    private GameElement clock;
    private bool started;

    public override void Initialize()
    {
        clock = new("clock");
        clock.SetDimensions(50, 50);
        clock.SetPosition(Global.renderTarget.Width - clock.GetWidth() - 50,
            Global.renderTarget.Height - clock.GetHeight() - 40);

        started = false;
    }

    public override Scene Update()
    {
        if (!started)
        {
            startTime = DateTime.Now;
            Tutorial tutorial = new();
            Global.night = tutorial;
            tutorial.Initialize();
            started = true;
        }
        else if ((DateTime.Now - startTime).TotalSeconds >= .5)
        {
            Global.night.OnStart();
            return Global.night;
        }
        return null;
    }

    public override void Draw()
    {
        clock.Draw();
    }
}
