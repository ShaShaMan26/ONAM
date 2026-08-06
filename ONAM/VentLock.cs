using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class VentLock : GameElement
{
    private Texture2D vent_inactive, vent_active;
    public bool closed;

    public VentLock() : base("vent_active")
    {
        vent_inactive = texture;
        vent_active = Global.content.Load<Texture2D>("vent_inactive");
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
