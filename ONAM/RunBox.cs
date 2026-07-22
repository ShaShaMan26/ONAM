using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace ONAM;

public class RunBox : GameElement
{
    private TextDisplay title, modsUsed, warning, diffDisp;
    private RunData run;
    private TextDisplay[] stats;
    private ModNode[] modNodes;

    public RunBox(string title, RunData run) : base("run-panel")
    {
        this.title = new(title, "consolas");
        this.run = run;
    }

    public void Initialize()
    {
        title.SetPosition(
            pos.X + (GetWidth() / 2 - title.GetWidth() / 2),
            pos.Y + 20
        );

        warning = null;
        if (run == null)
        {
            warning = new("No Run Data", "fnaf");
            warning.SetPosition(
                pos.X + (GetWidth() / 2 - warning.GetWidth() / 2),
                pos.Y + (GetHeight() / 2 - warning.GetHeight() / 2)
            );
            return;
        }
        stats = [
            new("Mikus Deterred: " + run.mikusDeterred, "fnaf-small"),
            new("Mikulings Calmed: " + run.mikulingsCalmed, "fnaf-small"),
            new("Doors Closed: " + run.doorsClosed, "fnaf-small"),
            new("Vents Sealed: " + run.ventsSealed, "fnaf-small"),
            new("Power Drained: " + run.powerDrained + "%", "fnaf-small"),
            new("Deaths: " + run.deaths, "fnaf-small"),
            new("Hours Survived: " + run.hours, "fnaf-small"),
            new("Furthest Loop: " + run.loop, "fnaf-small")
        ];
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i].SetPosition(
                pos.X + (GetWidth() / 2 - stats[i].GetWidth() / 2),
                i == 0 ? title.GetBounds().Bottom : stats[i - 1].GetBounds().Bottom - 12
            );
        }

        modsUsed = new("Mods Used:", "fnaf-small");
        modsUsed.SetPosition(
            pos.X + (GetWidth() / 2 - modsUsed.GetWidth() / 2),
            stats[^1].GetBounds().Bottom + 5
        );

        modNodes = new ModNode[run.enabledMods.Count + run.shadowModifiers.Count(m => m)];
        for (int i = 0; i < modNodes.Length; i++)
        {
            if (i < run.enabledMods.Count)
            {
                modNodes[i] = new(run.enabledMods[i], 0);
            }
            else
            {
                Modifier m = new();
                m.title = "";
                m.desc = "";
                if (i == run.enabledMods.Count)
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
            modNodes[i].SetDimensions(38, 38);
            modNodes[i].SetOutline();

            if (i == 0) modNodes[i].SetPosition(
                pos.X + (GetWidth() - (modNodes.Length * modNodes[0].GetWidth() + (modNodes.Length - 1) * 10)) / 2,
                modsUsed.GetBounds().Bottom
            );
            else modNodes[i].SetPosition(
                modNodes[i - 1].GetBounds().Right + 6,
                modNodes[i - 1].GetPosition().Y
            );
        }

        diffDisp = new("(" + run.difficultyManager.DiffToString() + ")", "fnaf-small");
        diffDisp.SetPosition(
            pos.X + (GetWidth() / 2 - diffDisp.GetWidth() / 2),
            GetBounds().Bottom
        );
    }

    public override void SetPosition(Vector2 pos)
    {
        base.SetPosition(pos);
        title.SetPosition(
            pos.X + (GetWidth() / 2 - title.GetWidth() / 2),
            pos.Y + 20
        );
        if (run == null)
        {
            warning.SetPosition(
                pos.X + (GetWidth() / 2 - warning.GetWidth() / 2),
                pos.Y + (GetHeight() / 2 - warning.GetHeight() / 2)
            );
        }
        else
        {
            for (int i = 0; i < stats.Length; i++)
            {
                stats[i].SetPosition(
                    pos.X + (GetWidth() / 2 - stats[i].GetWidth() / 2),
                    i == 0 ? title.GetBounds().Bottom : stats[i - 1].GetBounds().Bottom - 12
                );
            }
            modsUsed.SetPosition(
                pos.X + (GetWidth() / 2 - modsUsed.GetWidth() / 2),
                stats[^1].GetBounds().Bottom + 5
            );
            for (int i = 0; i < modNodes.Length; i++)
            {
                if (i == 0) modNodes[i].SetPosition(
                    pos.X + (GetWidth() - (modNodes.Length * modNodes[0].GetWidth() + (modNodes.Length - 1) * 10)) / 2,
                    modsUsed.GetBounds().Bottom
                );
                else modNodes[i].SetPosition(
                    modNodes[i - 1].GetBounds().Right + 6,
                    modNodes[i - 1].GetPosition().Y
                );
            }
        }
        
        diffDisp?.SetPosition(
            pos.X + (GetWidth() / 2 - diffDisp.GetWidth() / 2),
            GetBounds().Bottom - 5
        );
    }

    public override void Draw()
    {
        base.Draw();
        title.Draw();
        if (warning == null)
        {
            foreach (TextDisplay t in stats) t.Draw();
            modsUsed.Draw();
            foreach (ModNode m in modNodes) m.Draw();
        }
        else warning.Draw();
        diffDisp?.Draw();
    }
}
