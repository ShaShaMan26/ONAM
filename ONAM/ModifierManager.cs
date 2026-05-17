namespace ONAM;

public static class ModifierManager
{
    public static bool oneDoorAtATime
    {
        get
        {
            return false;
        }
    }
    
    public static bool doorDrainLess
    {
        get
        {
            return Global.runData.activeModifiers[1];
        }
    }
    
    public static bool reloadCam
    {
        get
        {
            return Global.runData.activeModifiers[2];
        }
    }
    
    public static bool clearCams
    {
        get
        {
            return Global.runData.activeModifiers[6];
        }
    }
    
    public static bool oneMoreMikuling
    {
        get
        {
            return Global.runData.activeModifiers[3];
        }
    }
    
    public static bool shockMikulings
    {
        get
        {
            return false;
        }
    }
    
    public static bool autoSeal
    {
        get
        {
            return false;
        }
    }
    
    public static bool instaSeal
    {
        get
        {
            return Global.runData.activeModifiers[4];
        }
    }
    
    public static bool hallucinateGreen
    {
        get
        {
            return Global.runData.activeModifiers[7];
        }
    }
    
    public static bool hearGreen
    {
        get
        {
            return Global.runData.activeModifiers[7];
        }
    }
    
    public static bool intermission
    {
        get
        {
            return false;
        }
    }
    
    public static bool fastNight
    {
        get
        {
            return Global.runData.activeModifiers[5];
        }
    }
    
    public static bool shadowMiku
    {
        get
        {
            return Global.runData.activeModifiers[3];
        }
    }
    
    public static bool peekaboo
    {
        get
        {
            return Global.runData.activeModifiers[2];
        }
    }
    
    public static bool noLateSeal
    {
        get
        {
            return Global.runData.activeModifiers[4];
        }
    }
    
    public static bool doorStuck
    {
        get
        {
            return Global.runData.activeModifiers[1];
        }
    }
    
    public static bool shadowOffice
    {
        get
        {
            return false;
        }
    }
    
    public static bool letsGoGambling
    {
        get
        {
            return Global.runData.activeModifiers[0];
        }
    }
    
    public static bool theGas
    {
        get
        {
            return Global.runData.activeModifiers[5];
        }
    }
    
    public static bool tricky
    {
        get
        {
            return Global.runData.activeModifiers[6];
        }
    }
}
