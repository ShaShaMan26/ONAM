namespace ONAM;

public class SceneManager
{
    public Scene currScene;

    public SceneManager(Scene currScene)
    {
        this.currScene = currScene;
    }

    public void Initialize()
    {
        currScene?.Initialize();
    }

    public void Update()
    {
        Scene s = currScene?.Update();
        if (s == null) return;
        // s.Initialize();
        currScene = s;
    }

    public void Draw()
    {
        currScene?.Draw();
    }
}
