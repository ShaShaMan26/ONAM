using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class Miku : GameElement
{
    private Texture2D overlay;
    public float shadow = .8f;
    public Miku next;
    public int id;

    public Miku(string path) : base(path)
    {
        switch (path)
        {
            case "miku":
                id = 0;
                break;
            case "red-miku":
                id = 1;
                break;
            case "yellow-miku":
                id = 1;
                break;
        }
        overlay = Global.content.Load<Texture2D>("shadow");
        next = null;
    }

    public override void Draw()
    {
        base.Draw();

        if (texture != null && visible)
            Global.spriteBatch.Draw(overlay, new Rectangle(pos.ToPoint(), dims.ToPoint()), null, Color.White * shadow, rotation, Vector2.Zero, SpriteEffects.None, 0);
    }
}
