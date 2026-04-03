using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ONAM;

public class Game1 : Game
{
    private bool prevActive;

    public Game1()
    {
        Global.game = this;
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
        // Console.WriteLine(1/ (float)gameTime.ElapsedGameTime.TotalSeconds);
        KeyboardManager.Update();
        MouseManager.Update();
        
        UpdateGame();

        AudioManager.Update();
    }

    protected override void Draw(GameTime gameTime)
    {
        Global.graphics.GraphicsDevice.SetRenderTarget(Global.renderTarget);
        GraphicsDevice.Clear(Color.Black);
        Global.spriteBatch.Begin();
        Global.sceneManager.Draw();
        Global.spriteBatch.End();

        Global.graphics.GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);
        Global.spriteBatch.Begin();
        if (Global.settings.displayMode == 2)
        {
            Global.spriteBatch.Draw(Global.renderTarget, new Rectangle(
                Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width / 2 - (int) (Global.renderTarget.Width * Global.settings.fullscreenScale) / 2,
                Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height / 2 - (int) (Global.renderTarget.Height * Global.settings.fullscreenScale) / 2,
                (int) (Global.renderTarget.Width * Global.settings.fullscreenScale),
                (int) (Global.renderTarget.Height * Global.settings.fullscreenScale)),
                Color.White);
        }
        else
        {
            Global.spriteBatch.Draw(Global.renderTarget, new Rectangle(0, 0, Global.graphics.PreferredBackBufferWidth, Global.graphics.PreferredBackBufferHeight), Color.White);
        }
        Global.spriteBatch.End();

        base.Draw(gameTime);
    }

    private void UpdateGame()
    {
        if (!IsActive && prevActive)
        {
            AudioManager.PauseSFXAll();
            AudioManager.PauseBGM();
        }
        else if (IsActive && !prevActive
            && Global.sceneManager.currScene.GetType() != typeof(Pause))
        {
            AudioManager.PlaySFXAll();
            AudioManager.ResumeBGM();
        }
        prevActive = IsActive;

        if (IsActive)
        {
            Global.sceneManager.Update();
        }
    }
}
