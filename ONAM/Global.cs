using System;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public static class Global
{
    // program globals
    public static Game1 game;
    public static Settings settings;
    public static DifficultyManager difficultyManager;
    private static string settingsPath = "./settings.txt";
    public static SpriteBatch spriteBatch;
    public static GraphicsDeviceManager graphics;
    public static ContentManager content;
    public static GameTime gameTime;
    public static RenderTarget2D renderTarget;
    public static double aniDelay;

    // scenes
    public static SceneManager sceneManager;
    public static MainMenu mainMenu;
    public static GameOver gameOver;
    public static Night night;
    public static DifficultySelect difficultySelect;
    public static LoadNight loadNight;
    public static OptionsMenu optionsMenu;

    public static void Initialize()
    {
        renderTarget = new(graphics.GraphicsDevice, 1280, 720);
        double targetFPS = 30d;
        aniDelay = 1.0 / targetFPS;

        settings = new();
        LoadSettings();
        settings.ApplyAll();

        difficultyManager = new();

        optionsMenu = new();

        mainMenu = new();
        mainMenu.Initialize();
        sceneManager = new(mainMenu);
        sceneManager.Initialize();
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

    public static void LoadDifficulty(string path)
    {
        difficultyManager = JsonSerializer.Deserialize<DifficultyManager>(File.ReadAllText(content.RootDirectory + "/difficulties/" + path + ".txt"));
    }
    public static void SaveDifficulty()
    {
        File.WriteAllText("./easy.txt", JsonSerializer.Serialize(difficultyManager));
    }
}
