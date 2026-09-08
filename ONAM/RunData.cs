using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ONAM;

public class RunData
{
    public int loop { get; set; }
    public bool[] activeModifiers { get; set; }
    public bool[] shadowModifiers { get; set; }
    public List<Modifier> enabledMods {get; set;}
    public DifficultyManager difficultyManager { get; set; }

    public int mikusDeterred { get; set; }
    public int mikulingsCalmed { get; set; }
    public int doorsClosed { get; set; }
    public int ventsSealed { get; set; }
    public int powerDrained { get; set; }
    public int deaths { get; set; }
    public int hours { get; set; }

    public void SetDifficulty(string path)
    {
        difficultyManager = JsonSerializer.Deserialize<DifficultyManager>(File.ReadAllText(Global.content.RootDirectory + "/difficulties/" + path + ".txt"));
    }

    public void SetToDefaults()
    {
        loop = 0;
        activeModifiers = new bool[Global.modifiers.Length];
        enabledMods = [];
        for (int i = 0; i < activeModifiers.Length; i ++)
        {
            activeModifiers[i] = false;
        }
        shadowModifiers = [false, false];
        
        ResetStats();
    }
    public void SetToNewLoop()
    {
        loop = 1;
        activeModifiers = new bool[Global.modifiers.Length];
        enabledMods = [];
        for (int i = 0; i < activeModifiers.Length; i ++)
        {
            activeModifiers[i] = false;
        }
        shadowModifiers = [false, false];

        ResetStats();
    }

    private void ResetStats()
    {
        mikusDeterred = 0;
        mikulingsCalmed = 0;
        doorsClosed = 0;
        ventsSealed = 0;
        powerDrained = 0;
        deaths = 0;
        hours = 0;
    }

    public RunData Clone()
    {
        RunData d = new();
        
        d.loop = loop;
        d.activeModifiers = activeModifiers;
        d.shadowModifiers = shadowModifiers;
        d.enabledMods = enabledMods;
        d.difficultyManager = difficultyManager;
        
        d.mikusDeterred = mikusDeterred;
        d.mikulingsCalmed = mikulingsCalmed;
        d.doorsClosed = doorsClosed;
        d.ventsSealed = ventsSealed;
        d.powerDrained = powerDrained;
        d.deaths = deaths;
        d.hours = hours;

        return d;
    }
}
