using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace ONAM;

public class ViewCams : TutorialState
{
    private SFXObject call2;

    public override void Initialize()
    {
        base.Initialize();

        nextState = new ExitCams();
        nextState.Initialize();

        call = new(Global.content.Load<SoundEffect>("sfx/calls/tutorial/3"));
        call2 = new(Global.content.Load<SoundEffect>("sfx/calls/tutorial/4"));
    }

    private State UpdateCams()
    {
        if (KeyboardManager.KeyPressed(Keys.Enter)
            && Global.night.camNum > 4 && Global.night.camNum != Global.night.sealedVentNum + 5)
        {
            ((TSV) Global.night.sealingVent).OnStart(this);
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
            if (Global.night.camView.seal_vent_bar.GetBounds().Contains(MouseManager.Location)
                && Global.night.camNum > 4 && Global.night.camNum != Global.night.sealedVentNum + 5)
            {
                ((TSV) Global.night.sealingVent).OnStart(this);
                return Global.night.sealingVent;
            }
        }

        return null;
    }

    private bool MouseOverCamBar()
    {
        return Global.night.camView.camBar.GetBounds().Contains(MouseManager.Location);
    }
    private State UpdateCamBar()
    {
        // cam bar
        if (Global.night.camView.camBar.visible 
            && (KeyboardManager.KeyPressed(Keys.S) || MouseOverCamBar()))
        {
            AudioManager.CloseSFXAll();
            Global.night.camView.camBar.visible = false;
            nextState.OnStart();
            return nextState;
        }

        return null;
    }

    public override void OnStart()
    {
        base.OnStart();
        AudioManager.AddSFX(call);
    }

    public override State Update()
    {
        base.Update();
        
        State s = UpdateCams();
        if (s != null) return s;

        if (call.PlaybackClosed)
        {
            Global.night.camView.camBar.visible = true;
            if (call2.PlaybackClosed) AudioManager.AddSFX(call2);
        }

        return UpdateCamBar();
    }
}
