namespace ONAM;

public class Scene
{
    public virtual void Initialize() {}

    public virtual void OnStart() {}

    public virtual Scene Update()
    {
        return null;
    }

    public virtual void Draw() {}
}
