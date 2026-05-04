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
        if ((KeyboardManager.KeyPressed(Keys.Enter)  || Global.userData.autoSeal)
            && Global.night.camNum > 4 && Global.night.camNum != Global.night.sealedVentNum + 5)
        {
            return Global.night.sealingVent;
        }

        if (KeyboardManager.KeyPressed(Keys.NumPad1) 
            || KeyboardManager.KeyPressed(Keys.D1))
        {
            Global.night.camView.SetToCam(1);
        }
        else if (KeyboardManager.KeyPressed(Keys.NumPad2) 
            || KeyboardManager.KeyPressed(Keys.D2))
        {
            Global.night.camView.SetToCam(2);
        }
        else if (KeyboardManager.KeyPressed(Keys.NumPad3) 
            || KeyboardManager.KeyPressed(Keys.D3))
        {
            Global.night.camView.SetToCam(3);
        }
        else if (KeyboardManager.KeyPressed(Keys.NumPad4) 
            || KeyboardManager.KeyPressed(Keys.D4))
        {
            Global.night.camView.SetToCam(4);
        }
        else if (KeyboardManager.KeyPressed(Keys.NumPad5) 
            || KeyboardManager.KeyPressed(Keys.D5))
        {
            Global.night.camView.SetToCam(5);
        }
        else if (KeyboardManager.KeyPressed(Keys.NumPad6) 
            || KeyboardManager.KeyPressed(Keys.D6))
        {
            Global.night.camView.SetToCam(6);
        }
        else if (KeyboardManager.KeyPressed(Keys.NumPad7) 
            || KeyboardManager.KeyPressed(Keys.D7))
        {
            Global.night.camView.SetToCam(7);
        }
        else if (KeyboardManager.KeyPressed(Keys.NumPad8) 
            || KeyboardManager.KeyPressed(Keys.D8))
        {
            Global.night.camView.SetToCam(8);
        }

        return null;
    }

    private bool MouseOverCamBar()
    {
        return Global.night.camView.camBar.GetBounds().Contains(MouseManager.Location);
    }
    private State CheckMouseActions()
    {
        // cam buttons
        if (MouseManager.LeftButtonClicked)
        {
            foreach (CamButton c in Global.night.camView.camButtons)
            {
                if (c.GetBounds().Contains(MouseManager.Location))
                {
                    Global.night.camView.SetToCam(c.id);
                    break;
                }
            }
            if ((Global.night.camView.seal_vent_bar.GetBounds().Contains(MouseManager.Location) || Global.userData.autoSeal)
                && Global.night.camNum > 4 && Global.night.camNum != Global.night.sealedVentNum + 5)
            {
                return Global.night.sealingVent;
            }
        }

        // cam bar
        if (KeyboardManager.KeyPressed(Keys.S) 
            || (Global.night.camView.camBar.visible && MouseOverCamBar()))
        {
            Global.night.camView.camBar.visible = false;
            return Global.night.closeCams;
        }
        else if (!Global.night.camView.camBar.visible && !MouseOverCamBar())
        {
            Global.night.camView.camBar.visible = true;
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
