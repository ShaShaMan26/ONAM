using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class CamButton : GameElement
{
    private Texture2D cam_button_inactive, cam_button_active, cbd, warning;
    public int id;
    public bool activeWarning;

    public CamButton(int id) : base("cam_button_inactive")
    {
        this.id = id;
        cbd = Global.content.Load<Texture2D>("cbd" + id);
        cam_button_inactive = Global.content.Load<Texture2D>("cam_button_inactive");
        cam_button_active = Global.content.Load<Texture2D>("cam_button_active");
        warning =  Global.content.Load<Texture2D>("warn");
        activeWarning = false;
    }

    public void Activate()
    {
        SetTexture(cam_button_active);
    }
    public void Deactivate()
    {
        SetTexture(cam_button_inactive);
    }

    public override void Draw()
    {
        base.Draw();
        Global.spriteBatch.Draw(cbd, GetPosition() + new Vector2(7, 7), Color.White);
        if (activeWarning) Global.spriteBatch.Draw(warning, 
            GetPosition() + new Vector2(cam_button_active.Width / 2 - warning.Width / 2 - 1, -warning.Height), Color.White);
    }
}
