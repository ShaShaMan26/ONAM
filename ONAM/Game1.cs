using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ONAM;

public class Game1 : Game
{
    private bool prevActive, fullscreen;
    private double fullscreenScale;

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
        fullscreenScale = 1;

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
                if (fullscreen)
                {
                    Global.graphics.PreferredBackBufferWidth = Global.renderTarget.Width;
                    Global.graphics.PreferredBackBufferHeight = Global.renderTarget.Height;
                    Window.IsBorderless = false;
                    Window.Position = new(Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width / 2 - Global.graphics.PreferredBackBufferWidth / 2,
                        Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height / 2 - Global.graphics.PreferredBackBufferHeight / 2);
                    Global.graphics.ApplyChanges();
                }
                else
                {
                    Global.graphics.PreferredBackBufferWidth = Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                    Global.graphics.PreferredBackBufferHeight = Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
                    Window.IsBorderless = true;
                    Window.Position = new(Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width / 2 - Global.graphics.PreferredBackBufferWidth / 2,
                        Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height / 2 - Global.graphics.PreferredBackBufferHeight / 2);
                    Global.graphics.ApplyChanges();

                    fullscreenScale = (double) Global.graphics.PreferredBackBufferHeight / Global.renderTarget.Height;
                }
                fullscreen = !fullscreen;
                MouseManager.LockedToWindow = fullscreen;
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
        GraphicsDevice.Clear(Color.Black);
        Global.spriteBatch.Begin();
        if (fullscreen)
        {
            Global.spriteBatch.Draw(Global.renderTarget, new Rectangle(
                Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width / 2 - (int) (Global.renderTarget.Width * fullscreenScale) / 2,
                Global.graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height / 2 - (int) (Global.renderTarget.Height * fullscreenScale) / 2, 
                (int) (Global.renderTarget.Width * fullscreenScale), 
                (int) (Global.renderTarget.Height * fullscreenScale)),
                Color.White);
        }
        else
        {
            Global.spriteBatch.Draw(Global.renderTarget, new Rectangle(0, 0, Global.graphics.PreferredBackBufferWidth, Global.graphics.PreferredBackBufferHeight), Color.White);
        }
        Global.spriteBatch.End();

        base.Draw(gameTime);
    }
}
