namespace ONAM;

public class UserData
{
    public bool firstTime { get; set; }
    public int completion { get; set; }
    public RunData mainRun { get; set; }
    public RunData customRun { get; set; }

    // public string difficulty { get; set; }
    // public int loop { get; set; }
    // public bool[] activeModifiers { get; set; }
    // public bool[] customModifiers { get; set; }

    public void SetToDefaults()
    {
        firstTime = true;
        completion = 0;
        mainRun = new();
        customRun = new();
        mainRun.SetToDefaults();
        customRun.SetToDefaults();

        // difficulty = "easy";
        // loop = 0;
        // activeModifiers = new bool[Global.modifiers.Length];
        // customModifiers = new bool[Global.modifiers.Length];
        // for (int i = 0; i < activeModifiers.Length; i ++)
        // {
        //     activeModifiers[i] = false;
        //     customModifiers[i] = false;
        //     File.Delete(Global.content.RootDirectory + "/difficulties/custom.txt");
        // }
    }

    // public void SetToNewLoop()
    // {
    //     loop = 1;
    //     activeModifiers = new bool[Global.modifiers.Length];
    //     for (int i = 0; i < activeModifiers.Length; i ++)
    //     {
    //         activeModifiers[i] = false;
    //     }
    // }
}
