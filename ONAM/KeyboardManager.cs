using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace ONAM;

public static class KeyboardManager
{
    private static KeyboardState CurKeyboardState;
    private static KeyboardState PrevKeyboardState;

    public static void Update()
    {
        PrevKeyboardState = CurKeyboardState;
        CurKeyboardState = Keyboard.GetState();
    }

    public static bool KeyPressed(Keys key)
    {
        return CurKeyboardState.IsKeyDown(key) && PrevKeyboardState.IsKeyUp(key);
    }
    public static bool KeyDown(Keys key)
    {
        return CurKeyboardState.IsKeyDown(key);
    }
    public static bool KeyReleased(Keys key)
    {
        return CurKeyboardState.IsKeyUp(key) && PrevKeyboardState.IsKeyDown(key);
    }

    public static List<Keys> PressedKeys
    {
        get
        {
            return [.. CurKeyboardState.GetPressedKeys().Except(PrevKeyboardState.GetPressedKeys())];
        }
    }
    public static List<Keys> HeldKeys
    {
        get
        {
            return [.. CurKeyboardState.GetPressedKeys()];
        }
    }
    public static List<Keys> ReleasedKeys
    {
        get
        {
            return [.. PrevKeyboardState.GetPressedKeys().Except(CurKeyboardState.GetPressedKeys())];
        }
    }
}
