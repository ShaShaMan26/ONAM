using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public static class Global
{
    // program globals
    public static Game1 game;
    public static Settings settings;
    public static RunData runData;
    public static UserData userData;
    public static Modifier[] modifiers;
    private static string settingsPath = "./settings.txt";
    private static string savedataPath = "./savedata.txt";
    public static SpriteBatch spriteBatch;
    public static GraphicsDeviceManager graphics;
    public static ContentManager content;
    public static GameTime gameTime;
    public static RenderTarget2D renderTarget;
    public static double aniDelay;
    public static Texture2D multiTexture;
    public static SFXObject selectSFX;

    public static bool customNight;

    // scenes
    public static SceneManager sceneManager;
    public static MainMenu mainMenu;
    public static GameOver gameOver;
    public static Night night;
    public static DifficultySelect difficultySelect;
    public static ModSelect modSelect;
    public static LoadNight loadNight;
    public static OptionsMenu optionsMenu;
    public static CustomSelect customSelect;
    public static Summary summary;
    public static RunHistory runHistory;

    public static void Initialize()
    {
        customNight = false;

        multiTexture = new(graphics.GraphicsDevice, 1, 1);
        multiTexture.SetData([Color.White]);

        renderTarget = new(graphics.GraphicsDevice, 1280, 720);
        double targetFPS = 30d;
        aniDelay = 1.0 / targetFPS;

        modifiers = JsonSerializer.Deserialize<Modifier[]>(File.ReadAllText(content.RootDirectory + "/modifiers.txt"));

        userData = new();
        LoadUserData();

        settings = new();
        LoadSettings();
        settings.ApplyAll();

        runData = userData.mainRun;

        optionsMenu = new();
        customSelect = new();
        modSelect = new();
        summary = new();
        summary.Initialize();
        runHistory = new();
        runHistory.Initialize();

        mainMenu = new();
        mainMenu.Initialize();
        sceneManager = new(mainMenu);
        sceneManager.Initialize();

        selectSFX = new(content.Load<SoundEffect>("sfx/cam_switch"));
    }
    
    public static void LoadSettings()
    {
        if (!File.Exists(settingsPath))
        {
            SaveSettings();
            return;
        }
        settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(settingsPath));
    }
    public static void SaveSettings()
    {
        File.WriteAllText(settingsPath, JsonSerializer.Serialize(settings));
    }

    public static void SaveUserData()
    {
        if (runData != null)
        {
            if (customNight) userData.customRun = runData;
            else userData.mainRun = runData;
        }
        File.WriteAllText(savedataPath, JsonSerializer.Serialize(userData));
    }
    public static void LoadUserData()
    {
        if (!File.Exists(savedataPath))
        {
            userData = new();
            userData.SetToDefaults();
            SaveUserData();
            return;
        } 
        else userData = JsonSerializer.Deserialize<UserData>(File.ReadAllText(savedataPath));
    }
    public static void ResetUserData()
    {
        userData.SetToDefaults();
        runData = userData.mainRun;
        SaveUserData();
    }

    // public static void LoadDifficulty(string path)
    // {
    //     difficultyManager = JsonSerializer.Deserialize<DifficultyManager>(File.ReadAllText(content.RootDirectory + "/difficulties/" + path + ".txt"));
    // }
    // public static void SaveDifficulty()
    // {
    //     File.WriteAllText("./easy.txt", JsonSerializer.Serialize(difficultyManager));
    // }
}
