using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ONAM;

public class RunData
{
    public int loop { get; set; }
    public bool[] activeModifiers { get; set; }
    // public List<Modifier> enabledMods {get; set;}
    public DifficultyManager difficultyManager { get; set; }

    public bool shopAccessible { get; set; }
    public int bankedTokens { get; set; }
    public int spentTokens { get; set; }

    public void SetDifficulty(string path)
    {
        difficultyManager = JsonSerializer.Deserialize<DifficultyManager>(File.ReadAllText(Global.content.RootDirectory + "/difficulties/" + path + ".txt"));
    }

    public void SetToDefaults()
    {
        loop = 0;
        activeModifiers = new bool[Global.modifiers.Length];
        for (int i = 0; i < activeModifiers.Length; i ++)
        {
            activeModifiers[i] = false;
        }
        
        shopAccessible = false;
        ResetTokens();
    }
    public void SetToNewLoop()
    {
        loop = 1;
        activeModifiers = new bool[Global.modifiers.Length];
        for (int i = 0; i < activeModifiers.Length; i ++)
        {
            activeModifiers[i] = false;
        }

        shopAccessible = false;
        ResetTokens();
    }

    public void ResetTokens()
    {
        bankedTokens = 0;
        spentTokens = 0;
    }
}
