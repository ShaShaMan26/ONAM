using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class CamView : Canvas
{
    private Random r;
    public SFXObject cam_switch, cam_interrupt;

    public GameElement bg, bgVent, camBar, seal_vent_bar, seal_vent_bar_active, seal_vent_dots;
    public Texture2D[] bgTextures, bgVentTextures;
    public CamButton[] camButtons;
    public VentLock[] ventLocks;

    private GameElement stat, flicker, smSprite;
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
        e.SetPosition(Global.renderTarget.Width / 2 - e.GetWidth() / 2,
            Global.renderTarget.Height / 2 - 40);
        Add(8, e);

        e.next = new("red-miku");
        e = e.next;
        e.SetDimensions(800, 799);
        e.SetPosition(-150,
            Global.renderTarget.Height / 2 - 120);
        Add(8, e);
        
        e.next = new("yellow-miku");
        e = e.next;
        e.SetDimensions(800, 799);
        e.SetPosition(Global.renderTarget.Width - e.GetWidth() + 150,
            Global.renderTarget.Height / 2 - 120);
        Add(8, e);

        // cam 2
        e = new("red-miku");
        camRenders[1] = e;
        e.visible = false;
        e.rotation = -0.2f;
        e.shadow = .85f;
        e.SetDimensions(1200, 1199);
        e.SetPosition(Global.renderTarget.Width - e.GetWidth() + 210,
            Global.renderTarget.Height - 490);
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

        e.next = new("green-miku");
        e = e.next;
        e.visible = false;
        e.rotation = 3.15f;
        e.shadow = .72f;
        e.SetDimensions(600, 599);
        e.SetPosition(500,
            275);
        Add(8, e);
        
        // cam 3
        e = new("miku");
        camRenders[2] = e;
        e.visible = false;
        e.rotation = 0.9f;
        e.shadow = .97f;
        e.SetDimensions(140, 139);
        e.SetPosition(450,
            Global.renderTarget.Height / 2 - 50);
        Add(8, e);
        
        e.next = new("red-miku");
        e = e.next;
        e.visible = false;
        e.shadow = .9f;
        e.SetDimensions(150, 149);
        e.SetPosition(Global.renderTarget.Width / 2 - e.GetWidth() / 2 - 80,
            Global.renderTarget.Height / 2 + 70);
        Add(8, e);

        // cam 4
        e = new("yellow-miku");
        camRenders[3] = e;
        e.visible = false;
        e.rotation = 3.2f;
        e.shadow = .6f;
        e.SetDimensions(1040, 1039);
        e.SetPosition(850,
            Global.renderTarget.Height / 2 + 130);
        Add(8, e);
        
        e.next = new("miku");
        e = e.next;
        e.visible = false;
        e.shadow = .975f;
        e.SetDimensions(150, 149);
        e.SetPosition(Global.renderTarget.Width / 2 - 60,
            Global.renderTarget.Height / 2 - 30);
        Add(8, e);

        // cam 5
        e = new("green-miku");
        camRenders[4] = e;
        e.visible = false;
        e.shadow = .74f;
        e.SetDimensions(130, 129);
        e.SetPosition(640, 220);
        Add(8, e);

        // cam 6
        e = new("green-miku");
        camRenders[5] = e;
        e.visible = false;
        e.rotation = 3.2f;
        e.shadow = .75f;
        e.SetDimensions(320, 319);
        e.SetPosition(400, 215);
        Add(8, e);

        // cam 7
        e = new("green-miku");
        camRenders[6] = e;
        e.visible = false;
        e.rotation = .8f;
        e.shadow = .8f;
        e.SetDimensions(200, 199);
        e.SetPosition(25, 100);
        Add(8, e);

        // cam 8
        e = new("green-miku");
        camRenders[7] = e;
        e.visible = false;
        e.shadow = .85f;
        e.SetDimensions(1200, 1199);
        e.SetPosition(Global.renderTarget.Width / 2 - e.GetWidth() / 2,
            -25);
        Add(8, e);
    }

    public override void Initialize()
    {
        base.Initialize();
        r = new();
        cam_switch = new(Global.content.Load<SoundEffect>("sfx/cam_switch"));
        cam_interrupt = new(Global.content.Load<SoundEffect>("sfx/cam_interrupton"));

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

        bgVentTextures = new Texture2D[8];
        for (int i = 0; i < bgVentTextures.Length; i++)
        {
            if (i > 3 && i < 8)
                bgVentTextures[i] = Global.content.Load<Texture2D>("cam" + (i + 1) + "vo");
        }

        bgVent = new("cam1");
        bgVent.visible = false;
        bgVent.SetPosition(-320, 0);
        Add(7, bgVent);

        bgTextures = new Texture2D[8];
        for (int i = 0; i < bgTextures.Length; i++)
        {
            bgTextures[i] = Global.content.Load<Texture2D>("cam" + (i + 1));
        }

        bg = new("cam1");
        bg.SetPosition(-320, 0);
        Add(7, bg);
        
        AddMikus();
        if (Global.userData.clearCams)
        {
            foreach (Miku c in camRenders)
            {
                for (Miku b = c; b != null; b = b.next)
                {
                    b.shadow -= .05f;
                }
            }
        }

        camBar = new GameElement("cam_bar");
        camBar.SetPosition(
            Global.renderTarget.Width / 2 - camBar.GetWidth() / 2, 
            Global.renderTarget.Height - camBar.GetHeight());
        Add(9, camBar);

        seal_vent_bar_active = new("seal_vent_bar_active");
        seal_vent_bar_active.SetPosition(
            Global.renderTarget.Width / 2 - seal_vent_bar_active.GetWidth() / 2, 
            10);
        Add(9, seal_vent_bar_active);
        seal_vent_bar_active.visible = false;

        seal_vent_bar = new("seal_vent_bar");
        seal_vent_bar.SetPosition(
            Global.renderTarget.Width / 2 - seal_vent_bar.GetWidth() / 2, 
            10);
        Add(9, seal_vent_bar);
        seal_vent_bar.visible = false;
        
        seal_vent_dots = new("ani_vent_seal/0");
        seal_vent_dots.SetPosition(seal_vent_bar.GetPosition() + new Vector2(0, seal_vent_bar.GetHeight()));
        Add(9, seal_vent_dots);
        seal_vent_dots.visible = false;

        camButtons = new CamButton[8];
        for (int i = 0; i < camButtons.Length; i++)
        {
            camButtons[i] = new CamButton(i + 1);
            Add(9, camButtons[i]);
        }

        ventLocks = new VentLock[4];
        for (int i = 0; i < ventLocks.Length; i++)
        {
            ventLocks[i] = new();
            Add(9, ventLocks[i]);
        }

        Add(9, new GameElement("cam_cover"));
        GameElement d = new("cam_dot");
        d.SetPosition(50, 50);
        Add(9, d);
        
        GameElement m = new ("cam_map");
        m.SetPosition(new Vector2(Global.renderTarget.Width - m.GetWidth() - 20,
            0));
        Add(9, m);

        Add(9, flicker);
        Add(9, stat);

        smSprite = new("shadow");
        smSprite.opacity = .4f;
        smSprite.SetDimensions(1350, 1249);
        smSprite.SetPosition(Global.renderTarget.Width / 2 - smSprite.GetWidth() / 2,
            Global.renderTarget.Height / 2 - smSprite.GetHeight() / 2);
        smSprite.visible = false;
        Add(9, smSprite);


        camButtons[0].SetPosition(m.GetPosition() + new Vector2(150, 60));
        camButtons[1].SetPosition(m.GetPosition() + new Vector2(150, 192));
        camButtons[2].SetPosition(m.GetPosition() + new Vector2(65, 330));
        camButtons[3].SetPosition(m.GetPosition() + new Vector2(230, 330));
        camButtons[4].SetPosition(m.GetPosition() + new Vector2(-25, 100));
        camButtons[5].SetPosition(m.GetPosition() + new Vector2(40, 165));
        camButtons[6].SetPosition(m.GetPosition() + new Vector2(315, 100));
        camButtons[7].SetPosition(m.GetPosition() + new Vector2(315, 180));

        ventLocks[0].SetPosition(m.GetPosition() + new Vector2(-14, 149));
        ventLocks[1].SetPosition(m.GetPosition() + new Vector2(14, 223));
        ventLocks[2].SetPosition(m.GetPosition() + new Vector2(372, 167));
        ventLocks[3].SetPosition(m.GetPosition() + new Vector2(348, 234));

        SetToCam(1);
        AudioManager.RemoveSFX(cam_switch);
    }

    public void SetToCam(int i)
    {
        if (i == Global.night.camNum) return;
        bg.SetTexture(bgTextures[i - 1]);
        bgVent.SetTexture(bgVentTextures[i - 1]);
        camButtons[Global.night.camNum - 1].Deactivate();
        camButtons[i - 1].Activate();
        
        RefreshCam(i);

        AudioManager.AddSFX(cam_switch);

        Global.night.camNum = i;

        seal_vent_bar.visible = Global.night.camNum > 4;
        seal_vent_bar_active.visible = Global.night.camNum == Global.night.sealedVentNum + 5;
    }

    public void SealVent(int i)
    {
        if (i == Global.night.sealedVentNum) return;

        if (Global.night.sealedVentNum >= 0 && Global.night.sealedVentNum < ventLocks.Length) ventLocks[Global.night.sealedVentNum].Toggle();
        ventLocks[i].Toggle();

        Global.night.sealedVentNum = i;
    }

    public void RefreshCam(int i)
    {
        CauseFlicker();
        bgVent.visible = Global.night.mikus[3].attacking 
            && ((GreenMiku) Global.night.mikus[3]).prevProg == i;
        for (Miku m = camRenders[Global.night.camNum - 1]; m != null; m = m.next)
        {
            m.visible = false;
        }
        if (Global.userData.hallucinateGreen && i > 4 && Global.night.mikus[3].progress != i 
            && !bgVent.visible && r.Next(0, 6) > 4)
        {
            for (Miku m = camRenders[i - 1]; m != null; m = m.next)
            {
                m.visible = true;
                m.opacity = .25f;
            }
        }
        else
        {
            for (Miku m = camRenders[i - 1]; m != null; m = m.next)
            {
                if (Global.night.mikus[m.id].progress == i) m.visible = true;
                m.opacity = 1;
                // m.visible = true;
            }
        }
    }

    public void InterruptCam(int i)
    {
        RefreshCam(i);
        if (Global.night.stateManager.currState.GetType() == typeof(InCams)) AudioManager.AddSFX(cam_interrupt);
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
        if (Global.userData.shadowMiku) smSprite.visible = Global.night.camNum == Global.night.shadowMiku.progress;
        // static
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        fcounter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (fcounter > Global.aniDelay && flicker.visible)
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
        if (counter > Global.aniDelay)
        {
            stat.opacity = (float)(r.NextDouble() * (.35f - .2f) + .2f)
                - (Global.userData.clearCams ? .1f : 0)
                + (Global.userData.shadowMiku && Global.night.shadowMiku.progress == Global.night.camNum ? (float) (.6f * Global.night.shadowMiku.fadeProg) : 0);
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
