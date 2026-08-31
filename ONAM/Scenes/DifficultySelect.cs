using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class DifficultySelect: Scene
{
    private CamStatic camStatic;

    private TextDisplay tip, title;

    private SFXObject select;
    private TextDisplay[] buttons;
    private TextDisplay buttonHighlight;

    private bool skipTutorial;

    public override void Initialize()
    {
        skipTutorial = false;
        select = new(Global.content.Load<SoundEffect>("sfx/cam_switch"));

        camStatic = new(.35f, .45f);
        camStatic.Initialize();

        tip = new("", "fnaf");
        tip.visible = false;
        tip.SetPosition(0, Global.renderTarget.Height - 200);

        buttonHighlight = new(">>", "consolas");
        buttonHighlight.visible = false;
        buttonHighlight.MapBoundsToTextSize();

        title = new("Select Difficulty:", "consolas");
        title.SetPosition(Global.renderTarget.Width / 2 - title.GetWidth() / 2,
            150);

        buttons = new TextDisplay[3];
        buttons[0] = new TextDisplay("Easy", "consolas");
        buttons[1] = new TextDisplay("Medium", "consolas");
        buttons[2] = new TextDisplay("Hard", "consolas");
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].MapBoundsToTextSize();
            if (i > 0)
            {
                buttons[i].SetPosition(Global.renderTarget.Width / 2 - buttons[i].GetWidth() / 2,
                    buttons[i - 1].GetPosition().Y + buttons[i - 1].GetHeight() + 14);
            }
            else
            {
                buttons[i].SetPosition(Global.renderTarget.Width / 2 - buttons[i].GetWidth() / 2,
                    260);
            }
        }
    }

    private void SetTip(int i)
    {
        if (i == 0) tip.Text = "For those unfamiliar with Five Nights at Freddy's style experiences.";
        else if (i == 1) tip.Text = "For those with at least a passing knowledge of FNaF-style games.";
        else if (i == 2) tip.Text = "For those looking for a challenge...";

        tip.MapBoundsToTextSize();
        tip.SetPosition(Global.renderTarget.Width / 2 - tip.GetWidth() / 2, tip.GetPosition().Y);
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
                    tip.visible = true;
                    SetTip(i);
                }
                
                if (MouseManager.LeftButtonReleased)
                {
                    Global.runData.SetToNewLoop();
                    switch (i)
                    {
                        case 0:
                            Global.runData.SetDifficulty("easy");
                            break;
                        case 1:
                            Global.runData.SetDifficulty("medium");
                            break;
                        case 2:
                            Global.runData.SetDifficulty("hard");
                            break;
                    }
                    Global.SaveUserData();

                    if (Global.userData.firstTime)
                    {
                        AudioManager.PauseBGM();
                        LoadTutorial t = new();
                        t.Initialize();
                        return t;
                    }
                    else
                    {
                        skipTutorial = true;
                        Confirm c = new(
                            this,
                            "Replay Tutorial?",
                            "(Never hurts to go over the basics!)",
                            () =>
                            {
                                AudioManager.PauseBGM();
                                LoadTutorial t = new();
                                t.Initialize();
                                Global.sceneManager.currScene = t;
                            },
                            camStatic.Update
                        );
                        c.Initialize();
                        return c;
                    }
                }
                return null;
            }
        }
        if (buttonHighlight.visible)
        {
            buttonHighlight.visible = false;
            tip.visible = false;
            buttonHighlight.SetPosition(0, 0);
        }
        return null;
    }

    public override Scene Update()
    {
                    
        if (skipTutorial)
        {
            AudioManager.PauseBGM();
            Global.loadNight = new();
            Global.loadNight.Initialize();
            return Global.loadNight;
        }

        camStatic.Update();
        if (KeyboardManager.KeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
        {
            TransFlicker t = new(Global.mainMenu);
            t.Initialize();
            Global.sceneManager.currScene = t;
            return t;
        }
        return CheckInput();
    }

    public override void Draw()
    {
        camStatic.Draw();
        title.Draw();
        buttonHighlight.Draw();
        tip.Draw();
        foreach (TextDisplay t in buttons)
        {
            t.Draw();
        }
    }
}
