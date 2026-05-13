using Microsoft.Xna.Framework;

namespace ONAM;

public class ShopNode : GameElement
{
    public string title, desc;
    public int cost;

    private Rectangle topRec, botRec, leftRec, rightRec;
    private int outlineThickness, outlineOffset;
    
    public ShopNode(string iconPath, string title, string desc, int cost) : base(iconPath)
    {
        this.title = title;
        this.desc = desc;
        this.cost = cost;

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
        return leftRec.Height;
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

    private void SetOutline()
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
        Global.spriteBatch.Draw(Global.multiTexture, topRec, Color.White);
        Global.spriteBatch.Draw(Global.multiTexture, leftRec, Color.White);
        Global.spriteBatch.Draw(Global.multiTexture, rightRec, Color.White);
        Global.spriteBatch.Draw(Global.multiTexture, botRec, Color.White);
    }
}
