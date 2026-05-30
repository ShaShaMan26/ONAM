using System;
using Microsoft.Xna.Framework;

namespace ONAM;

public class NavButton : TextDisplay
{
    private string title;
    private Action onPress;
    private float offset;

    public NavButton(string title, Action onPress) : base(title, "consolas")
    {
        this.title = title;
        this.onPress = onPress;

        offset = new TextDisplay("> ", "consolas").GetWidth();
    }

    private void Recenter()
    {
        MapBoundsToTextSize();
        SetPosition(pos.X - offset * (Text == title ? -1 : 1), pos.Y);
    }
    public void Update()
    {
        if (GetBounds().Contains(MouseManager.Location))
        {
            if (Text != "> " + title + " <")
            {
                if (Global.selectSFX.PlaybackClosed) AudioManager.AddSFX(Global.selectSFX);
                Text = "> " + title + " <";
                Recenter();
            }
            if (MouseManager.LeftButtonReleased)
            {
                onPress.Invoke();
            }
        }
        else if (Text != title)
        {
            Text = title;
            Recenter();
        }
    }
}
