using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class GameElement
{
    protected Vector2 pos, dims;
    protected Texture2D texture;
    public float opacity, rotation;
    public Color color;

    public bool visible;

    public GameElement(Vector2 pos, string path)
    {
        color = Color.White;
        opacity = 1;
        visible = true;
        rotation = 0;

        this.pos = pos;
        texture = Global.content.Load<Texture2D>(path);
        dims = texture.Bounds.Size.ToVector2();
    }
    public GameElement(string path) : this(Vector2.Zero, path) { }

    public void SetTexture(Texture2D texture)
    {
        this.texture = texture;
    }

    public virtual void SetPosition(Vector2 pos)
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

    public virtual float GetWidth()
    {
        return dims.X;
    }
    public virtual float GetHeight()
    {
        return dims.Y;
    }

    public void SetDimensions(Vector2 dims)
    {
        this.dims = dims;
    }
    public void SetDimensions(int w, int h)
    {
        dims = new Vector2(w, h);
    }

    public virtual Rectangle GetBounds()
    {
        return new Rectangle(pos.ToPoint(), dims.ToPoint());
    }

    public virtual void Draw()
    {
        if (texture != null && visible)
            Global.spriteBatch.Draw(texture, new Rectangle(pos.ToPoint(), dims.ToPoint()), null, color * opacity, rotation, Vector2.Zero, SpriteEffects.None, 0);
    }
}
