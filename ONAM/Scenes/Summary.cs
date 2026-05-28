using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class Summary : Scene
{
    private double counter;

    private Canvas canvas;
    private CamStatic camStatic;
    private TextDisplay[] stats;
    private TextDisplay modsUsed, bgTxt, back;
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

        stats = [
            new("Mikus Deterred: " + Global.runData.mikusDeterred, "fnaf"),
            new("Mikulings Calmed: " + Global.runData.mikulingsCalmed, "fnaf"),
            new("Doors Closed: " + Global.runData.doorsClosed, "fnaf"),
            new("Vents Sealed: " + Global.runData.ventsSealed, "fnaf"),
            new("Power Drained: " + Global.runData.powerDrained + "%", "fnaf"),
            new("Deaths: " + Global.runData.deaths, "fnaf")
        ];
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i].SetPosition(
                Global.renderTarget.Width / 2 - stats[i].GetWidth() / 2,
                i == 0 ? 50 : stats[i - 1].GetBounds().Bottom
            );
            stats[i].visible = false;
            canvas.Add(7, stats[i]);
        }

        modsUsed = new("Mods Used:", "fnaf");
        modsUsed.SetPosition(
            Global.renderTarget.Width / 2 - modsUsed.GetWidth() / 2,
            stats[^1].GetBounds().Bottom + 40
        );
        modsUsed.visible = false;
        canvas.Add(7, modsUsed);

        modNodes = new ModNode[Global.runData.enabledMods.Count];
        for (int i = 0; i < modNodes.Length; i++)
        {
            modNodes[i] = new(Global.runData.enabledMods[i], 0);
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
        back.SetPosition(Global.renderTarget.Width / 2 - back.GetWidth() / 2 + 5, 
            Global.renderTarget.Height - back.GetHeight() * 1.75f);
        back.visible = false;
        canvas.Add(7, back);
    }

    public override void OnStart()
    {
        // base.OnStart();
        counter = 0;
        AudioManager.PauseBGM();
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
                back.SetPosition(Global.renderTarget.Width / 2 - back.GetWidth() / 2 + 5, 
                    Global.renderTarget.Height - back.GetHeight() * 1.75f);
            }
            if (MouseManager.LeftButtonReleased)
            {
                Confirm c = new(
                    this,
                    "Return to Main Menu?",
                    () =>
                    {
                        if (Global.runData.difficultyManager.id > Global.userData.completion) Global.userData.completion = Global.runData.difficultyManager.id;
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
            back.SetPosition(Global.renderTarget.Width / 2 - back.GetWidth() / 2 + 5, 
                Global.renderTarget.Height - back.GetHeight() * 1.75f);
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
                boom.Stop();
                AudioManager.RemoveSFX(boom);
                AudioManager.AddSFX(yay);
            }
            return null;
        }
        return UpdateBackButton();
    }

    public override void Draw()
    {
        canvas.Draw();
    }
}
