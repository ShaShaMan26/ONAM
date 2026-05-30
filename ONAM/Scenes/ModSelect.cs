using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class ModSelect : Scene
{
    private Random r;

    private ModNode[] modNodes, heldMods;
    private ModNode cursedMod;
    private TextDisplay titleDisp, descDisp, bgTxt, heldTxt;
    private GameElement textBack, heldBack;
    private SFXObject select;
    private int selectionID;

    private CamStatic camStatic;

    private bool startFade, adopted;
    private double counter;
    private SFXObject boom, adopt;

    public override void Initialize()
    {
        startFade = false;
        adopted = false;
        selectionID = -1;
        counter = 0;
        
        bgTxt = new("ADOPT", "fnaf-big");
        bgTxt.SetPosition(
            Global.renderTarget.Width / 2 - bgTxt.GetWidth() / 2 + 25,
            -175
        );
        bgTxt.opacity = .5f;
        bgTxt.visible = false;
        boom = new(Global.content.Load<SoundEffect>("sfx/clock_chime"));
        adopt = new(Global.content.Load<SoundEffect>("sfx/adopted"));
        adopt.Volume = .5f;

        camStatic = new(.25f, .35f);
        camStatic.Initialize();
        select = new(Global.content.Load<SoundEffect>("sfx/cam_switch"));
        titleDisp = new("", "consolas");
        titleDisp.SetPosition(0, Global.renderTarget.Height - 200);
        descDisp = new("", "fnaf");
        descDisp.SetPosition(0, Global.renderTarget.Height - 150);

        textBack = new("miku");
        textBack.SetTexture(Global.multiTexture);
        textBack.color = Color.Black;
        textBack.opacity = .75f;
        textBack.visible = false;

        r = new();
        modNodes = new ModNode[Math.Clamp(Global.modifiers.Length - Global.runData.enabledMods.Count, 1, 3)];

        List<Modifier> mods = [];
        bool found;
        foreach (Modifier m in Global.modifiers)
        {
            found = false;
            foreach(Modifier n in Global.runData.enabledMods)
            {
                if (n.title == m.title) 
                {
                    found = true;
                    break;
                }
            }
            if (!found) mods.Add(m);
        }
        Modifier[] e = [.. mods];
        for (int i = 0; i < modNodes.Length; i++)
        {
            int id = r.Next(0, e.Length);
            Modifier m = e[id];
            for (int j = 0; j < Global.modifiers.Length; j++)
            {
                if (Global.modifiers[j].title == m.title)
                {
                    modNodes[i] = new(Global.modifiers[j], j);
                    break;
                }
            }
            e[id] = null;
            e = [.. e.ToList().FindAll(x => x != null)];
        }

        foreach (ModNode m in modNodes)
        {
            m.opacity = 0;
            m.borderOpacity = 0;
        }

        modNodes[0]?.SetPosition(Global.renderTarget.Width / 2 - modNodes[0].GetWidth() / 2, 
            Global.renderTarget.Height / 2 - modNodes[0].GetHeight() / 2 - 100);
        if (modNodes.Length > 1) modNodes[1]?.SetPosition(modNodes[0].GetPosition() - new Vector2(modNodes[1].GetWidth() + 50, 0));
        if (modNodes.Length > 2) modNodes[2]?.SetPosition(modNodes[0].GetPosition() + new Vector2(modNodes[2].GetWidth() + 50, 0));

        cursedMod = null;
        if (Global.runData.loop > 1 && (!ModifierManager.shadowMiku || !ModifierManager.shadowOffice))
        {
            if (r.Next(0, 8) > 6) cursedMod = modNodes[r.Next(0, modNodes.Length)];
        }
    
        heldTxt = new("Held: ", "fnaf-small");
        heldTxt.visible = false;

        heldMods = new ModNode[Global.runData.enabledMods.Count];
        for (int i = 0; i < heldMods.Length; i++)
        {
            heldMods[i] = new(Global.runData.enabledMods[i], i + 10);
            heldMods[i].outlineOffset = 0;
            heldMods[i].outlineThickness = 0;
            heldMods[i].drawOutline = false;
            heldMods[i].SetDimensions(42, 42);
            heldMods[i].SetOutline();

            if (i == 0)
            {
                heldTxt.SetPosition(
                    Global.renderTarget.Width / 2 - (heldTxt.GetWidth() + (heldMods.Length * (heldMods[i].GetWidth() + 10)) - 10) / 2,
                    (5 + heldMods[i].GetHeight()) / 2 - heldTxt.GetHeight() / 2
                );
                heldMods[i].SetPosition(heldTxt.GetPosition().X + heldTxt.GetWidth(), 5);
            }
            else
            {
                heldMods[i].SetPosition(
                    heldMods[i - 1].GetPosition().X + heldMods[i - 1].GetWidth() + 10,
                    heldMods[i - 1].GetPosition().Y
                );
            }

            heldMods[i].visible = false;
        }
        heldBack = new("miku");
        heldBack.SetTexture(Global.multiTexture);
        heldBack.color = Color.Black;
        heldBack.opacity = .75f;
        if (heldMods.Length > 0)
        {
            heldBack.SetDimensions(
                (int) (heldTxt.GetWidth() + (heldMods.Length * (heldMods[0].GetWidth() + 10)) - 10),
                (int) heldMods[0].GetHeight()
            );
            heldBack.SetPosition(heldTxt.GetPosition().X, heldMods[0].GetPosition().Y);
        }
    }

    public override void OnStart()
    {
        startFade = true;
    }

    private void UpdateFadeIn()
    {
        ModNode m = modNodes.First(m => m.opacity < .75);
        m.opacity += (float) (1.5 * Global.gameTime.ElapsedGameTime.TotalSeconds);
        if (m.opacity > .75) m.opacity = .75f;
    }
    private Scene UpdateEnd()
    {
        if (adopt.PlaybackClosed)
        {
            Global.runData.loop++;
            if (Global.runData.loop > 1) Global.userData.firstTime = false;
            Global.SaveUserData();

            LoadNight l = new();
            l.Initialize();
            return l;
        }

        foreach (ModNode m in modNodes)
        {
            if (m.id != selectionID)
            {
                m.opacity -= (float) (.5 * Global.gameTime.ElapsedGameTime.TotalSeconds);
                if (m.opacity < 0) m.opacity = 0;
                m.borderOpacity = m.opacity;
            }
        }

        return null;
    }

    private void CheckInput()
    {
        for (int i = 0; i < modNodes.Length; i++)
        {
            if (modNodes[i] != null && modNodes[i].GetBounds().Contains(MouseManager.Location))
            {
                if (selectionID != modNodes[i].id)
                {
                    titleDisp.visible = true;
                    descDisp.visible = true;
                    textBack.visible = true;
                    if (select.PlaybackClosed) AudioManager.AddSFX(select);
                    titleDisp.Text = ">" + modNodes[i].modifier.title + "<";
                    titleDisp.MapBoundsToTextSize();
                    titleDisp.SetPosition(Global.renderTarget.Width / 2 - titleDisp.GetWidth() / 2, 
                        titleDisp.GetPosition().Y);
                    descDisp.Text = modNodes[i].modifier.desc;
                    descDisp.MapBoundsToTextSize();
                    descDisp.SetPosition(Global.renderTarget.Width / 2 - descDisp.GetWidth() / 2, 
                        descDisp.GetPosition().Y);

                    textBack.SetPosition(
                        titleDisp.GetWidth() > descDisp.GetWidth() ? titleDisp.GetPosition().X : descDisp.GetPosition().X,
                        titleDisp.GetPosition().Y
                    );
                    textBack.SetDimensions(
                        (int) (titleDisp.GetWidth() > descDisp.GetWidth() ? titleDisp.GetWidth() : descDisp.GetWidth()),
                        (int) (descDisp.GetBounds().Bottom - titleDisp.GetPosition().Y)
                    );

                    selectionID = modNodes[i].id;
                    if (cursedMod != null && selectionID == cursedMod.id) camStatic.SetRange(.35f, .45f);
                    else camStatic.SetRange(.25f, .35f);
                }

                if (MouseManager.LeftButtonClicked)
                {
                    Global.runData.activeModifiers[selectionID] = true;
                    Global.runData.enabledMods.Add(modNodes[i].modifier);
                    if (cursedMod != null && selectionID == cursedMod.id)
                    {
                        if (Global.runData.shadowModifiers[0]) Global.runData.shadowModifiers[1] = true;
                        else if (Global.runData.shadowModifiers[1]) Global.runData.shadowModifiers[0] = true;
                        else Global.runData.shadowModifiers[r.Next(0, 2)] = true;
                    }
                    
                    adopted = true;
                    AudioManager.AddSFX(adopt);
                    boom.Stop();
                    AudioManager.RemoveSFX(boom);
                }
                return;
            }
        }

        for (int i = 0; i < heldMods.Length; i++)
        {
            if (heldMods[i].GetBounds().Contains(MouseManager.Location))
            {
                if (selectionID != heldMods[i].id)
                {
                    titleDisp.visible = true;
                    descDisp.visible = true;
                    textBack.visible = true;
                    if (select.PlaybackClosed) AudioManager.AddSFX(select);
                    titleDisp.Text = ">" + heldMods[i].modifier.title + "<";
                    titleDisp.MapBoundsToTextSize();
                    titleDisp.SetPosition(Global.renderTarget.Width / 2 - titleDisp.GetWidth() / 2, 
                        titleDisp.GetPosition().Y);
                    descDisp.Text = heldMods[i].modifier.desc;
                    descDisp.MapBoundsToTextSize();
                    descDisp.SetPosition(Global.renderTarget.Width / 2 - descDisp.GetWidth() / 2, 
                        descDisp.GetPosition().Y);

                    textBack.SetPosition(
                        titleDisp.GetWidth() > descDisp.GetWidth() ? titleDisp.GetPosition().X : descDisp.GetPosition().X,
                        titleDisp.GetPosition().Y
                    );
                    textBack.SetDimensions(
                        (int) (titleDisp.GetWidth() > descDisp.GetWidth() ? titleDisp.GetWidth() : descDisp.GetWidth()),
                        (int) (descDisp.GetBounds().Bottom - titleDisp.GetPosition().Y)
                    );

                    selectionID = heldMods[i].id;
                }
                return;
            }
        }

        selectionID = -1;
        titleDisp.visible = false;
        descDisp.visible = false;
        textBack.visible = false;
        camStatic.SetRange(.25f, .35f);
    }

    public override Scene Update()
    {
        // check skip
        if (!modNodes.Any(m => m.opacity == 1)
            && !Global.userData.firstTime 
            && (MouseManager.RightButtonReleased 
                || MouseManager.LeftButtonReleased 
                || KeyboardManager.PressedKeys.Count != 0
        ))
        {
            counter = 100;
            foreach (ModNode m in modNodes)
            {
                m.opacity = 1;
                m.borderOpacity = 1;
            }
            bgTxt.visible = true;
            if (heldMods.Length > 0)
            {
                heldTxt.visible = true;
                foreach (ModNode m in heldMods) m.visible = true;
            }
            AudioManager.AddSFX(boom);
            return null;
        }

        camStatic.Update();
        if (adopted)
        {
            return UpdateEnd();
        }
        else if (modNodes[^1].opacity < .75)
        {
            if (startFade) UpdateFadeIn();
            return null;
        }
        else if (counter < 1)
        {
            counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (counter >= 1)
            {
                foreach (ModNode m in modNodes)
                {
                    m.opacity = 1;
                    m.borderOpacity = 1;
                }
                bgTxt.visible = true;
                if (heldMods.Length > 0)
                {
                    heldTxt.visible = true;
                    foreach (ModNode m in heldMods) m.visible = true;
                }
                AudioManager.AddSFX(boom);
            }
            return null;
        }
        else if (!adopted)
        {
            CheckInput();
            return null;
        }
        return null;
    }

    public override void Draw()
    {
        bgTxt.Draw();
        if (bgTxt.visible) camStatic.Draw();
        foreach (ModNode m in modNodes) m?.Draw();
        if (!bgTxt.visible) camStatic.Draw();

        if (heldTxt.visible) heldBack.Draw();
        heldTxt.Draw();
        foreach (ModNode m in heldMods) m?.Draw();

        textBack.Draw();
        titleDisp.Draw();
        descDisp.Draw();
    }
}
