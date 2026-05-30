using Microsoft.Xna.Framework;

namespace ONAM;

public class ModNode : GameElement
{
    public int id;
    public Modifier modifier;

    private Rectangle topRec, botRec, leftRec, rightRec;
    public int outlineThickness, outlineOffset;
    public bool drawOutline;
    public float borderOpacity;

    public ModNode(Modifier modifier, int id) : base(modifier.iconPath)
    {
        drawOutline = true;
        this.modifier = modifier;
        this.id = id;
        borderOpacity = 1;

        outlineThickness = 6;
        outlineOffset = 4;

        SetOutline();
    }

    public override float GetWidth()
    {
        return topRec.Width;
    }
    public override float GetHeight()
    {
        return leftRec.Height + outlineThickness * 2;
    }
    public override Rectangle GetBounds()
    {
        return new(topRec.X, topRec.Y, (int) GetWidth(), (int) GetHeight());
    }
    public override Vector2 GetPosition()
    {
        return base.GetPosition()
            - new Vector2(pos.X - leftRec.X, pos.Y - topRec.Y);
    }

    public void SetOutline()
    {
        topRec = new((int) (pos.X - outlineThickness * 2 - outlineOffset), (int) (pos.Y - outlineOffset - outlineThickness * 2),
            (int) (dims.X + outlineOffset * 2 + outlineThickness * 4), outlineThickness);
        leftRec = new((int) (pos.X - outlineThickness * 2 - outlineOffset), (int) (pos.Y - outlineOffset - outlineThickness),
            outlineThickness, (int) (dims.Y + outlineOffset * 2 + outlineThickness * 2));
        rightRec = new((int) (pos.X + dims.X + outlineOffset + outlineThickness), (int) (pos.Y - outlineOffset - outlineThickness),
            outlineThickness, (int) (dims.Y + outlineOffset * 2 + outlineThickness * 2));
        botRec = new((int) (pos.X - outlineThickness * 2 - outlineOffset), (int) (pos.Y + dims.Y + outlineOffset + outlineThickness),
            (int) (dims.X + outlineOffset * 2 + outlineThickness * 4), outlineThickness);
    }

    public override void SetPosition(Vector2 pos)
    {
        base.SetPosition(pos + new Vector2(this.pos.X - leftRec.X, this.pos.Y - topRec.Y));
        SetOutline();
    }

    public override void Draw()
    {
        base.Draw();
        
        // draw outline
        if (!drawOutline) return;
        Global.spriteBatch.Draw(Global.multiTexture, topRec, Color.White * borderOpacity);
        Global.spriteBatch.Draw(Global.multiTexture, leftRec, Color.White * borderOpacity);
        Global.spriteBatch.Draw(Global.multiTexture, rightRec, Color.White * borderOpacity);
        Global.spriteBatch.Draw(Global.multiTexture, botRec, Color.White * borderOpacity);
    }
}
