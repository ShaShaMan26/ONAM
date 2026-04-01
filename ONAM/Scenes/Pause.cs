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
    private TextDisplay buttonHighlightR, buttonHighlightL;

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

        buttonHighlightR = new(">", "consolas");
        buttonHighlightR.visible = false;
        buttonHighlightR.MapBoundsToTextSize();
        canvas.Add(9, buttonHighlightR);
        buttonHighlightL = new("<", "consolas");
        buttonHighlightL.visible = false;
        buttonHighlightL.MapBoundsToTextSize();
        canvas.Add(9, buttonHighlightL);

        buttons = new TextDisplay[3];
        buttons[0] = new TextDisplay("Resume", "consolas");
        buttons[1] = new TextDisplay("Return to Title", "consolas");
        buttons[2] = new TextDisplay("Quit Game", "consolas");
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].MapBoundsToTextSize();
            canvas.Add(9, buttons[i]);
            if (i > 0)
            {
                buttons[i].SetPosition(buttons[i - 1].GetPosition().X,
                    buttons[i - 1].GetPosition().Y + buttons[i - 1].GetHeight() + 26);
            }
            else
            {
                buttons[i].SetPosition(125, 373);
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
                if (buttonHighlightR.GetPosition().Y != buttons[i].GetPosition().Y)
                {
                    buttonHighlightR.SetPosition(
                        buttons[i].GetPosition().X - buttonHighlightR.GetWidth() - 4,
                        buttons[i].GetPosition().Y);
                    buttonHighlightL.SetPosition(
                        buttons[i].GetPosition().X + buttons[i].GetWidth() 
                            + buttonHighlightL.GetWidth() - 20,
                        buttons[i].GetPosition().Y);
                    if (select.PlaybackClosed) AudioManager.AddSFX(select);
                    buttonHighlightR.visible = true;
                    buttonHighlightL.visible = true;
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
                            Global.mainMenu.Initialize();
                            return Global.mainMenu;
                        case 2:
                            Environment.Exit(0);
                            break;
                    }
                }
                return null;
            }
        }
        if (buttonHighlightR.visible)
        {
            buttonHighlightR.visible = false;
            buttonHighlightL.visible = false;
            buttonHighlightR.SetPosition(0, 0);
        }
        return null;
    }
}
