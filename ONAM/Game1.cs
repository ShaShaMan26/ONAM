using Microsoft.Xna.Framework;
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

        // TODO: use this.Content to load your game content here
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
        }

        Global.stateManager.Update();
        // Global.UpdateMikus();

        base.Update(gameTime);
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
