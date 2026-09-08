using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ONAM;

public class MainMenu : Scene
{
    private Canvas canvas;
    
    private GameElement stat, flicker;
    private Texture2D[] staticFrames, flickerFrames;
    private double counter, counter2, counter3, tweakDelay;
    private int istatic;

    private MMButtonsP1 page1;
    private MMButtonsP2 page2;

    private Miku miku;

    public override void Initialize()
    {
        if (Global.customNight)
        {
            Global.runData = Global.userData.mainRun;
            Global.customNight = false;
        }

        canvas = new();
        canvas.Initialize();
        
        counter = 0;
        counter2 = 0;
        counter3 = 0;
        tweakDelay = Global.random.NextDouble() * (6 - .5) + .5;

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
            if (i + 1 <= Global.userData.completionF) s = new("star_g");
            s.SetPosition(title.GetPosition().X + s.GetWidth() * i, title.GetPosition().Y + title.GetHeight());
            canvas.Add(9, s);
        }

        page1 = new();
        page1.Initialize();
        canvas.Add(9, page1);
        page2 = new();
        page2.visible = false;
        page2.Initialize();
        canvas.Add(9, page2);

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
        if (Global.customNight)
        {
            Global.runData = Global.userData.mainRun;
            Global.customNight = false;
        }

        UpdateAnimations();
        // debug
        if (Global.devEnabled)
        {
            if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.R))
            {
                Confirm c = new(
                    this, 
                    "Reset All Data?", 
                    () => 
                    {
                        Global.ResetUserData();
                        Initialize();
                        Global.sceneManager.currScene = this;
                    }, 
                    UpdateAnimations
                );
                c.Initialize();
                return c;
            }
            if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.C))
            {
                TransFlicker t = new(Global.customSelect);
                Global.customSelect.Initialize();
                t.Initialize();
                return t;
            }
            if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.S))
            {
                TransFlicker t = new(Global.summary, true);
                Global.summary.Initialize();
                t.Initialize();
                return t;
            }
            if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.P))
            {
                TransFlicker t = new(Global.runHistory);
                Global.runHistory.Initialize();
                t.Initialize();
                return t;
            }
            if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.V))
            {
                Global.userData.completion = 3;
                Global.userData.firstTime = false;
                Global.SaveUserData();
                Global.mainMenu.Initialize();
            }
            if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.T))
            {
                AudioManager.PauseBGM();
                LoadTutorial l = new();
                l.Initialize();
                return l;
            }
            if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.E))
            {
                AudioManager.PauseBGM();
                End e = new();
                e.Initialize();
                e.OnStart();
                return e;
            }
        }
        // end debug
        
        if (page1.visible) return page1.Update();
        else return page2.Update();
    }

    public override void Draw()
    {
        canvas.Draw();
    }

    public void UpdateAnimations()
    {
        // static
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter > .04)
        {
            stat.opacity = (float)(Global.random.NextDouble() * (.5f - .4f) + .4f);
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
            flicker.SetTexture(flickerFrames[Global.random.Next(0, flickerFrames.Length)]);
            if (Global.random.Next(1, 6) > 3)
            {
                flicker.opacity = (float) (Global.random.NextDouble() * (.35 - .15) + .15);
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
            tweakDelay = Global.random.NextDouble() * (4 - .5) + .5;
        }
        else if (counter2 > tweakDelay && miku.GetWidth() == 1300)
        {
            miku.SetDimensions(Global.random.Next(20, 50) * 100, Global.random.Next(9, 13) * 100 + 99);
            miku.SetPosition(1020 - miku.GetWidth() / 2,
                420 - miku.GetHeight() / 2);
        }
    }

    public void TogglePage()
    {
        AudioManager.AddSFX(Global.clickSFX);
        page1.visible = !page1.visible;
        page2.visible = !page2.visible;
    }
}
