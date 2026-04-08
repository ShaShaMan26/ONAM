namespace ONAM;

public class DifficultySelect: Scene
{
    public override void Initialize()
    {

    }

    public override Scene Update()
    {
        // Global.SaveDifficulty();
        Global.LoadDifficulty("easy");

        Global.loadNight = new();
        Global.loadNight.Initialize();
        return Global.loadNight;
    }
}
