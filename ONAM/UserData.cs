using System.IO;
using System.Text.Json.Serialization;

namespace ONAM;

public class UserData
{
    public bool firstTime { get; set; }
    public int completion { get; set; }
    public string difficulty { get; set; }
    public int loop { get; set; }
    public bool[] activeModifiers { get; set; }
    public bool[] customModifiers { get; set; }

    public void SetToDefaults()
    {
        firstTime = true;
        completion = 0;
        difficulty = "easy";
        loop = 0;
        activeModifiers = new bool[Global.modifiers.Length];
        customModifiers = new bool[Global.modifiers.Length];
        for (int i = 0; i < activeModifiers.Length; i ++)
        {
            activeModifiers[i] = false;
            customModifiers[i] = false;
            File.Delete(Global.content.RootDirectory + "/difficulties/custom.txt");
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
            if (Global.customNight) return customModifiers[0];
            return activeModifiers[0];
        }
    }
    [JsonIgnore]
    public bool doorDrainLess
    {
        get
        {
            return false;
        }
    }
    [JsonIgnore]
    public bool reloadCam
    {
        get
        {
            if (Global.customNight) return customModifiers[1];
            return activeModifiers[1];
        }
    }
    [JsonIgnore]
    public bool clearCams
    {
        get
        {
            if (Global.customNight) return customModifiers[2];
            return activeModifiers[2];
        }
    }
    [JsonIgnore]
    public bool oneMoreMikuling
    {
        get
        {
            return false;
        }
    }
    [JsonIgnore]
    public bool shockMikulings
    {
        get
        {
            if (Global.customNight) return customModifiers[3];
            return activeModifiers[3];
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
            if (Global.customNight) return customModifiers[4];
            return activeModifiers[4];
        }
    }
    [JsonIgnore]
    public bool hallucinateGreen
    {
        get
        {
            if (Global.customNight) return customModifiers[5];
            return activeModifiers[5];
        }
    }
    [JsonIgnore]
    public bool hearGreen
    {
        get
        {
            if (Global.customNight) return customModifiers[6];
            return activeModifiers[6];
        }
    }
    [JsonIgnore]
    public bool intermission
    {
        get
        {
            if (Global.customNight) return customModifiers[7];
            return activeModifiers[7];
        }
    }
    [JsonIgnore]
    public bool fastNight
    {
        get
        {
            if (Global.customNight) return customModifiers[8];
            return activeModifiers[8];
        }
    }
    [JsonIgnore]
    public bool shadowMiku
    {
        get
        {
            if (Global.customNight) return customModifiers[9];
            return activeModifiers[9];
        }
    }
    [JsonIgnore]
    public bool peekaboo
    {
        get
        {
            if (Global.customNight) return customModifiers[10];
            return activeModifiers[10];
        }
    }
    [JsonIgnore]
    public bool noLateSeal
    {
        get
        {
            if (Global.customNight) return customModifiers[11];
            return activeModifiers[11];
        }
    }
    [JsonIgnore]
    public bool doorStuck
    {
        get
        {
            if (Global.customNight) return customModifiers[12];
            return activeModifiers[12];
        }
    }
    [JsonIgnore]
    public bool shadowOffice
    {
        get
        {
            if (Global.customNight) return customModifiers[13];
            return activeModifiers[13];
        }
    }
    [JsonIgnore]
    public bool letsGoGambling
    {
        get
        {
            if (Global.customNight) return customModifiers[14];
            return activeModifiers[14];
        }
    }
    [JsonIgnore]
    public bool theGas
    {
        get
        {
            if (Global.customNight) return customModifiers[15];
            return activeModifiers[15];
        }
    }
    [JsonIgnore]
    public bool tricky
    {
        get
        {
            if (Global.customNight) return customModifiers[16];
            return activeModifiers[16];
        }
    }
}
