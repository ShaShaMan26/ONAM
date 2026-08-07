using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace ONAM;

public class InCams : State
{
    private SFXObject error;

    public override void Initialize()
    {
        error = new(Global.content.Load<SoundEffect>("sfx/error"));
        base.Initialize();
    }

    private State UpdateCams()
    {
        if ((KeyboardManager.KeyPressed(Keys.Enter)  || ModifierManager.autoSeal)
            && Global.night.camNum > 4 && Global.night.camNum != Global.night.sealedVentNum + 5)
        {
            if (ModifierManager.noLateSeal && ((GreenMiku) Global.night.mikus[3]).targetProg == Global.night.camNum && Global.night.mikus[3].attacking)
            {
                AudioManager.AddSFX(error);
            }
            else return Global.night.sealingVent;
        }
        if (KeyboardManager.KeyPressed(Keys.Enter) && Global.night.camNum < 5 
            && Global.night.camView.camButtons[Global.night.camNum - 1].activeWarning)
        {
            return Global.night.clearingGas;
        }
        if (KeyboardManager.KeyPressed(Keys.Q) && Global.runData.shopAccessible)
        {
            Global.night.camView.OpenShop();
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
            if ((Global.night.camView.seal_vent_bar.GetBounds().Contains(MouseManager.Location) || ModifierManager.autoSeal)
                && Global.night.camNum > 4 && Global.night.camNum != Global.night.sealedVentNum + 5)
            {
                if (ModifierManager.noLateSeal && ((GreenMiku) Global.night.mikus[3]).targetProg == Global.night.camNum && Global.night.mikus[3].attacking)
                {
                    AudioManager.AddSFX(error);
                }
                else return Global.night.sealingVent;
            }
            if (Global.night.camView.gas_bar.GetBounds().Contains(MouseManager.Location)
                && Global.night.camNum < 5 && Global.night.camView.camButtons[Global.night.camNum - 1].activeWarning)
            {
                return Global.night.clearingGas;
            }
        }

        Global.night.camView.goldie.visible = Global.night.camNum == 1
            && Global.IsFun(66) 
            && !Global.night.mikus.Any(m => m.progress == 1);

        return null;
    }
    private void UpdateShop()
    {
        if (KeyboardManager.KeyPressed(Keys.Q) && Global.runData.shopAccessible)
        {
            Global.night.camView.CloseShop();
        }
        else Global.night.camView.shopScreen.Update();
    }

    private bool MouseOverCamBar()
    {
        return Global.night.camView.camBar.GetBounds().Contains(MouseManager.Location);
    }
    private State UpdateCamBar()
    {
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
        if (Global.night.camView.shopScreen.visible) UpdateShop();
        else
        {
            State s = UpdateCams();
            if (s != null) return s;
        }

        if (Global.night.camView.goldie.visible
            && MouseManager.LeftButtonClicked
            && Global.night.camView.goldie.GetBounds().Contains(MouseManager.Location))
        {
            Global.sceneManager.currScene = new Interesting();
            Global.sceneManager.currScene.Initialize();
        }

        return UpdateCamBar();
    }
}
