using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class TextDisplay : GameElement
{
    public SpriteFont Font { get; set; }
    public float FontScale { get; set; } = 1;
    public Color TextColor { get; set; } = Color.White;
    public string Text { get; set; } = "Hello World";

    public void MapBoundsToTextSize()
    {
        if (Text != null)
        {
            SetDimensions(Font.MeasureString(Text));
        }
    }

    public override void Draw()
    {
        base.Draw();
        if (visible && Text != null)
        {
            Global.spriteBatch.DrawString(Font, Text, pos, TextColor * opacity, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);
        }
    }

    public TextDisplay(string font_file_name) : base("miku")
    {
        texture = null;
        Font = Global.content.Load<SpriteFont>("fonts/" + font_file_name);
        MapBoundsToTextSize();
    }
    public TextDisplay(string text, string font_file_name) : this(font_file_name)
    {
        Text = text;
        MapBoundsToTextSize();
    }
}
