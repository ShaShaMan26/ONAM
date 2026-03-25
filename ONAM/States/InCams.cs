using Microsoft.Xna.Framework.Input;

namespace ONAM;

public class InCams : State
{
    public override void Initialize()
    {
        base.Initialize();
    }

    private State CheckKeyActions()
    {
        if (KeyboardManager.KeyPressed(Keys.NumPad1) 
            || KeyboardManager.KeyPressed(Keys.D1))
        {
            Global.camView.SetToCam(1);
        }
        else if (KeyboardManager.KeyPressed(Keys.NumPad2) 
            || KeyboardManager.KeyPressed(Keys.D2))
        {
            Global.camView.SetToCam(2);
        }
        else if (KeyboardManager.KeyPressed(Keys.NumPad3) 
            || KeyboardManager.KeyPressed(Keys.D3))
        {
            Global.camView.SetToCam(3);
        }
        else if (KeyboardManager.KeyPressed(Keys.NumPad4) 
            || KeyboardManager.KeyPressed(Keys.D4))
        {
            Global.camView.SetToCam(4);
        }

        return null;
    }

    private bool MouseOverCamBar()
    {
        return Global.camView.camBar.GetBounds().Contains(MouseManager.Location);
    }
    private State CheckMouseActions()
    {
        // cam buttons
        if (MouseManager.LeftButtonClicked)
        {
            foreach (CamButton c in Global.camView.camButtons)
            {
                if (c.GetBounds().Contains(MouseManager.Location))
                {
                    Global.camView.SetToCam(c.id);
                    break;
                }
            }
        }

        // cam bar
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
