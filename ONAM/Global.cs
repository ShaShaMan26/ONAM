using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public static class Global
{
    // program globals
    public static SpriteBatch spriteBatch;
    public static GraphicsDeviceManager graphics;
    public static ContentManager content;
    public static GameTime gameTime;
    public static RenderTarget2D renderTarget;
    public static double aniDelay;

    // scenes
    public static SceneManager sceneManager;
    public static MainMenu mainMenu;
    public static Pause pause;
    public static GameOver gameOver;
    public static Night night;
    public static LoadNight loadNight;

    public static void Initialize()
    {
        renderTarget = new(graphics.GraphicsDevice, 1280, 720);
        double targetFPS = 30d;
        aniDelay = 1.0 / targetFPS;

        mainMenu = new();
        mainMenu.Initialize();
        sceneManager = new(mainMenu);
        sceneManager.Initialize();
    }
}
