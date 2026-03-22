using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class Office : Canvas
{
    public GameElement bg, camBar;
    public GameElement camTablet;
    public Texture2D[] tabAni;

    public override void Initialize()
    {
        base.Initialize();

        tabAni = new Texture2D[10];
        for (int i = 1; i < 11; i ++)
        {
            tabAni[i - 1] = Global.content.Load<Texture2D>("ani_camflip/" + i);
        }

        bg = new GameElement("office");
        Add(7, bg);

        camTablet = new GameElement("ani_camflip/1");
        camTablet.visible = false;
        Add(8, camTablet);

        camBar = new GameElement("cam_bar");
        camBar.SetPosition(
            Global.graphics.PreferredBackBufferWidth / 2 - camBar.GetWidth() / 2, 
            Global.graphics.PreferredBackBufferHeight - camBar.GetHeight());
        Add(9, camBar);
    }
}
