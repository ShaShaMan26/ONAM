using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace ONAM;

public class Welcome : TutorialState
{
    private SFXObject call2;

    public override void Initialize()
    {
        base.Initialize();

        // nextState = new Doors();
        nextState = new EnterCams();
        nextState.Initialize();

        Global.night.office.bg.SetPosition(Global.renderTarget.Width / 2 - Global.night.office.bg.GetWidth() / 2, 0);

        Global.night.office.door_L.SetPosition(Global.night.office.bg.GetPosition() + new Vector2(70, 0));
        Global.night.office.door_R.SetPosition(Global.night.office.bg.GetPosition() + new Vector2(1272, 0));
        Global.night.office.door_button_holder_l.SetPosition(Global.night.office.bg.GetPosition() + new Vector2(-8, 270));
        Global.night.office.door_button_l.SetPosition(Global.night.office.door_button_holder_l.GetPosition() + new Vector2(29, 52));
        Global.night.office.door_button_holder_r.SetPosition(Global.night.office.bg.GetPosition() + new Vector2(Global.night.office.bg.GetWidth() - 116, 270));
        Global.night.office.door_button_r.SetPosition(Global.night.office.door_button_holder_r.GetPosition() + new Vector2(23, 52));
        Global.night.office.mikulingManager.SetPosition(Global.night.office.bg.GetPosition());
    
        Global.night.office.camBar.visible = false;

        call = new(Global.content.Load<SoundEffect>("sfx/calls/tutorial/test1-3"));
        call2 = new(Global.content.Load<SoundEffect>("sfx/calls/tutorial/test2-3"));
    }

    protected bool MouseOverCamBar()
    {
        return Global.night.office.camBar.GetBounds().Contains(MouseManager.Location);
    }

    protected State CheckCamFlip()
    {
        if (Global.night.office.camBar.visible 
            && (KeyboardManager.KeyPressed(Keys.S) || MouseOverCamBar()))
        {
            AudioManager.CloseSFXAll();
            Global.night.office.camBar.visible = false;
            // nextState.OnStart();
            return nextState;
        }
        // else if (!Global.night.office.camBar.visible && !MouseOverCamBar())
        // {
        //     Global.night.office.camBar.visible = true;
        // }
        return null;
    }

    public override void OnStart()
    {
        base.OnStart();
        // AudioManager.PlaySFX(call);
        AudioManager.AddSFX(call);
    }

    public override State Update()
    {
        base.Update();
        if (call.PlaybackClosed)
        {
            Global.night.office.camBar.visible = true;
            if (call2.PlaybackClosed) AudioManager.AddSFX(call2);
        }

        return CheckCamFlip();
        // return base.Update();
    }
}
