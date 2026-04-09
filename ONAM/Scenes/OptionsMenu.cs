using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class OptionsMenu : Scene
{
    private Canvas canvas;

    private GameElement stat;
    private Texture2D[] staticFrames;
    private double counter;
    private int istatic;
    private Random r;
    
    private SettingsButton[] buttons;
    private TextDisplay back;
    private SFXObject select, click;

    private Action onBackClick;

    public void Initialize(Action a)
    {
        onBackClick = a;

        canvas = new();
        canvas.Initialize();

        buttons = new SettingsButton[4];
        MakeButtons();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i]?.Initialize();
            if (buttons[i] == null) return;
            canvas.Add(buttons[i]);
            if (i == 0)
            {
                buttons[i].SetPosition(new(775, 150));
            }
            else
            {
                buttons[i].SetPosition(new(buttons[i - 1].GetPosition().X,
                    buttons[i - 1].GetPosition().Y + buttons[i - 1].GetHeight() + 50));
            }
        }
        TextDisplay t = new("Display Mode", "consolas");
        t.SetPosition(110, buttons[0].GetPosition().Y);
        canvas.Add(t);
        t = new("Frame Rate", "consolas");
        t.SetPosition(110, buttons[1].GetPosition().Y);
        canvas.Add(t);
        t = new("Music Volume", "consolas");
        t.SetPosition(110, buttons[2].GetPosition().Y);
        canvas.Add(t);
        t = new("Sound Effect Volume", "consolas");
        t.SetPosition(110, buttons[3].GetPosition().Y);
        canvas.Add(t);

        r = new();
        counter = 0;
        istatic = 0;
        staticFrames = new Texture2D[8];
        for (int i = 0; i < staticFrames.Length; i++)
        {
            staticFrames[i] = Global.content.Load<Texture2D>("ani_cam_static/" + i);
        }
        stat = new("ani_cam_static/0");
        stat.opacity = .2f;
        canvas.Add(1, stat);

        back = new("Back", "consolas");
        back.MapBoundsToTextSize();
        back.SetPosition(Global.renderTarget.Width / 2 - back.GetWidth() / 2, 
            Global.renderTarget.Height - back.GetHeight() * 1.5f);
        canvas.Add(back);

        select = new(Global.content.Load<SoundEffect>("sfx/cam_switch"));
        click = new(Global.content.Load<SoundEffect>("sfx/vent_beep"));

        TextDisplay e = new("OPTIONS", "fnaf-big");
        e.opacity = .55f;
        e.SetPosition(-208, -117);
        canvas.Add(0, e);
    }

    public override Scene Update()
    {
        if (back.GetBounds().Contains(MouseManager.Location))
        {
            if (back.Text != "> Back <")
            {
                if (select.PlaybackClosed) AudioManager.AddSFX(select);
                back.Text = "> Back <";
                back.MapBoundsToTextSize();
                back.SetPosition(Global.renderTarget.Width / 2 - back.GetWidth() / 2, 
                    Global.renderTarget.Height - back.GetHeight() * 1.5f);
            }
            if (MouseManager.LeftButtonPressed)
            {
                onBackClick.Invoke();
            }
        }
        else if (back.Text != "Back")
        {
            back.Text = "Back";
            back.MapBoundsToTextSize();
            back.SetPosition(Global.renderTarget.Width / 2 - back.GetWidth() / 2, 
                Global.renderTarget.Height - back.GetHeight() * 1.5f);
        }

        foreach (SettingsButton b in buttons)
        {
            if (b.Update()) AudioManager.PlaySFX(click);
        }

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

        return null;
    }

    public override void Draw()
    {
        canvas.Draw();
    }

    private void MakeButtons()
    {
        buttons[0] = new("Borderlessss", 
        () =>
        {
            Global.settings.displayMode++;
            if (Global.settings.displayMode > 2)
                Global.settings.displayMode = 0;
            Global.settings.SetDisplayMode(Global.settings.displayMode);
        }, 
        () =>
        {
            Global.settings.displayMode--;
            if (Global.settings.displayMode < 0)
                Global.settings.displayMode = 2;
            Global.settings.SetDisplayMode(Global.settings.displayMode);
        },
        () =>
        {
            switch (Global.settings.displayMode)
            {
                case 0:
                    buttons[0].title.Text = "Windowed";
                    break;
                case 1:
                    buttons[0].title.Text = "Borderless";
                    break;
                case 2:
                    buttons[0].title.Text = "Fullscreen";
                    break;
            }
        });

        buttons[1] = new("Borderlessss", 
        () =>
        {
            Global.settings.refreshRate--;
            if (Global.settings.refreshRate < 0)
                Global.settings.refreshRate = 6;
            Global.settings.SetRefreshRate(Global.settings.refreshRate);
        }, 
        () =>
        {
            Global.settings.refreshRate++;
            if (Global.settings.refreshRate > 6)
                Global.settings.refreshRate = 0;
            Global.settings.SetRefreshRate(Global.settings.refreshRate);
        },
        () =>
        {
            if (Global.settings.refreshRate == 0)
            {
                buttons[1].title.Text = "Unlimited";
            }
            else if (Global.settings.refreshRate == 1)
            {
                buttons[1].title.Text = "30";
            }
            else if (Global.settings.refreshRate == 2)
            {
                buttons[1].title.Text = "60";
            }
            else if (Global.settings.refreshRate == 3)
            {
                buttons[1].title.Text = "90";
            }
            else if (Global.settings.refreshRate == 4)
            {
                buttons[1].title.Text = "120";
            }
            else if (Global.settings.refreshRate == 5)
            {
                buttons[1].title.Text = "144";
            }
            else if (Global.settings.refreshRate == 6)
            {
                buttons[1].title.Text = "240";
            }
        });
        
        buttons[2] = new("Borderlessss", 
        () =>
        {
            Global.settings.musicVolume -= 0.05;
            if (Global.settings.musicVolume < 0)
                Global.settings.musicVolume = 0;
            Global.settings.SetMusicVolume(Global.settings.musicVolume);
        }, 
        () =>
        {
            Global.settings.musicVolume += 0.05;
            if (Global.settings.musicVolume > 1)
                Global.settings.musicVolume = 1;
            Global.settings.SetMusicVolume(Global.settings.musicVolume);
        },
        () =>
        {
            if (Global.settings.musicVolume < 0.04)
            {
                buttons[2].title.Text = "OFF";
            }
            else if (Global.settings.musicVolume == 1)
            {
                buttons[2].title.Text = "MAX";
            }
            else
            {
                buttons[2].title.Text = (Global.settings.musicVolume * 100).ToString("F0") + "%";
            }
        });
        
        buttons[3] = new("Borderlessss", 
        () =>
        {
            Global.settings.sfxVolume -= 0.05;
            if (Global.settings.sfxVolume < 0)
                Global.settings.sfxVolume = 0;
            Global.settings.SetSFXVolume(Global.settings.sfxVolume);
        }, 
        () =>
        {
            Global.settings.sfxVolume += 0.05;
            if (Global.settings.sfxVolume > 1)
                Global.settings.sfxVolume = 1;
            Global.settings.SetSFXVolume(Global.settings.sfxVolume);
        },
        () =>
        {
            if (Global.settings.sfxVolume < 0.04)
            {
                buttons[3].title.Text = "OFF";
            }
            else if (Global.settings.sfxVolume == 1)
            {
                buttons[3].title.Text = "MAX";
            }
            else
            {
                buttons[3].title.Text = (Global.settings.sfxVolume * 100).ToString("F0") + "%";
            }
        });
    }
}
