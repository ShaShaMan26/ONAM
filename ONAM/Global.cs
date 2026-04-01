using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public static class Global
{
    private static bool RyanIsWatching = true;

    // program globals
    public static SpriteBatch spriteBatch;
    public static GraphicsDeviceManager graphics;
    public static ContentManager content;
    public static GameTime gameTime;

    // scenes
    public static SceneManager sceneManager;
    public static MainMenu mainMenu;
    public static Pause pause;
    public static Night night;
    public static LoadNight loadNight;

    public static void Initialize()
    {
        mainMenu = new();
        mainMenu.Initialize();
        sceneManager = new(mainMenu);
        sceneManager.Initialize();
    }
}
