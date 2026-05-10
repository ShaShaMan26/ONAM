using System;
using Microsoft.Xna.Framework;

namespace ONAM;

public class SettingsButton : GameElement
{
    public TextDisplay title;
    private GameElement leftButton, rightButton;
    private Action leftClick, rightClick, onClick;

    public SettingsButton(string title, Action leftClick, Action rightClick, Action onClick) : base("miku")
    {
        visible = false;

        this.title = new(title, "consolas");
        this.title.MapBoundsToTextSize();
        this.leftClick = leftClick;
        this.rightClick = rightClick;
        this.onClick = onClick;
        leftButton = new("left_button");
        rightButton = new("right_button");

        leftButton.SetPosition(pos);
        this.title.SetPosition(pos.X + leftButton.GetWidth(), pos.Y);
        rightButton.SetPosition(this.title.GetPosition().X + this.title.GetWidth(), pos.Y);
    }

    public void Initialize()
    {
        onClick.Invoke();
        CenterText();
    }

    public override void SetPosition(Vector2 pos)
    {
        float dist = rightButton.GetPosition().X - leftButton.GetPosition().X + leftButton.GetWidth();
        leftButton.SetPosition(pos);
        rightButton.SetPosition(leftButton.GetPosition().X + dist, pos.Y);
        CenterText();
        base.SetPosition(pos);
    }
    public void CenterText()
    {
        float dist = rightButton.GetPosition().X - leftButton.GetPosition().X + leftButton.GetWidth();
        title.MapBoundsToTextSize();
        title.SetPosition(leftButton.GetPosition().X
            + dist / 2 - title.GetWidth() / 2,
            rightButton.GetPosition().Y);
    }

    public override float GetWidth()
    {
        return rightButton.GetPosition().X + rightButton.GetWidth() - leftButton.GetPosition().X;
    }
    public override float GetHeight()
    {
        return title.GetHeight();
    }

    public bool Update()
    {
        if (MouseManager.LeftButtonClicked)
        {
            if (leftButton.GetBounds().Contains(MouseManager.Location))
            {
                leftClick.Invoke();
                onClick.Invoke();
                CenterText();
                return true;
            }
            else if (rightButton.GetBounds().Contains(MouseManager.Location))
            {
                rightClick.Invoke();
                onClick.Invoke();
                CenterText();
                return true;
            }
        }
        return false;
    }

    public override void Draw()
    {
        title.Draw();
        leftButton.Draw();
        rightButton.Draw();
    }
}
