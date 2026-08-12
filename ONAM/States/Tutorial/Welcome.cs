using Microsoft.Xna.Framework;

namespace ONAM;

public class Welcome : TutorialState
{
    public override void Initialize()
    {
        base.Initialize();

        nextState = new Doors();
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
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override State Update()
    {
        // add check for call to be over

        return base.Update();
    }
}
