using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ONAM;

public class Game1 : Game
{
    private bool prevActive;

    public Game1()
    {
        Global.graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Global.graphics.PreferredBackBufferWidth = 1280;
        Global.graphics.PreferredBackBufferHeight = 720;
    }

    protected override void Initialize()
    {
        Global.content = Content;
        prevActive = false;

        Global.Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        Global.spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        Global.gameTime = gameTime;
        KeyboardManager.Update();
        MouseManager.Update();
        
        if (!IsActive && prevActive)
        {
            AudioManager.PauseSFXAll();
            AudioManager.PauseBGM();
        }
        else if (IsActive && !prevActive)
        {
            AudioManager.PlaySFXAll();
            AudioManager.ResumeBGM();
        }
        prevActive = IsActive;

        if (IsActive)
        {
            if (KeyboardManager.KeyPressed(Keys.F))
            {
                Global.graphics.IsFullScreen = !Global.graphics.IsFullScreen;
                Global.graphics.ApplyChanges();
                MouseManager.LockedToWindow = Global.graphics.IsFullScreen;
            }
            Global.sceneManager.Update();
        }

        AudioManager.Update();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        Global.spriteBatch.Begin();
        Global.sceneManager.Draw();
        Global.spriteBatch.End();

        base.Draw(gameTime);
    }
}
