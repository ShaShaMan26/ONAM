using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ONAM;

public class Game1 : Game
{
    private bool prevActive, fullscreen;

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
        fullscreen = false;

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
                // Global.graphics.IsFullScreen = !Global.graphics.IsFullScreen;
                // Global.graphics.ApplyChanges();
                // MouseManager.LockedToWindow = Global.graphics.IsFullScreen;
                if (fullscreen)
                {
                    Global.graphics.PreferredBackBufferWidth = Global.renderTarget.Width;
                    Global.graphics.PreferredBackBufferHeight = Global.renderTarget.Height;
                    Window.IsBorderless = false;
                    Global.graphics.ApplyChanges();
                }
                else
                {
                    Global.graphics.PreferredBackBufferWidth = Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                    Global.graphics.PreferredBackBufferHeight = Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
                    Window.IsBorderless = true;
                    Global.graphics.ApplyChanges();
                }
                fullscreen = !fullscreen;
            }
            Global.sceneManager.Update();
        }

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
        GraphicsDevice.Clear(Color.CornflowerBlue);
        Global.spriteBatch.Begin();
        Global.spriteBatch.Draw(Global.renderTarget, new Rectangle(0, 0, Global.graphics.PreferredBackBufferWidth, Global.graphics.PreferredBackBufferHeight), Color.White);
        Global.spriteBatch.End();

        base.Draw(gameTime);
    }
}
