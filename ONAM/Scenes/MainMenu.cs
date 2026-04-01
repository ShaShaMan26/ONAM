namespace ONAM;

public class MainMenu : Scene
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override Scene Update()
    {
        if (!KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.E)) return null;
        Global.night = new();
        Global.night.Initialize();
        return Global.night;
    }
}
