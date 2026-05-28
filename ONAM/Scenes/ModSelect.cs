using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class ModSelect : Scene
{
    private ModNode[] modNodes;
    private TextDisplay titleDisp, descDisp, bgTxt;
    private GameElement textBack;
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

        Random r = new();
        int id = 0, j;
        modNodes = new ModNode[3];
        for (int i = 0; i < modNodes.Length; i++)
        {
            j = 0;
            do 
            {
                if (j > Global.modifiers.Length * 2) break;
                id = r.Next(0, Global.modifiers.Length);
                j++;
            }
            while (Global.runData.activeModifiers[id]);

            if (j > Global.modifiers.Length) modNodes[i] = null;
            else
            {
                modNodes[i] = new(Global.modifiers[id], id);
                Global.runData.activeModifiers[id] = true;
            }
        }
        foreach (ModNode m in modNodes)
        {
            if (m != null) Global.runData.activeModifiers[m.id] = false;
            m.opacity = 0;
            m.borderOpacity = 0;
        }

        modNodes[0]?.SetPosition(Global.renderTarget.Width / 2 - modNodes[0].GetWidth() / 2, 
            Global.renderTarget.Height / 2 - modNodes[0].GetHeight() / 2 - 100);
        modNodes[1]?.SetPosition(modNodes[0].GetPosition() - new Vector2(modNodes[1].GetWidth() + 50, 0));
        modNodes[2]?.SetPosition(modNodes[0].GetPosition() + new Vector2(modNodes[2].GetWidth() + 50, 0));
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
                }
                if (MouseManager.LeftButtonClicked)
                {
                    Global.runData.loop++;
                    if (Global.runData.loop > 1) Global.userData.firstTime = false;
                    Global.runData.activeModifiers[selectionID] = true;
                    Global.runData.bankedTokens += Global.night.tokens;
                    Global.runData.enabledMods.Add(modNodes[i].modifier);
                    Global.runData.powerDrained += 100 - (int) (Global.night.currPower / Global.night.totalPower * 100);
                    Global.SaveUserData();
                    
                    adopted = true;
                    AudioManager.AddSFX(adopt);
                    boom.Stop();
                    AudioManager.RemoveSFX(boom);
                }
                return;
            }
        }
        selectionID = -1;
        titleDisp.visible = false;
        descDisp.visible = false;
        textBack.visible = false;
    }

    public override Scene Update()
    {
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
        textBack.Draw();
        titleDisp.Draw();
        descDisp.Draw();
    }
}
