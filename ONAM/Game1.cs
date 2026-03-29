using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ONAM;

public class Game1 : Game
{
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
        if (KeyboardManager.KeyPressed(Keys.Escape))
            Exit();
        if (KeyboardManager.KeyPressed(Keys.F))
        {
            Global.graphics.IsFullScreen = !Global.graphics.IsFullScreen;
            Global.graphics.ApplyChanges();
            MouseManager.LockedToWindow = Global.graphics.IsFullScreen;
        }

        Global.stateManager.Update();
        Global.office.door_L.Update();
        Global.office.door_R.Update();
        if (!Global.jumpytime) Global.UpdateMikus();
        // if (!Global.jumpytime) Global.mikus[3].Update();
        if (Global.jumpytime)
        {
            if (Global.stateManager.currState.GetType() == typeof(InOffice))
            {
                Global.jumpytime = false;
                Global.stateManager.currState = Global.jumpscare;
            }
            else if (Global.stateManager.currState.GetType() == typeof(InCams))
            {
                Global.stateManager.currState = Global.closeCams;
            }
        }
        Global.camView.UpdateAnimations();

        base.Update(gameTime);
        AudioManager.Update();

        // demo
        // foreach (Miku m in Global.mikus)
        // {
        //     if (m.level < 20) return; 
        // }
        // if (Global.jumpytime) return;
        // Global.office.jumpscarePNG.SetTexture(Global.content.Load<Texture2D>("shadow"));
        // AudioManager.AddSFX(new SFXObject(Global.content.Load<SoundEffect>("sfx/yay")));
        // Global.jumpytime = true;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        Global.spriteBatch.Begin();
        Global.canvas.Draw();
        Global.spriteBatch.End();

        base.Draw(gameTime);
    }
}
