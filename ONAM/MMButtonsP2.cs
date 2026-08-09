namespace ONAM;

public class MMButtonsP2 : GameElement
{
    Canvas canvas;

    private TextDisplay[] buttons;
    private TextDisplay buttonHighlight;

    public MMButtonsP2() : base("miku")
    {
        
    }

    public void Initialize()
    {
        canvas = new();
        canvas.Initialize();

        // buttons
        buttonHighlight = new(">>", "consolas");
        buttonHighlight.MapBoundsToTextSize();
        canvas.Add(9, buttonHighlight);
        
        buttons = [
            new TextDisplay("Credits", "consolas"),
            new TextDisplay("Run History", "consolas"),
            new TextDisplay("Custom Night", "consolas"),
            new TextDisplay("Back", "consolas")
        ];

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].MapBoundsToTextSize();
            canvas.Add(9, buttons[i]);
            if (i > 0)
            {
                buttons[i].SetPosition(buttons[i - 1].GetPosition().X,
                    buttons[i - 1].GetPosition().Y + buttons[i - 1].GetHeight() + (Global.userData.completion > 0 ? 8 : 18));
            }
            else
            {
                buttons[i].SetPosition(125, 372);
            }
        }

        buttonHighlight.SetPosition(
            buttons[^1].GetPosition().X - buttonHighlight.GetWidth() - 5,
            buttons[^1].GetPosition().Y);
    }

    public Scene Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetBounds().Contains(MouseManager.Location))
            {   
                if (buttonHighlight.GetPosition().Y != buttons[i].GetPosition().Y)
                {
                    buttonHighlight.SetPosition(
                        buttons[i].GetPosition().X - buttonHighlight.GetWidth() - 5,
                        buttons[i].GetPosition().Y);
                    if (Global.selectSFX.PlaybackClosed) AudioManager.AddSFX(Global.selectSFX);
                    buttonHighlight.visible = true;
                }
                
                if (MouseManager.LeftButtonReleased)
                {
                    switch (i)
                    {
                        case 0:
                            TransFlicker c = new(Global.credits);
                            Global.credits.Initialize();
                            c.Initialize();
                            return c;
                        case 1:
                            TransFlicker t = new(Global.runHistory);
                            Global.runHistory.Initialize();
                            t.Initialize();
                            return t;
                        case 2:
                            TransFlicker b = new(Global.customSelect);
                            Global.customSelect.Initialize();
                            b.Initialize();
                            return b;
                        case 3:
                            Global.mainMenu.TogglePage();
                            return null;
                    }
                }
                return null;
            }
        }
        if (buttonHighlight.visible)
        {
            buttonHighlight.visible = false;
            buttonHighlight.SetPosition(0, 0);
        }
        return null;
    }

    public override void Draw()
    {
        if (visible) canvas.Draw();
    }
}
