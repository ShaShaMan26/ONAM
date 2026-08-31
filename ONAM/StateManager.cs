namespace ONAM;

public class StateManager
{
    public State currState;
    
    public StateManager(State state)
    {
        currState = state;
    }
    public StateManager() : this(null) { }

    public void Initialize()
    {
        currState?.Initialize();
    }

    public void Update()
    {
        State s = currState?.Update();
        s?.Initialize();
        if (s != null && s is TutorialState t)
        {
            t.OnStart();
        }
        if (s != null) currState = s;
    }
}
