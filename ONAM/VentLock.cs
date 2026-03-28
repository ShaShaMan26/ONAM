using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class VentLock : GameElement
{
    private Texture2D vent_inactive, vent_active;
    public bool closed;

    public VentLock() : base("vent_inactive")
    {
        vent_inactive = texture;
        vent_active = Global.content.Load<Texture2D>("vent_active");
    }

    public void Toggle()
    {
        if (closed)
        {
            SetTexture(vent_inactive);
        }
        else
        {
            SetTexture(vent_active);
        }

        closed = !closed;
    }
}
