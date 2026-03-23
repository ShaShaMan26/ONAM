using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class CamView : Canvas
{
    public GameElement bg, camBar;
    public Texture2D cam1, cam2, cam3, cam4;

    public override void Initialize()
    {
        base.Initialize();

        cam1 = Global.content.Load<Texture2D>("cam1");
        cam2 = Global.content.Load<Texture2D>("cam2");
        cam3 = Global.content.Load<Texture2D>("cam3");
        cam4 = Global.content.Load<Texture2D>("cam4");

        // Add(7, new GameElement("cam_cover"));
        bg = new("cam1");
        bg.SetPosition(-320, 0);
        Add(7, bg);
        
        camBar = new GameElement("cam_bar");
        camBar.SetPosition(
            Global.graphics.PreferredBackBufferWidth / 2 - camBar.GetWidth() / 2, 
            Global.graphics.PreferredBackBufferHeight - camBar.GetHeight());
        Add(9, camBar);
    }
}
