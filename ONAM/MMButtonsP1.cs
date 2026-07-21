using System;
using Microsoft.Xna.Framework;

namespace ONAM;

public class MMButtonsP1 : GameElement
{
    Canvas canvas;

    private string contText, contTextFull;
    private TextDisplay[] buttons;
    private TextDisplay buttonHighlight;

    public MMButtonsP1() : base("miku")
    {
        
    }

    public void Initialize()
    {
        canvas = new();
        canvas.Initialize();

        // buttons
        buttonHighlight = new(">>", "consolas");
        buttonHighlight.visible = false;
        buttonHighlight.MapBoundsToTextSize();
        canvas.Add(9, buttonHighlight);

        contText = "Continue";
        contTextFull = contText + " " + Global.runData.loop;

        if (Global.userData.completion > 0)
        {
            buttons = [
                new TextDisplay("New Game", "consolas"),
                new TextDisplay(contText, "consolas"),
                new TextDisplay("Options", "consolas"),
                new TextDisplay("Extras", "consolas"),
                new TextDisplay("Quit Game", "consolas")
            ];
        }
        else
        {
            buttons = [
                new TextDisplay("New Game", "consolas"),
                new TextDisplay(contText, "consolas"),
                new TextDisplay("Options", "consolas"),
                new TextDisplay("Quit Game", "consolas")
            ];
        }

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

        if (Global.runData.loop < 1) buttons[1].opacity = .65f;
    }

    public Scene Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetBounds().Contains(MouseManager.Location))
            {   
                if (i == 1)
                {
                    if (Global.runData.loop < 1) return null;
                    else if (buttons[i].Text != contTextFull) buttons[i].Text = contTextFull;
                }
                else if (buttons[i].Text != contText) buttons[1].Text = contText;

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
                            if (Global.runData.loop > 0)
                            {
                                Confirm c = new Confirm(
                                    Global.mainMenu,
                                    "Start New Game?",
                                    "(Current run will be lost).",
                                    () =>
                                    {
                                        Global.difficultySelect = new();
                                        TransFlicker j = new(Global.difficultySelect);
                                        Global.difficultySelect.Initialize();
                                        j.Initialize();
                                        Global.sceneManager.currScene = j;
                                    },
                                    Global.mainMenu.UpdateAnimations
                                );
                                c.Initialize();
                                return c;
                            }
                            else
                            {
                                Global.difficultySelect = new();
                                TransFlicker j = new(Global.difficultySelect);
                                Global.difficultySelect.Initialize();
                                j.Initialize();
                                return j;
                            }
                        case 1:
                            AudioManager.PauseBGM();
                            LoadNight l = new();
                            l.Initialize();
                            return l;
                        case 2:
                            TransFlicker t = new(Global.optionsMenu);
                            Global.optionsMenu.Initialize(() =>
                            {
                                Global.SaveSettings();
                                TransFlicker t = new(Global.mainMenu);
                                t.Initialize();
                                Global.sceneManager.currScene = t;
                            });
                            t.Initialize();
                            return t;
                        case 3:
                            if (Global.userData.completion > 0)
                            {
                                Global.mainMenu.TogglePage();
                                return null;
                            }
                            Confirm b = new(
                                Global.mainMenu, 
                                "Quit Game?",
                                () => Environment.Exit(0),
                                Global.mainMenu.UpdateAnimations
                            );
                            b.Initialize();
                            return b;
                        case 4:
                            Confirm d = new(
                                Global.mainMenu, 
                                "Quit Game?",
                                () => Environment.Exit(0),
                                Global.mainMenu.UpdateAnimations
                            );
                            d.Initialize();
                            return d;
                    }
                }
                return null;
            }
        }
        if (buttonHighlight.visible)
        {
            buttonHighlight.visible = false;
            buttonHighlight.SetPosition(0, 0);
            buttons[1].Text = contText;
        }
        return null;
    }

    public override void Draw()
    {
        if (visible) canvas.Draw();
    }
}
