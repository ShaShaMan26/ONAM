using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class ModSelect : Scene
{
    private ModNode[] modNodes;
    private TextDisplay titleDisp, descDisp;
    private SFXObject select;
    private int selectionID;

    private CamStatic camStatic;

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

        Random r = new();
        int id = 0, j;
        modNodes = new ModNode[3];
        for (int i = 0; i < modNodes.Length; i++)
        {
            j = 0;
            do 
            {
                if (j > Global.modifiers.Length) break;
                id = r.Next(0, Global.modifiers.Length);
                j++;
            }
            while (Global.userData.activeModifiers[id]);

            if (j > Global.modifiers.Length) modNodes[i] = null;
            else
            {
                modNodes[i] = new(Global.modifiers[id], id);
                Global.userData.activeModifiers[id] = true;
            }
        }
        foreach (ModNode m in modNodes)
        {
            if (m != null) Global.userData.activeModifiers[m.id] = false;
        }

        modNodes[0]?.SetPosition(Global.renderTarget.Width / 2 - modNodes[0].GetWidth() / 2, 
            Global.renderTarget.Height / 2 - modNodes[0].GetHeight() / 2 - 75);
        modNodes[1]?.SetPosition(modNodes[0].GetPosition() - new Vector2(modNodes[1].GetWidth() + 50, 0));
        modNodes[2]?.SetPosition(modNodes[0].GetPosition() + new Vector2(modNodes[2].GetWidth() + 50, 0));
    }

    public override void OnStart()
    {
        
    }

    private Scene CheckInput()
    {
        for (int i = 0; i < modNodes.Length; i++)
        {
            if (modNodes[i] != null && modNodes[i].GetBounds().Contains(MouseManager.Location))
            {
                if (selectionID != modNodes[i].id)
                {
                    titleDisp.visible = true;
                    descDisp.visible = true;
                    if (select.PlaybackClosed) AudioManager.AddSFX(select);
                    titleDisp.Text = modNodes[i].modifier.title;
                    titleDisp.MapBoundsToTextSize();
                    titleDisp.SetPosition(Global.renderTarget.Width / 2 - titleDisp.GetWidth() / 2, 
                        titleDisp.GetPosition().Y);
                    descDisp.Text = modNodes[i].modifier.desc;
                    descDisp.MapBoundsToTextSize();
                    descDisp.SetPosition(Global.renderTarget.Width / 2 - descDisp.GetWidth() / 2, 
                        descDisp.GetPosition().Y);

                    selectionID = modNodes[i].id;
                }
                if (MouseManager.LeftButtonClicked)
                {
                    Global.userData.loop++;
                    if (Global.userData.loop > 1) Global.userData.firstTime = false;
                    Global.userData.activeModifiers[selectionID] = true;
                    Global.SaveUserData();

                    LoadNight l = new();
                    l.Initialize();
                    return l;
                }
                return null;
            }
        }
        selectionID = -1;
        titleDisp.visible = false;
        descDisp.visible = false;
        return null;
    }

    public override Scene Update()
    {
        Scene s = CheckInput();
        camStatic.Update();
        return s;
    }

    public override void Draw()
    {
        foreach (ModNode m in modNodes) m?.Draw();
        camStatic.Draw();
        titleDisp.Draw();
        descDisp.Draw();
    }
}
