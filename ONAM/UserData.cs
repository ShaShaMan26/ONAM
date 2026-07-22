namespace ONAM;

public class UserData
{
    public bool firstTime { get; set; }
    public int completion { get; set; }
    public int completionF { get; set; }
    public RunData mainRun { get; set; }
    public RunData customRun { get; set; }
    public RunData bestRun { get; set; }
    public RunData recentRun { get; set; }

    public void SetToDefaults()
    {
        firstTime = true;
        completion = 0;
        completionF = 0;
        mainRun = new();
        customRun = new();
        mainRun.SetToDefaults();
        customRun.SetToDefaults();
        bestRun = null;
        recentRun = null;
    }
}
