using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class CamView : Canvas
{
    private Random r;
    public SFXObject cam_switch, cam_interrupt;

    public GameElement bg, camBar;
    public Texture2D[] bgTextures;
    public CamButton[] camButtons;

    private GameElement stat, flicker;
    private Texture2D[] staticFrames, flickerFrames;
    private double counter, fcounter;
    private int istatic, iflicker;

    private Miku[] camRenders;

    private void AddMikus()
    {
        camRenders = new Miku[8];

        // cam 1
        Miku e = new("miku");
        camRenders[0] = e;
        e.SetDimensions(800, 799);
        e.SetPosition(Global.graphics.PreferredBackBufferWidth / 2 - e.GetWidth() / 2,
            Global.graphics.PreferredBackBufferHeight / 2 - 40);
        Add(8, e);

        e.next = new("red-miku");
        e = e.next;
        e.SetDimensions(800, 799);
        e.SetPosition(-150,
            Global.graphics.PreferredBackBufferHeight / 2 - 120);
        Add(8, e);
        
        e.next = new("yellow-miku");
        e = e.next;
        e.SetDimensions(800, 799);
        e.SetPosition(Global.graphics.PreferredBackBufferWidth - e.GetWidth() + 150,
            Global.graphics.PreferredBackBufferHeight / 2 - 120);
        Add(8, e);

        // cam 2
        e = new("red-miku");
        camRenders[1] = e;
        e.visible = false;
        e.rotation = -0.2f;
        e.shadow = .85f;
        e.SetDimensions(1200, 1199);
        e.SetPosition(Global.graphics.PreferredBackBufferWidth - e.GetWidth() + 210,
            Global.graphics.PreferredBackBufferHeight - 490);
        Add(8, e);

        e.next = new("miku");
        e = e.next;
        e.visible = false;
        e.shadow = .955f;
        e.SetDimensions(300, 299);
        e.SetPosition(150,
            475);
        Add(8, e);
        
        e.next = new("yellow-miku");
        e = e.next;
        e.visible = false;
        e.rotation = 1.5f;
        e.shadow = .7f;
        e.SetDimensions(125, 124);
        e.SetPosition(625, 210);
        Add(8, e);
        
        // cam 3
        e = new("miku");
        camRenders[2] = e;
        e.visible = false;
        e.rotation = 0.9f;
        e.shadow = .97f;
        e.SetDimensions(140, 139);
        e.SetPosition(450,
            Global.graphics.PreferredBackBufferHeight / 2 - 50);
        Add(8, e);
        
        e.next = new("red-miku");
        e = e.next;
        e.visible = false;
        e.shadow = .9f;
        e.SetDimensions(150, 149);
        e.SetPosition(Global.graphics.PreferredBackBufferWidth / 2 - e.GetWidth() / 2 - 80,
            Global.graphics.PreferredBackBufferHeight / 2 + 70);
        Add(8, e);

        // cam 4
        e = new("yellow-miku");
        camRenders[3] = e;
        e.visible = false;
        e.rotation = 3.2f;
        e.shadow = .6f;
        e.SetDimensions(1040, 1039);
        e.SetPosition(850,
            Global.graphics.PreferredBackBufferHeight / 2 + 130);
        Add(8, e);
        
        e.next = new("miku");
        e = e.next;
        e.visible = false;
        e.shadow = .975f;
        e.SetDimensions(150, 149);
        e.SetPosition(Global.graphics.PreferredBackBufferWidth / 2 - 60,
            Global.graphics.PreferredBackBufferHeight / 2 - 30);
        Add(8, e);
    }

    public override void Initialize()
    {
        base.Initialize();
        r = new();
        cam_switch = new(Global.content.Load<SoundEffect>("sfx/cam_switch"));
        cam_interrupt = new(Global.content.Load<SoundEffect>("sfx/cam_interrupton"));
        cam_interrupt.Volume = .85f;

        counter = 0;
        fcounter = 0;
        istatic = 0;
        staticFrames = new Texture2D[8];
        for (int i = 0; i < staticFrames.Length; i++)
        {
            staticFrames[i] = Global.content.Load<Texture2D>("ani_cam_static/" + i);
        }
        stat = new("ani_cam_static/0");
        stat.opacity = .2f;

        flickerFrames = new Texture2D[9];
        for (int i = 0; i < flickerFrames.Length; i++)
        {
            flickerFrames[i] = Global.content.Load<Texture2D>("ani_flicker/" + i);
        }
        flicker = new("ani_flicker/0");
        flicker.opacity = .75f;
        iflicker = 0;


        bgTextures = new Texture2D[8];
        for (int i = 0; i < bgTextures.Length; i++)
        {
            bgTextures[i] = Global.content.Load<Texture2D>("cam" + (i + 1));
        }

        bg = new("cam1");
        bg.SetPosition(-320, 0);
        Add(7, bg);
        
        AddMikus();

        camBar = new GameElement("cam_bar");
        camBar.SetPosition(
            Global.graphics.PreferredBackBufferWidth / 2 - camBar.GetWidth() / 2, 
            Global.graphics.PreferredBackBufferHeight - camBar.GetHeight());
        Add(9, camBar);

        camButtons = new CamButton[8];
        for (int i = 0; i < camButtons.Length; i++)
        {
            camButtons[i] = new CamButton(i + 1);
            Add(9, camButtons[i]);
        }

        Add(9, new GameElement("cam_cover"));
        GameElement d = new("cam_dot");
        d.SetPosition(50, 50);
        Add(9, d);
        
        GameElement m = new ("cam_map");
        m.SetPosition(new Vector2(Global.graphics.PreferredBackBufferWidth - m.GetWidth() - 20,
            0));
        Add(9, m);

        Add(9, flicker);
        Add(9, stat);

        camButtons[0].SetPosition(m.GetPosition() + new Vector2(150, 60));
        // camButtons[0].Activate();
        camButtons[1].SetPosition(m.GetPosition() + new Vector2(150, 192));
        camButtons[2].SetPosition(m.GetPosition() + new Vector2(65, 335));
        camButtons[3].SetPosition(m.GetPosition() + new Vector2(230, 335));
        camButtons[4].SetPosition(m.GetPosition() + new Vector2(-25, 110));
        camButtons[5].SetPosition(m.GetPosition() + new Vector2(40, 165));
        camButtons[6].SetPosition(m.GetPosition() + new Vector2(315, 100));
        camButtons[7].SetPosition(m.GetPosition() + new Vector2(315, 180));

        SetToCam(1);
        AudioManager.RemoveSFX(cam_switch);
    }

    public void SetToCam(int i)
    {
        if (i == Global.camNum) return;
        bg.SetTexture(bgTextures[i - 1]);
        camButtons[Global.camNum - 1].Deactivate();
        camButtons[i - 1].Activate();
        
        RefreshCam(i);
        
        AudioManager.AddSFX(cam_switch);

        Global.camNum = i;
    }

    public void RefreshCam(int i)
    {
        CauseFlicker();
        for (Miku m = camRenders[Global.camNum - 1]; m != null; m = m.next)
        {
            m.visible = false;
        }
        for (Miku m = camRenders[i - 1]; m != null; m = m.next)
        {
            // if (Global.progress[m.id] == i) m.visible = true;
            if (Global.mikus[m.id].progress == i) m.visible = true;
        }
    }

    public void InterruptCam(int i)
    {
        RefreshCam(i);
        if (Global.stateManager.currState.GetType() == typeof(InCams)) AudioManager.AddSFX(cam_interrupt);
    }

    public void CauseFlicker()
    {
        if (flicker.visible)
        {
            iflicker = 0;
        }
        flicker.visible = true;
    }

    public void UpdateAnimations()
    {
        // static
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        fcounter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (fcounter > .017 && flicker.visible)
        {
            fcounter = 0;
            flicker.SetTexture(flickerFrames[iflicker]);
            iflicker++;
            if (iflicker >= flickerFrames.Length)
            {
                iflicker = 0;
                flicker.visible = false;
            }
        }
        if (counter > .032)
        {
            stat.opacity = (float)(r.NextDouble() * (.35f - .2f) + .2f);
            stat.SetTexture(staticFrames[istatic]);
            istatic++;
            counter = 0;
            if(istatic >= staticFrames.Length)
            {
                istatic = 0;
            }
        }
    }
}
