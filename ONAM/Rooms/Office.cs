using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class Office : Canvas
{
    public MikulingManager mikulingManager;
    public GameElement bg, camBar, camTablet, jumpscarePNG, nose;
    public CamReloadBar camReloadBar;
    public Door_L door_L;
    public Door_R door_R;
    public GameElement door_button_holder_l, door_button_l, door_button_holder_r, door_button_r;
    public GameElement door_eyes_l, door_eyes_r;
    public Texture2D[] tabAni;
    public Sign sign;

    public override void Initialize()
    {
        base.Initialize();

        tabAni = new Texture2D[11];
        for (int i = 0; i < 11; i ++)
        {
            tabAni[i] = Global.content.Load<Texture2D>("ani_camflip/" + i);
        }

        bg = new GameElement("office");
        Add(6, bg);

        camTablet = new GameElement("ani_camflip/0");
        camTablet.visible = false;
        Add(8, camTablet);

        nose = new("Miku");
        nose.visible = false;
        nose.SetDimensions(14, 8);
        nose.opacity = .5f;
        Add(9, nose);

        jumpscarePNG = new("miku");
        jumpscarePNG.visible = false;
        jumpscarePNG.SetDimensions(1200, 1199);
        jumpscarePNG.SetPosition(
            Global.renderTarget.Width / 2 - jumpscarePNG.GetWidth() / 2, 
            Global.renderTarget.Height / 2 - jumpscarePNG.GetHeight() / 2);
        Add(9, jumpscarePNG);

        camBar = new GameElement("cam_bar");
        camBar.SetPosition(
            Global.renderTarget.Width / 2 - camBar.GetWidth() / 2, 
            Global.renderTarget.Height - camBar.GetHeight());
        camReloadBar = new();
        camReloadBar.SetPosition(camBar.GetPosition());
        camReloadBar.visible = false;
        Add(9, camReloadBar);
        Add(9, camBar);

        door_L = new();
        Add(7, door_L);

        door_button_l = new("door_button_active_l");
        door_button_l.visible = false;
        Add(7, door_button_l);
        door_button_holder_l = new("door_button_l");
        Add(7, door_button_holder_l);

        door_button_r = new("door_button_active_r");
        door_button_r.visible = false;
        Add(7, door_button_r);
        door_button_holder_r = new("door_button_r");
        Add(7, door_button_holder_r);

        door_R = new();
        Add(7, door_R);

        door_eyes_l = new("eyes");
        door_eyes_l.SetDimensions(400, 399);
        door_eyes_l.opacity = 0;
        door_eyes_r = new("eyes");
        door_eyes_r.SetDimensions(400, 399);
        door_eyes_r.opacity = 0;
        Add(7, door_eyes_l);
        Add(7, door_eyes_r);

        mikulingManager = new();
        mikulingManager.Initialize();
        if (Global.runData.difficultyManager.m_level > 0) Add(7, mikulingManager);

        sign = new();
        Add(7, sign);
        sign.visible = ModifierManager.letsGoGambling;
    }
}
