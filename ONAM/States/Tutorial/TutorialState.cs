namespace ONAM;

public class TutorialState : State
{
    protected TutorialState nextState;

    public override void Initialize()
    {
        nextState = null;
        base.Initialize();
    }

    public virtual void OnStart()
    {
        
    }

    public override State Update()
    {
        if (Global.devEnabled
                && KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.N)
                && nextState != null)
        {
            nextState.OnStart();
            return nextState;
        }
        return base.Update();
    }
}
