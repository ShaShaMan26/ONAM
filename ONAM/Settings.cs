using System;

namespace ONAM;

public class Settings
{
    public short displayMode { get; set; } = 0;
    public short refreshRate { get; set; } = 2;
    public float musicVolume { get; set; } = 1;
    public float sfxVolume { get; set; } = 1;

    public double fullscreenScale;

    public void ApplyAll()
    {
        SetDisplayMode(displayMode);
        SetRefreshRate(refreshRate);
    }

    public void SetDisplayMode(short i)
    {
        Global.graphics.IsFullScreen = false;
        Global.graphics.ApplyChanges();
        Global.game.Window.IsBorderless = false;
        if (i == 0)
        {
            // windowed
            Global.graphics.PreferredBackBufferWidth = Global.renderTarget.Width;
            Global.graphics.PreferredBackBufferHeight = Global.renderTarget.Height;
            Global.game.Window.Position = new(Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width / 2 - Global.graphics.PreferredBackBufferWidth / 2,
                Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height / 2 - Global.graphics.PreferredBackBufferHeight / 2);
        }
        else if (i == 1)
        {
            // borderless full
            Global.graphics.PreferredBackBufferWidth = Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            Global.graphics.PreferredBackBufferHeight = Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            Global.game.Window.IsBorderless = true;
            Global.game.Window.Position = new(Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width / 2 - Global.graphics.PreferredBackBufferWidth / 2,
                Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height / 2 - Global.graphics.PreferredBackBufferHeight / 2);
            fullscreenScale = (double) Global.graphics.PreferredBackBufferHeight / Global.renderTarget.Height;
        }
        else if (i == 2)
        {
            // full
            SetDisplayMode(1);
            Global.graphics.IsFullScreen = true;
        }
        MouseManager.LockedToWindow = Global.graphics.IsFullScreen;
        Global.graphics.ApplyChanges();
    }

    public void SetRefreshRate(short i)
    {
        Global.graphics.SynchronizeWithVerticalRetrace = true;
        Global.game.IsFixedTimeStep = true;

        double targetFPS = 0;
        if (i == 0)
        {
            Global.graphics.SynchronizeWithVerticalRetrace = false;
            Global.game.IsFixedTimeStep = false;
        }
        else if (i == 1)
        {
            targetFPS = 29.9;
        }
        else if (i == 2)
        {
            targetFPS = 59.9;
        }
        else if (i == 3)
        {
            targetFPS = 89.9;
        }
        else if (i == 4)
        {
            targetFPS = 119.9;
        }
        else if (i == 5)
        {
            targetFPS = 144.001;
        }
        else if (i == 6)
        {
            targetFPS = 240;
        }

        if (targetFPS > 0)
        {
            Global.game.TargetElapsedTime = TimeSpan.FromTicks((long) (TimeSpan.TicksPerSecond / targetFPS));
        }
        Global.graphics.ApplyChanges();
    }
}
