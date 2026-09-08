namespace ONAM;

public class TutorialState : State
{
    protected TutorialState nextState;
    public SFXObject call;

    public override void Initialize()
    {
        nextState = null;
        call = null;
        base.Initialize();
    }

    public virtual void OnStart() {}

    public override State Update()
    {
        if (Global.devEnabled
                && KeyboardManager.KeyReleased(Microsoft.Xna.Framework.Input.Keys.N))
        {
            if (call != null)
            {
                call.Stop();
                AudioManager.RemoveSFX(call);
            }
        }
        return base.Update();
    }
}
