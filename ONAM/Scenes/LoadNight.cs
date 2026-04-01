using System;

namespace ONAM;

public class LoadNight : Scene
{
    private DateTime startTime;
    private GameElement clock;
    private bool started;

    public override void Initialize()
    {
        clock = new("clock");
        clock.SetDimensions(50, 50);
        clock.SetPosition(Global.graphics.PreferredBackBufferWidth - clock.GetWidth() - 50,
            Global.graphics.PreferredBackBufferHeight - clock.GetHeight() - 40);

        started = false;
    }

    public override Scene Update()
    {
        if (!started)
        {
            startTime = DateTime.Now;
            Global.night = new();
            Global.night.Initialize();
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
