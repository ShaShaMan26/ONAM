using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class Pause : Scene
{
    private Scene prevScene;

    private Canvas canvas;
    private GameElement cover;

    private SFXObject select;
    private TextDisplay[] buttons;
    private TextDisplay buttonHighlight;

    public Pause(Scene scene)
    {
        prevScene = scene;
    }


    public override void Initialize()
    {
        AudioManager.PauseSFXAll();
        AudioManager.PauseBGM();

        canvas = new();
        canvas.Initialize();

        cover = new("ani_flicker/0");
        cover.color = Color.Black;
        cover.opacity = .8f;
        canvas.Add(6, cover);

        // buttons

        select = new(Global.content.Load<SoundEffect>("sfx/cam_switch"));
        select.Volume = .75f;

        TextDisplay t = new("PAUSED", "fnaf-big");
        t.SetPosition(-15, 110);
        canvas.Add(5, t);

        buttonHighlight = new(">>", "consolas");
        buttonHighlight.visible = false;
        buttonHighlight.MapBoundsToTextSize();
        canvas.Add(9, buttonHighlight);

        buttons = new TextDisplay[4];
        buttons[0] = new TextDisplay("Resume", "consolas");
        buttons[1] = new TextDisplay("Options", "consolas");
        buttons[2] = new TextDisplay("Return to Title", "consolas");
        buttons[3] = new TextDisplay("Quit Game", "consolas");
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].MapBoundsToTextSize();
            canvas.Add(9, buttons[i]);
            if (i > 0)
            {
                buttons[i].SetPosition(buttons[i - 1].GetPosition().X,
                    buttons[i - 1].GetPosition().Y + buttons[i - 1].GetHeight() + 8);
            }
            else
            {
                buttons[i].SetPosition(125, 368);
            }
        }
    }

    public override Scene Update()
    {
        if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
        {
            AudioManager.PlaySFXAll();
            AudioManager.ResumeBGM();
            return prevScene;
        }
        return CheckInput();
    }

    public override void Draw()
    {
        prevScene.Draw();
        canvas.Draw();
    }

    private Scene CheckInput()
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
                    if (select.PlaybackClosed) AudioManager.AddSFX(select);
                    buttonHighlight.visible = true;
                }
                
                if (MouseManager.LeftButtonReleased)
                {
                    switch (i)
                    {
                        case 0:
                            AudioManager.PlaySFXAll();
                            AudioManager.ResumeBGM();
                            return prevScene;
                        case 1:
                            TransFlicker t = new(Global.optionsMenu);
                            Global.optionsMenu.Initialize(() =>
                            {
                                Global.SaveSettings();
                                TransFlicker t = new(this);
                                t.Initialize();
                                Global.sceneManager.currScene = t;
                            });
                            t.Initialize();
                            return t;
                        case 2:
                            Global.mainMenu.Initialize();
                            TransFlicker t1 = new(Global.mainMenu);
                            t1.Initialize();
                            return t1;
                        case 3:
                            Environment.Exit(0);
                            break;
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
}
