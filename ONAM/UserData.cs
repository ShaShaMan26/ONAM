using System.Text.Json.Serialization;

namespace ONAM;

public class UserData
{
    public bool firstTime { get; set; }
    public int completion { get; set; }
    public string difficulty { get; set; }
    public int loop { get; set; }
    public bool[] activeModifiers { get; set; }

    public void SetToDefaults()
    {
        firstTime = true;
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
        loop = 1;
        activeModifiers = new bool[Global.modifiers.Length];
        for (int i = 0; i < activeModifiers.Length; i ++)
        {
            activeModifiers[i] = false;
        }
    }

    [JsonIgnore]
    public bool oneDoorAtATime
    {
        get
        {
            return activeModifiers[0];
        }
    }
    [JsonIgnore]
    public bool doorDrainLess
    {
        get
        {
            return activeModifiers[0];
        }
    }
    [JsonIgnore]
    public bool reloadCam
    {
        get
        {
            return activeModifiers[1];
        }
    }
    [JsonIgnore]
    public bool clearCams
    {
        get
        {
            return activeModifiers[1];
        }
    }
    [JsonIgnore]
    public bool oneMoreMikuling
    {
        get
        {
            return activeModifiers[2];
        }
    }
    [JsonIgnore]
    public bool shockMikulings
    {
        get
        {
            return activeModifiers[2];
        }
    }
    [JsonIgnore]
    public bool autoSeal
    {
        get
        {
            return false;
        }
    }
    [JsonIgnore]
    public bool instaSeal
    {
        get
        {
            return false;
        }
    }
    [JsonIgnore]
    public bool hallucinateGreen
    {
        get
        {
            return true;
        }
    }
    [JsonIgnore]
    public bool hearGreen
    {
        get
        {
            return true;
        }
    }
}
