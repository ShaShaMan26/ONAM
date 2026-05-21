using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class Pause : Scene
{
    private Scene prevScene;
    private bool bgmWasPlaying;

    private Canvas canvas;
    private GameElement cover, scanline;
    private CamStatic camStatic;

    private SFXObject select;
    private TextDisplay[] buttons;
    private TextDisplay buttonHighlight;

    public Pause(Scene scene)
    {
        prevScene = scene;
    }

    public override void OnStart()
    {
        bgmWasPlaying = AudioManager.PlayingBGM;
        AudioManager.PauseSFXAll();
        AudioManager.PauseBGM();
    }

    public override void Initialize()
    {
        canvas = new();
        canvas.Initialize();

        cover = new("ani_flicker/0");
        cover.color = Color.Black;
        cover.opacity = .7f;
        canvas.Add(6, cover);

        scanline = new("scanline");
        scanline.opacity = .08f;
        canvas.Add(7, scanline);

        camStatic = new(.2f, .3f);
        camStatic.Initialize();
        canvas.Add(7, camStatic);

        // buttons
        select = new(Global.content.Load<SoundEffect>("sfx/cam_switch"));

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
            if (bgmWasPlaying) AudioManager.ResumeBGM();
            return prevScene;
        }
        scanline.SetPosition(0, scanline.GetPosition().Y + (float) (38 * Global.gameTime.ElapsedGameTime.TotalSeconds));
        if (scanline.GetPosition().Y >= Global.renderTarget.Height) scanline.SetPosition(0, -scanline.GetHeight());
        camStatic.Update();
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
                            if (bgmWasPlaying) AudioManager.ResumeBGM();
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
                            Confirm c = new(
                                this, 
                                "Return to Main Menu?",
                                "(Current progress will be lost).",
                                () => 
                                {
                                    Global.mainMenu.Initialize();
                                    TransFlicker t1 = new(Global.mainMenu);
                                    t1.Initialize();
                                    Global.sceneManager.currScene = t1;
                                }, 
                                () =>
                                {
                                    scanline.SetPosition(0, scanline.GetPosition().Y + (float) (38 * Global.gameTime.ElapsedGameTime.TotalSeconds));
                                    if (scanline.GetPosition().Y >= Global.renderTarget.Height) scanline.SetPosition(0, -scanline.GetHeight());
                                    camStatic.Update();
                                }
                            );
                            c.Initialize();
                            return c;
                        case 3:
                            Confirm b = new(
                                this, 
                                "Quit Game?", 
                                "(Current progress will be lost).", 
                                () => Environment.Exit(0),
                                () =>
                                {
                                    scanline.SetPosition(0, scanline.GetPosition().Y + (float) (38 * Global.gameTime.ElapsedGameTime.TotalSeconds));
                                    if (scanline.GetPosition().Y >= Global.renderTarget.Height) scanline.SetPosition(0, -scanline.GetHeight());
                                    camStatic.Update();
                                }
                            );
                            b.Initialize();
                            return b;
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
