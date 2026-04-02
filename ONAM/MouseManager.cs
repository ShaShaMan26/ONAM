using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ONAM;

public static class MouseManager
{
    private static MouseState CurMouseState;
    private static MouseState PrevMouseState;
    
    public static bool LockedToWindow;

    public static void Update()
    {
        PrevMouseState = CurMouseState;
        CurMouseState = Mouse.GetState();

        if (LockedToWindow && !WithinWindow)
        {
            int x = Math.Clamp((int) RealLocation.X, 0, Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width);
            int y = Math.Clamp((int) RealLocation.Y, 0, Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
            Mouse.SetPosition(x, y);
        }
    }

    public static void SetCursor(MouseCursor mouseCursor)
    {
        Mouse.SetCursor(mouseCursor);
    }

    // left button
    public static bool LeftButtonClicked
    {
        get
        {
            return PrevMouseState.LeftButton.Equals(ButtonState.Released) && LeftButtonPressed;
        }
    }
    public static bool LeftButtonPressed
    {
        get
        {
            return CurMouseState.LeftButton.Equals(ButtonState.Pressed);
        }
    }
    public static bool LeftButtonReleased
    {
        get
        {
            return PrevMouseState.LeftButton.Equals(ButtonState.Pressed) && !LeftButtonPressed;
        }
    }

    // right button
    public static bool RightButtonClicked
    {
        get
        {
            return PrevMouseState.RightButton.Equals(ButtonState.Released) && RightButtonPressed;
        }
    }
    public static bool RightButtonPressed
    {
        get
        {
            return CurMouseState.RightButton.Equals(ButtonState.Pressed);
        }
    }
    public static bool RightButtonReleased
    {
        get
        {
            return PrevMouseState.RightButton.Equals(ButtonState.Pressed) && !RightButtonPressed;
        }
    }

    public static Vector2 Location
    {
        get
        {
            return CurMouseState.Position.ToVector2() / 
                (new Vector2(Global.graphics.PreferredBackBufferWidth,
                    Global.graphics.PreferredBackBufferHeight)
                    / Global.renderTarget.Bounds.Size.ToVector2());
        }
    }
    private static Vector2 RealLocation
    {
        get
        {
            return CurMouseState.Position.ToVector2();
        }
    }
    // public static Vector2 PrevLocation
    // {
    //     get
    //     {
    //         return PrevMouseState.Position.ToVector2();
    //     }
    // }

    public static bool WithinWindow
    {
        get
        {
            return Global.graphics.GraphicsDevice.PresentationParameters.Bounds.Contains(Location);
        }
    }
    // public static bool HasMoved
    // {
    //     get
    //     {
    //         return PrevLocation != Location;
    //     }
    // }
    public static Vector2 DistanceTraveled
    {
        get
        {
            return (CurMouseState.Position - PrevMouseState.Position).ToVector2();
        }
    }

    // scrollWheel value
    public static float ScrollWheelValue
    {
        get
        {
            return CurMouseState.ScrollWheelValue;
        }
    }
    public static float PrevScrollWheelValue
    {
        get
        {
            return PrevMouseState.ScrollWheelValue;
        }
    }public static float ScrollWheelValueDelta
    {
        get
        {
            return PrevMouseState.ScrollWheelValue - CurMouseState.ScrollWheelValue;
        }
    }
}