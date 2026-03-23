using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class CamView : Canvas
{
    public GameElement bg, camBar;
    public Texture2D[] bgTexts;
    public CamButton[] camButtons;

    public override void Initialize()
    {
        base.Initialize();

        bgTexts = new Texture2D[4];
        for (int i = 0; i < bgTexts.Length; i++)
        {
            bgTexts[i] = Global.content.Load<Texture2D>("cam" + (i + 1));
        }

        bg = new("cam1");
        bg.SetPosition(-320, 0);
        Add(7, bg);
        
        camBar = new GameElement("cam_bar");
        camBar.SetPosition(
            Global.graphics.PreferredBackBufferWidth / 2 - camBar.GetWidth() / 2, 
            Global.graphics.PreferredBackBufferHeight - camBar.GetHeight());
        Add(9, camBar);

        camButtons = new CamButton[4];
        for (int i = 0; i < camButtons.Length; i++)
        {
            camButtons[i] = new CamButton(i + 1);
            Add(9, camButtons[i]);
        }

        Add(9, new GameElement("cam_cover"));
        
        GameElement m = new ("cam_map");
        m.SetPosition(new Vector2(Global.graphics.PreferredBackBufferWidth - m.GetWidth() - 20,
            0));
        Add(9, m);

        camButtons[0].SetPosition(m.GetPosition() + new Vector2(150, 60));
        camButtons[1].SetPosition(m.GetPosition() + new Vector2(150, 192));
        camButtons[2].SetPosition(m.GetPosition() + new Vector2(65, 335));
        camButtons[3].SetPosition(m.GetPosition() + new Vector2(230, 335));

        // GameElement e = new("miku");
        // e.SetPosition(Global.graphics.PreferredBackBufferWidth / 2 - e.GetWidth() / 2,
        //     Global.graphics.PreferredBackBufferHeight / 2 - e.GetHeight() / 2);
        // Add(9, e);
    }

    public void SetToCam(int i)
    {
        bg.SetTexture(bgTexts[i - 1]);
        foreach (CamButton cc in camButtons) cc.Deactivate();
        camButtons[i - 1].Activate();
    }
}
