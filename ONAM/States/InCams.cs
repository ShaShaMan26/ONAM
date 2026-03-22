using Microsoft.Xna.Framework.Input;

namespace ONAM;

public class InCams : State
{
    public override void Initialize()
    {
        base.Initialize();
    }

    private void SwitchToCam(int i)
    {
        switch (i)
        {
            case 1:
                Global.camView.bg.SetTexture(Global.camView.cam1);
                break;
            case 2:
                Global.camView.bg.SetTexture(Global.camView.cam2);
                break;
            case 3:
                Global.camView.bg.SetTexture(Global.camView.cam3);
                break;
            case 4:
                Global.camView.bg.SetTexture(Global.camView.cam4);
                break;
        }
    }
    private State CheckKeyActions()
    {
        if (KeyboardManager.KeyPressed(Keys.NumPad1) 
            || KeyboardManager.KeyPressed(Keys.D1))
        {
            SwitchToCam(1);
        }
        else if (KeyboardManager.KeyPressed(Keys.NumPad2) 
            || KeyboardManager.KeyPressed(Keys.D2))
        {
            SwitchToCam(2);
        }
        else if (KeyboardManager.KeyPressed(Keys.NumPad3) 
            || KeyboardManager.KeyPressed(Keys.D3))
        {
            SwitchToCam(3);
        }
        else if (KeyboardManager.KeyPressed(Keys.NumPad4) 
            || KeyboardManager.KeyPressed(Keys.D4))
        {
            SwitchToCam(4);
        }
        return null;
    }

    private bool MouseOverCamBar()
    {
        return Global.camView.camBar.GetBounds().Contains(MouseManager.Location);
    }
    private State CheckMouseActions()
    {
        if (KeyboardManager.KeyPressed(Keys.S) 
            || (Global.camView.camBar.visible && MouseOverCamBar()))
        {
            Global.camView.camBar.visible = false;
            return Global.closeCams;
        }
        else if (!Global.camView.camBar.visible && !MouseOverCamBar())
        {
            Global.camView.camBar.visible = true;
        }

        return null;
    }

    public override State Update()
    {
        State s = CheckKeyActions();
        if (s != null) return s;
        return CheckMouseActions();
    }
}
