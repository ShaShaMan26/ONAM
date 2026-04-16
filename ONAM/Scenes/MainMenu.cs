using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ONAM;

public class MainMenu : Scene
{
    private Canvas canvas;
    private Random r;
   
    private GameElement stat, flicker;
    private Texture2D[] staticFrames, flickerFrames;
    private double counter, counter2, counter3, tweakDelay;
    private int istatic;

    private SFXObject select, click;

    private TextDisplay[] buttons;
    private TextDisplay buttonHighlight;
    private Miku miku;

    public override void Initialize()
    {
        canvas = new();
        canvas.Initialize();
        r = new();
        counter = 0;
        counter2 = 0;
        counter3 = 0;
        tweakDelay = r.NextDouble() * (6 - .5) + .5;
        
        // sfx
        select = new(Global.content.Load<SoundEffect>("sfx/cam_switch"));
        click = new(Global.content.Load<SoundEffect>("sfx/cam_switch"));

        // flicker
        flickerFrames = new Texture2D[8];
        for (int i = 1; i < flickerFrames.Length; i++)
        {
            flickerFrames[i] = Global.content.Load<Texture2D>("ani_flicker/" + i);
        }
        flicker = new("ani_flicker/1");
        canvas.Add(8, flicker);
        flicker.opacity = 0;

        // static
        istatic = 0;
        staticFrames = new Texture2D[8];
        for (int i = 0; i < staticFrames.Length; i++)
        {
            staticFrames[i] = Global.content.Load<Texture2D>("ani_cam_static/" + i);
        }
        stat = new("ani_cam_static/0");
        stat.opacity = .2f;
        canvas.Add(8, stat);

        // title
        TextDisplay title = new("One*\nNight\nat\nMiku's", "consolas");
        title.SetPosition(125, 62);
        title.MapBoundsToTextSize();
        canvas.Add(9, title);

        // stars
        for (int i = 0; i < Global.userData.completion; i++)
        {
            GameElement s = new("star");
            s.SetPosition(title.GetPosition().X + s.GetWidth() * i, title.GetPosition().Y + title.GetHeight());
            canvas.Add(9, s);
        }

        // buttons
        buttonHighlight = new(">>", "consolas");
        buttonHighlight.visible = false;
        buttonHighlight.MapBoundsToTextSize();
        canvas.Add(9, buttonHighlight);

        buttons = new TextDisplay[4];
        buttons[0] = new TextDisplay("New Game", "consolas");
        buttons[1] = new TextDisplay("Continue", "consolas");
        buttons[2] = new TextDisplay("Options", "consolas");
        buttons[3] = new TextDisplay("Quit Game", "consolas");
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].MapBoundsToTextSize();
            canvas.Add(9, buttons[i]);
            if (i > 0)
            {
                buttons[i].SetPosition(buttons[i - 1].GetPosition().X,
                    buttons[i - 1].GetPosition().Y + buttons[i - 1].GetHeight() + 18);
            }
            else
            {
                buttons[i].SetPosition(125, 372);
            }
        }

        if (Global.userData.loop < 1) buttons[1].opacity = .65f;

        // miku
        miku = new("miku");
        miku.shadow = .65f;
        miku.SetDimensions(1300, 1299);
        miku.SetPosition(1020 - miku.GetWidth() / 2,
            420 - miku.GetHeight() / 2);
        canvas.Add(7, miku);

        // music
        AudioManager.LoopingBGM = true;
        AudioManager.PlayBGM(Global.content.Load<Song>("music/title"));
    }

    public override Scene Update()
    {
        UpdateAnimations();
        // debug REMOVE LATER
        if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.R))
        {
            Global.userData.SetToDefaults();
            Global.SaveUserData();
        }
        return CheckInput();
    }

    public override void Draw()
    {
        canvas.Draw();
    }

    private Scene CheckInput()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetBounds().Contains(MouseManager.Location))
            {   
                if (Global.userData.loop < 1 && i == 1) return null;

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
                    // AudioManager.AddSFX(click);
                    switch (i)
                    {
                        case 0:
                            Global.userData.SetToNewLoop();
                            Global.SaveUserData();
                            Global.difficultySelect = new();
                            TransFlicker j = new(Global.difficultySelect);
                            Global.difficultySelect.Initialize();
                            j.Initialize();
                            return j;
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

    private void UpdateAnimations()
    {
        // static
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter > .04)
        {
            stat.opacity = (float)(r.NextDouble() * (.5f - .4f) + .4f);
            stat.SetTexture(staticFrames[istatic]);
            istatic++;
            counter = 0;
            if(istatic >= staticFrames.Length)
            {
                istatic = 0;
            }
        }

        // flicker
        counter3 += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter3 > .5)
        {
            flicker.SetTexture(flickerFrames[r.Next(0, flickerFrames.Length)]);
            if (r.Next(1, 6) > 3)
            {
                flicker.opacity = (float) (r.NextDouble() * (.35 - .15) + .15);
            }
            counter3 = 0;
        }
        else if (counter3 > .1)
        {
            flicker.opacity = 0;
        }

        // miku
        counter2 += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter2 - tweakDelay > .15)
        {
            miku.SetDimensions(1300, 1299);
            miku.SetPosition(1020 - miku.GetWidth() / 2,
                420 - miku.GetHeight() / 2);
            counter2 = 0;
            tweakDelay = r.NextDouble() * (4 - .5) + .5;
        }
        else if (counter2 > tweakDelay && miku.GetWidth() == 1300)
        {
            miku.SetDimensions(r.Next(20, 50) * 100, r.Next(9, 13) * 100 + 99);
            miku.SetPosition(1020 - miku.GetWidth() / 2,
                420 - miku.GetHeight() / 2);
        }
    }
}
