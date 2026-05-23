using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace ONAM;

public class ModView : GameElement
{
    private ModNode[] nodes;
    private Canvas canvas;
    private int padding;
    private TextDisplay titleDisp, descDisp;
    private int selIndex;
    private GameElement textBack, iconBack;

    public ModView() : base("miku")
    {
        padding = 25;
    }

    public void Initialize()
    {
        canvas = new();
        canvas.Initialize();

        nodes = new ModNode[Global.runData.activeModifiers.Count(m => m)];
        int j = 0;
        for (int i = 0; i < Global.runData.activeModifiers.Length; i++)
        {
            if (Global.runData.activeModifiers[i])
            {
                nodes[j] = new(Global.modifiers[i], i);
                nodes[j].outlineThickness = 0;
                nodes[j].outlineOffset = 0;
                nodes[j].drawOutline = false;
                nodes[j].SetOutline();
                // nodes[j].SetDimensions();
                j++;
            }
        }
        if (!Global.customNight)
        {
            ModNode[] n = new ModNode[nodes.Length];
            for (int i = 0; i < nodes.Length; i++)
            {
                for (j = 0; j < nodes.Length; j++)
                {
                    if (Global.runData.enabledMods[i].title == nodes[j].modifier.title)
                    {
                        n[i] = nodes[j];
                    }
                }
            }
            nodes = n;
        }

        for (int i = 0; i < nodes.Length; i++)
        {
            if (i == 0) nodes[i].SetPosition(
                (Global.renderTarget.Width - (Math.Clamp(nodes.Length, 0, 8) * nodes[i].GetWidth() + (Math.Clamp(nodes.Length, 0, 8) - 1) * padding)) / 2,
                nodes.Length > 8 ? 125 : 175
            );
            else if (i == 8) nodes[i].SetPosition(
                nodes[0].GetPosition().X, 
                nodes[0].GetBounds().Bottom + 20
            );
            else nodes[i].SetPosition(
                nodes[i - 1].GetBounds().Right + padding, 
                nodes[i - 1].GetPosition().Y
            );
            canvas.Add(7, nodes[i]);
        }

        selIndex = -1;
        titleDisp = new("", "consolas");
        titleDisp.SetPosition(0, Global.renderTarget.Height - 215);
        canvas.Add(7, titleDisp);
        descDisp = new("", "fnaf");
        descDisp.SetPosition(0, Global.renderTarget.Height - 175);
        canvas.Add(7, descDisp);
        titleDisp.visible = false;
        descDisp.visible = false;

        textBack = new("miku");
        textBack.SetTexture(Global.multiTexture);
        textBack.color = Color.Black;
        textBack.opacity = .85f;
        textBack.visible = false;
        canvas.Add(6, textBack);

        iconBack = new("miku");
        iconBack.SetTexture(Global.multiTexture);
        iconBack.color = Color.Black;
        iconBack.opacity = .75f;
        canvas.Add(6, iconBack);
        if (nodes.Length > 0)
        {
            iconBack.SetPosition(0, nodes[0].GetPosition().Y - 20);
            iconBack.SetDimensions(Global.renderTarget.Width, (int) (nodes[^1].GetBounds().Bottom - nodes[0].GetPosition().Y + 40));
        }
    }

    public void Update()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].GetBounds().Contains(MouseManager.Location))
            {
                if (selIndex != i)
                {
                    titleDisp.Text = nodes[i].modifier.title;
                    titleDisp.MapBoundsToTextSize();
                    titleDisp.SetPosition(
                        Global.renderTarget.Width / 2 - titleDisp.GetWidth() / 2, 
                        titleDisp.GetPosition().Y
                    );
                    descDisp.Text = nodes[i].modifier.desc;
                    descDisp.MapBoundsToTextSize();
                    descDisp.SetPosition(
                        Global.renderTarget.Width / 2 - descDisp.GetWidth() / 2, 
                        descDisp.GetPosition().Y
                    );

                    textBack.SetPosition(
                        titleDisp.GetWidth() > descDisp.GetWidth() ? titleDisp.GetPosition().X : descDisp.GetPosition().X,
                        titleDisp.GetPosition().Y
                    );
                    textBack.SetDimensions(
                        (int) (titleDisp.GetWidth() > descDisp.GetWidth() ? titleDisp.GetWidth() : descDisp.GetWidth()),
                        (int) (descDisp.GetBounds().Bottom - titleDisp.GetPosition().Y)
                    );

                    if (selIndex < 0)
                    {
                        titleDisp.visible = true;
                        descDisp.visible = true;
                        textBack.visible = true;
                    }
                    selIndex = i;
                    // if (select.PlaybackClosed) AudioManager.AddSFX(select);
                }
                return;
            }
        }
        if (selIndex > -1)
        {
            titleDisp.visible = false;
            descDisp.visible = false;
            textBack.visible = false;
            selIndex = -1;
        }
    }

    public override void Draw()
    {
        // base.Draw();
        if (visible) canvas.Draw();
    }
}
