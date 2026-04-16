namespace ONAM;

public class UserData
{
    public int completion { get; set; }
    public string difficulty { get; set; }
    public int loop { get; set; }
    public bool[] activeModifiers { get; set; }

    public void SetToDefaults()
    {
        completion = 0;
        difficulty = "easy";
        loop = 0;
        activeModifiers = new bool[Global.modifiers.Length];
        for (int i = 0; i < activeModifiers.Length; i ++)
        {
            activeModifiers[i] = false;
        }
    }

    public void SetToNewLoop()
    {
        loop = 0;
        activeModifiers = new bool[Global.modifiers.Length];
        for (int i = 0; i < activeModifiers.Length; i ++)
        {
            activeModifiers[i] = false;
        }
    }
}
