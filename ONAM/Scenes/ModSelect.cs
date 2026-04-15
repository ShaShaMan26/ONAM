using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class ModSelect : Scene
{
    private ModNode[] modNodes;
    private int selectionID;

    private CamStatic camStatic;
    private TextDisplay titleDisp, descDisp;
    private SFXObject select;

    public override void Initialize()
    {
        selectionID = -1;
        
        camStatic = new(.2f, .3f);
        camStatic.Initialize();
        select = new(Global.content.Load<SoundEffect>("sfx/cam_switch"));
        titleDisp = new("", "consolas");
        titleDisp.SetPosition(0, Global.renderTarget.Height - 300);
        descDisp = new("", "fnaf");
        descDisp.SetPosition(0, Global.renderTarget.Height - 250);

        modNodes = new ModNode[3];
        for (int i = 0; i < modNodes.Length; i++)
        {
            modNodes[i] = new();
            modNodes[i].modID = i;
            modNodes[i].title = "test"+(i+1);
            modNodes[i].desc = "description"+(i+1);
        }
        modNodes[0].SetPosition(Global.renderTarget.Width / 2 - modNodes[0].GetWidth() / 2, 
            Global.renderTarget.Height / 2 - modNodes[0].GetHeight() / 2 - 75);
        modNodes[1].SetPosition(modNodes[0].GetPosition() - new Vector2(modNodes[1].GetWidth() + 50, 0));
        modNodes[2].SetPosition(modNodes[0].GetPosition() + new Vector2(modNodes[2].GetWidth() + 50, 0));
    }

    public override void OnStart()
    {
        
    }

    private void CheckInput()
    {
        for (int i = 0; i < modNodes.Length; i++)
        {
            if (modNodes[i].GetBounds().Contains(MouseManager.Location))
            {
                if (selectionID != modNodes[i].modID)
                {
                    titleDisp.visible = true;
                    descDisp.visible = true;
                    if (select.PlaybackClosed) AudioManager.AddSFX(select);
                    titleDisp.Text = modNodes[i].title;
                    titleDisp.MapBoundsToTextSize();
                    titleDisp.SetPosition(Global.renderTarget.Width / 2 - titleDisp.GetWidth() / 2, 
                        titleDisp.GetPosition().Y);
                    descDisp.Text = modNodes[i].desc;
                    descDisp.MapBoundsToTextSize();
                    descDisp.SetPosition(Global.renderTarget.Width / 2 - descDisp.GetWidth() / 2, 
                        descDisp.GetPosition().Y);

                    selectionID = modNodes[i].modID;
                }
                return;
            }
        }
        selectionID = -1;
        titleDisp.visible = false;
        descDisp.visible = false;
    }

    public override Scene Update()
    {
        CheckInput();
        camStatic.Update();
        return null;
    }

    public override void Draw()
    {
        foreach (ModNode m in modNodes) m.Draw();
        camStatic.Draw();
        titleDisp.Draw();
        descDisp.Draw();
    }
}
