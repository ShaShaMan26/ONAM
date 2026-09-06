using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class Summary : Scene
{
    private double counter;

    private Canvas canvas;
    private CamStatic camStatic;
    private TextDisplay[] stats;
    private TextDisplay modsUsed, bgTxt, back, cont;
    private ModNode[] modNodes;
    private SFXObject select, boom, yay;

    public override void Initialize()
    {
        counter = -10000000;

        select = new(Global.content.Load<SoundEffect>("sfx/cam_switch"));
        yay = new(Global.content.Load<SoundEffect>("sfx/yay"));
        boom = new(Global.content.Load<SoundEffect>("sfx/clock_chime"));
        
        canvas = new();
        canvas.Initialize();

        bgTxt = new("STATS", "fnaf-big");
        bgTxt.opacity = .25f;
        bgTxt.SetPosition(
            Global.renderTarget.Width / 2 - bgTxt.GetWidth() / 2 + 24, 
            -120
        );
        canvas.Add(4, bgTxt);

        camStatic = new(.25f, .35f);
        camStatic.Initialize();
        canvas.Add(5, camStatic);

        TextDisplay congText = new("Congrats", "fnaf-slight-big");
        if (Global.runData.loop > 5 
            && Global.runData.activeModifiers.Count(c => c) == Global.modifiers.Length
            && Global.runData.shadowModifiers.Count(c => c) == Global.runData.shadowModifiers.Length) congText.Text = "You truly did it";
        else if (Global.runData.loop > 5 && Global.runData.activeModifiers.Count(c => c) == Global.modifiers.Length) congText.Text = "You really did it";
        else if (Global.runData.loop > 5) congText.Text = "Way to go";
        for (int i = 0; i < Global.runData.difficultyManager.id; i++) congText.Text += "!";
        congText.MapBoundsToTextSize();
        congText.SetPosition(
                Global.renderTarget.Width / 2 - congText.GetWidth() / 2,
                15
            );
        canvas.Add(7, congText);

        if (Global.runData.loop > 5)
        {
            stats = [
                new("Mikus Deterred: " + Global.runData.mikusDeterred, "fnaf"),
                new("Mikulings Calmed: " + Global.runData.mikulingsCalmed, "fnaf"),
                new("Doors Closed: " + Global.runData.doorsClosed, "fnaf"),
                new("Vents Sealed: " + Global.runData.ventsSealed, "fnaf"),
                new("Power Drained: " + Global.runData.powerDrained + "%", "fnaf"),
                new("Deaths: " + Global.runData.deaths, "fnaf"),
                new("Hours Survived: " + Global.runData.hours, "fnaf"),
                new("Furthest Loop: " + Global.runData.loop, "fnaf")
            ];
        }
        else
        {
            stats = [
                new("Mikus Deterred: " + Global.runData.mikusDeterred, "fnaf"),
                new("Mikulings Calmed: " + Global.runData.mikulingsCalmed, "fnaf"),
                new("Doors Closed: " + Global.runData.doorsClosed, "fnaf"),
                new("Vents Sealed: " + Global.runData.ventsSealed, "fnaf"),
                new("Power Drained: " + Global.runData.powerDrained + "%", "fnaf"),
                new("Deaths: " + Global.runData.deaths, "fnaf"),
            ];
        }
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i].SetPosition(
                Global.renderTarget.Width / 2 - stats[i].GetWidth() / 2,
                i == 0 ? 90 : stats[i - 1].GetBounds().Bottom - 5
            );
            stats[i].visible = false;
            canvas.Add(7, stats[i]);
        }

        modsUsed = new("Mods Used:", "fnaf");
        modsUsed.SetPosition(
            Global.renderTarget.Width / 2 - modsUsed.GetWidth() / 2,
            stats[^1].GetBounds().Bottom + 15
        );
        modsUsed.visible = false;
        canvas.Add(7, modsUsed);

        modNodes = new ModNode[Global.runData.enabledMods.Count + Global.runData.shadowModifiers.Count(m => m)];
        for (int i = 0; i < modNodes.Length; i++)
        {
            if (i < Global.runData.enabledMods.Count)
            {
                modNodes[i] = new(Global.runData.enabledMods[i], 0);
            }
            else
            {
                Modifier m = new();
                m.title = "";
                m.desc = "";
                if (i == Global.runData.enabledMods.Count && ModifierManager.shadowMiku)
                {
                    m.iconPath = "mod_icons/shadow-cams";
                }
                else
                {
                    m.iconPath = "mod_icons/shadow-office";
                }
                modNodes[i] = new(m, 0);
            }
            modNodes[i].outlineOffset = 0;
            modNodes[i].outlineThickness = 0;
            modNodes[i].drawOutline = false;
            modNodes[i].SetDimensions(72, 72);
            modNodes[i].SetOutline();

            if (i == 0) modNodes[i].SetPosition(
                i == 0 ? 
                    (Global.renderTarget.Width - (modNodes.Length * modNodes[0].GetWidth() + (modNodes.Length - 1) * 10)) / 2 
                    : modNodes[i - 1].GetBounds().Right + 10,
                modsUsed.GetBounds().Bottom + 5
            );
            else modNodes[i].SetPosition(
                modNodes[i - 1].GetBounds().Right + 8,
                modNodes[i - 1].GetPosition().Y
            );
            modNodes[i].visible = false;
            canvas.Add(7, modNodes[i]);
        }

        back = new("End Run", "consolas");
        back.MapBoundsToTextSize();
        back.visible = false;
        canvas.Add(7, back);
        cont = null;
        if (Global.runData.loop == 5)
        {
            cont = new("Continue", "consolas");
            cont.MapBoundsToTextSize();
            cont.SetPosition(Global.renderTarget.Width / 2 - cont.GetWidth() / 2 + 5 - 150, 
                Global.renderTarget.Height - cont.GetHeight() * 1.75f);
            cont.visible = false;
            canvas.Add(7, cont);

            back.SetPosition(Global.renderTarget.Width / 2 - back.GetWidth() / 2 + 5 + 150, 
                Global.renderTarget.Height - back.GetHeight() * 1.75f);
        }
        else
        {
            back.SetPosition(Global.renderTarget.Width / 2 - back.GetWidth() / 2 + 5, 
                Global.renderTarget.Height - back.GetHeight() * 1.75f);
        }
    }

    public override void OnStart()
    {
        counter = 0;
        AudioManager.PauseBGM();
    }
    private Scene UpdateContButton()
    {
        if (cont.GetBounds().Contains(MouseManager.Location))
        {
            if (cont.Text != "> Continue <")
            {
                if (select.PlaybackClosed) AudioManager.AddSFX(select);
                cont.Text = "> Continue <";
                cont.MapBoundsToTextSize();
                cont.SetPosition(Global.renderTarget.Width / 2 - cont.GetWidth() / 2 + 5 - 150, 
                    Global.renderTarget.Height - cont.GetHeight() * 1.75f);
            }
            if (MouseManager.LeftButtonReleased)
            {
                Confirm c = new(
                    this,
                    "Go Beyond?",
                    "(Death will now be permanent.)",
                    () =>
                    {
                        if (Global.runData.difficultyManager.id > Global.userData.completion) Global.userData.completion = Global.runData.difficultyManager.id;
                        Global.SaveUserData();

                        TransFlicker t = new(Global.modSelect, true);
                        t.Initialize();
                        Global.modSelect.Initialize();
                        Global.sceneManager.currScene = t;
                        yay.Stop();
                        AudioManager.RemoveSFX(yay);
                    },
                    camStatic.Update
                );
                c.Initialize();
                return c;
            }
        }
        else if (cont.Text != "Continue")
        {
            cont.Text = "Continue";
            cont.MapBoundsToTextSize();
            cont.SetPosition(Global.renderTarget.Width / 2 - cont.GetWidth() / 2 + 5 - 150, 
                Global.renderTarget.Height - cont.GetHeight() * 1.75f);
        }

        return null;
    }
    private Scene UpdateBackButton()
    {
        if (back.GetBounds().Contains(MouseManager.Location))
        {
            if (back.Text != "> End Run <")
            {
                if (select.PlaybackClosed) AudioManager.AddSFX(select);
                back.Text = "> End Run <";
                back.MapBoundsToTextSize();
                if (cont == null)
                {
                    back.SetPosition(Global.renderTarget.Width / 2 - back.GetWidth() / 2 + 5, 
                        Global.renderTarget.Height - back.GetHeight() * 1.75f);
                }
                else
                {
                    back.SetPosition(Global.renderTarget.Width / 2 - back.GetWidth() / 2 + 5 + 150, 
                        Global.renderTarget.Height - back.GetHeight() * 1.75f);
                }
            }
            if (MouseManager.LeftButtonReleased)
            {
                Confirm c = new(
                    this,
                    "Return to Main Menu?",
                    () =>
                    {
                        Global.userData.recentRun = Global.runData.Clone();
                        if (Global.userData.bestRun == null 
                            || Global.userData.bestRun.hours < Global.runData.hours 
                            || Global.userData.bestRun.powerDrained > Global.runData.powerDrained)
                        {
                            Global.userData.bestRun = Global.runData.Clone();
                        }

                        if (Global.runData.difficultyManager.id > Global.userData.completion) Global.userData.completion = Global.runData.difficultyManager.id;
                        if (Global.runData.loop >= 11 
                            && Global.runData.difficultyManager.id > Global.userData.completionF) Global.userData.completionF = Global.runData.difficultyManager.id;
                        Global.runData.SetToDefaults();
                        Global.SaveUserData();
                        TransFlicker t = new(Global.mainMenu);
                        t.Initialize();
                        Global.mainMenu.Initialize();
                        Global.sceneManager.currScene = t;
                        yay.Stop();
                        AudioManager.RemoveSFX(yay);
                    },
                    camStatic.Update
                );
                c.Initialize();
                return c;
            }
        }
        else if (back.Text != "End Run")
        {
            back.Text = "End Run";
            back.MapBoundsToTextSize();
            if (cont == null)
            {
                back.SetPosition(Global.renderTarget.Width / 2 - back.GetWidth() / 2 + 5, 
                    Global.renderTarget.Height - back.GetHeight() * 1.75f);
            }
            else
            {
                back.SetPosition(Global.renderTarget.Width / 2 - back.GetWidth() / 2 + 5 + 150, 
                    Global.renderTarget.Height - back.GetHeight() * 1.75f);
            }
        }

        return null;
    }
    public override Scene Update()
    {
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;

        if (!back.visible)
        {
            if (modNodes[^1].visible && counter > 3)
            {
                AudioManager.AddSFX(yay);
                back.visible = true;
                if (cont != null) cont.visible = true;
                counter = 0;
            }
            else if (!modNodes[^1].visible && stats[^1].visible && counter > 1.75)
            {
                modsUsed.visible = true;
                foreach (ModNode n in modNodes) n.visible = true;
                counter = 0;
                AudioManager.AddSFX(boom);
            }
            else if (!stats[^1].visible && counter > .5)
            {
                stats.First(s => !s.visible).visible = true;
                counter = 0;
                AudioManager.AddSFX(boom);
            }
        }

        camStatic.Update();
        if (!back.visible) 
        {
            if (Global.userData.completion > 0 
                && (MouseManager.RightButtonReleased 
                    || MouseManager.LeftButtonReleased 
                    || KeyboardManager.PressedKeys.Count != 0
            ))
            {
                foreach (TextDisplay t in stats) t.visible = true;
                modsUsed.visible = true;
                foreach (ModNode n in modNodes) n.visible = true;
                back.visible = true;
                if (cont != null) cont.visible = true;
                boom.Stop();
                AudioManager.RemoveSFX(boom);
                AudioManager.AddSFX(yay);
            }
            return null;
        }
        Scene s = UpdateBackButton();
        if (s != null) return s;
        if (cont != null) return UpdateContButton();
        return null;
    }

    public override void Draw()
    {
        canvas.Draw();
    }
}
