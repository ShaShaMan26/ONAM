using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class ShopScreen : GameElement
{
    private Canvas canvas;
    private ShopNode[] nodes;
    private TextDisplay titleDisp, descDisp;
    private SFXObject select, buy, poor;
    private int selIndex;

    public ShopScreen() : base("miku")
    {
        canvas = new();
        canvas.Initialize();

        nodes = [
            new("shop_icons/auto_door", "Auto Door", "Door will shut automatically.", 5),
            new("shop_icons/auto_vent", "Auto Seal", "Vent will seal automatically.", 5),
            new("shop_icons/freeze", "Freeze", "No one move for 15 seconds.", 10),
            new("shop_icons/hour", "Skip an Hour", "What do you think it does?", 30)
        ];
        for (int i = 0; i < nodes.Length; i++)
        {
            if (i == 0) nodes[i].SetPosition(100, 100);
            else nodes[i].SetPosition(
                nodes[i - 1].GetPosition().X + nodes[i - 1].GetWidth() + 25,
                nodes[i - 1].GetPosition().Y
            );
            canvas.Add(nodes[i]);

            TextDisplay t = new("" + nodes[i].cost, "fnaf");
            t.SetPosition(
                nodes[i].GetPosition().X + nodes[i].GetWidth() / 2 - t.GetWidth() / 2,
                nodes[i].GetPosition().Y - 45
            );
            canvas.Add(t);
        }

        select = new(Global.content.Load<SoundEffect>("sfx/cam_switch"));
        buy = new(Global.content.Load<SoundEffect>("sfx/reg"));
        buy.Volume = .25f;
        poor = new(Global.content.Load<SoundEffect>("sfx/error"));

        selIndex = -1;
        titleDisp = new("", "consolas");
        titleDisp.SetPosition(0, Global.renderTarget.Height - 300);
        canvas.Add(titleDisp);
        descDisp = new("", "fnaf");
        descDisp.SetPosition(0, Global.renderTarget.Height - 250);
        canvas.Add(descDisp);
        titleDisp.visible = false;
        descDisp.visible = false;
    }

    private void CheckSelection()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].GetBounds().Contains(MouseManager.Location))
            {
                if (selIndex != i)
                {
                    titleDisp.Text = nodes[i].title;
                    titleDisp.MapBoundsToTextSize();
                    titleDisp.SetPosition(
                        Global.renderTarget.Width / 2 - titleDisp.GetWidth() / 2, 
                        titleDisp.GetPosition().Y
                    );
                    descDisp.Text = nodes[i].desc;
                    descDisp.MapBoundsToTextSize();
                    descDisp.SetPosition(
                        Global.renderTarget.Width / 2 - descDisp.GetWidth() / 2, 
                        descDisp.GetPosition().Y
                    );

                    if (selIndex < 0)
                    {
                        titleDisp.visible = true;
                        descDisp.visible = true;
                    }
                    selIndex = i;
                    if (select.PlaybackClosed) AudioManager.AddSFX(select);
                }
                return;
            }
        }
        if (selIndex > 0)
        {
            titleDisp.visible = false;
            descDisp.visible = false;
            selIndex = -1;
        }
    }
    private void CheckPurchase()
    {
        if (MouseManager.LeftButtonClicked && selIndex >= 0)
        {
            if (nodes[selIndex].opacity == 1)
            {
                Global.night.SubTokens(nodes[selIndex].cost);
                AudioManager.AddSFX(buy);
            }
            else AudioManager.AddSFX(poor);
        }
    }

    public void Update()
    {
        CheckSelection();
        CheckPurchase();
    }

    public void UpdateItemVis()
    {
        foreach (ShopNode s in nodes)
        {
            if (Global.night.tokens >= s.cost) s.opacity = 1;
            else s.opacity = .75f;
        }
    }

    public override void Draw()
    {
        // base.Draw();
        if (visible)
        {
            canvas.Draw();
        }
    }
}
