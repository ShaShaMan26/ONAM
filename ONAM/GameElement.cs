using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class GameElement
{
    private Vector2 pos, dims;
    private Texture2D texture;
    
    public bool visible;

    public GameElement(Vector2 pos, string path)
    {
        visible = true;

        this.pos = pos;
        texture = Global.content.Load<Texture2D>(path);
        dims = texture.Bounds.Size.ToVector2();
    }
    public GameElement(string path) : this(Vector2.Zero, path) { }

    public void SetTexture(Texture2D texture)
    {
        this.texture = texture;
    }

    public void SetPosition(Vector2 pos)
    {
        this.pos = pos;
    }
    public void SetPosition(float x, float y) 
    {
        SetPosition(new Vector2(x, y));
    }

    public Vector2 GetPosition()
    {
        return pos;
    }

    public float GetWidth()
    {
        return dims.X;
    }
    public float GetHeight()
    {
        return dims.Y;
    }

    public Rectangle GetBounds()
    {
        return new Rectangle(pos.ToPoint(), dims.ToPoint());
    }

    public virtual void Draw()
    {
        if (texture != null && visible)
            Global.spriteBatch.Draw(texture, pos, Color.White);
    }
}
