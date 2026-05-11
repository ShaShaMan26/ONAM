using System;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class CustomSelect : Scene
{
    private ModNode[] modNodes;
    private TextDisplay titleDisp, descDisp;
    private SFXObject select;
    private int selectionID;

    private CamStatic camStatic;

    private Canvas canvas;
    private GameElement[] AINodes;
    private SettingsButton[] buttons;

    private GameElement backer;

    public override void Initialize()
    {
        Global.customNight = true;
        Global.runData = Global.userData.customRun;

        if (Global.runData.difficultyManager == null)
        {
            Global.runData.SetDifficulty("hard");
            Global.runData.difficultyManager.id = 20;
            Global.runData.difficultyManager.level = [0, 0, 0, 0];
            Global.runData.difficultyManager.m_level = 0;
        }

        selectionID = -1;
        
        camStatic = new(.2f, .3f);
        camStatic.Initialize();
        select = new(Global.content.Load<SoundEffect>("sfx/cam_switch"));
        titleDisp = new("", "consolas");
        titleDisp.SetPosition(0, Global.renderTarget.Height - 200);
        descDisp = new("", "fnaf");
        descDisp.SetPosition(0, Global.renderTarget.Height - 150);

        int id = 0;
        modNodes = new ModNode[Global.modifiers.Length];
        for (int i = 0; i < modNodes.Length; i++, id++)
        {
            modNodes[i] = new(Global.modifiers[id], id);
            if (i == 0) modNodes[i].SetPosition(28 + (Global.renderTarget.Width - ((modNodes[0].GetWidth() + 20) * 7)) / 2,
                30);
            else if (i % 7 == 0) modNodes[i].SetPosition(modNodes[0].GetPosition().X, modNodes[i - 1].GetPosition().Y + modNodes[i].GetHeight() + 20);
            else modNodes[i].SetPosition(modNodes[i - 1].GetPosition().X + modNodes[i - 1].GetWidth() + 20, modNodes[i - 1].GetPosition().Y);
            if (!Global.runData.activeModifiers[i]) modNodes[i].opacity = .5f;
        }

        // miku ai
        canvas = new();
        canvas.Initialize();
        AINodes = new GameElement[5];

        buttons = new SettingsButton[AINodes.Length];
        buttons[0] = new("20",
            () => {DecreaseAI(0);},
            () => {IncreaseAI(0);},
            () => {}
        );
        buttons[1] = new("20",
            () => {DecreaseAI(1);},
            () => {IncreaseAI(1);},
            () => {}
        );
        buttons[2] = new("20",
            () => {DecreaseAI(2);},
            () => {IncreaseAI(2);},
            () => {}
        );
        buttons[3] = new("20",
            () => {DecreaseAI(3);},
            () => {IncreaseAI(3);},
            () => {}
        );
        buttons[4] = new("20",
            () => {DecreaseAI(4);},
            () => {IncreaseAI(4);},
            () => {}
        );

        for (int i = 0; i < AINodes.Length; i++)
        {
            AINodes[i] = new("cus_icons/" + i);
            if (i == 0) AINodes[i].SetPosition((50 + Global.renderTarget.Width - (AINodes[i].GetWidth() + 50) * AINodes.Length) / 2,
                515);
            else AINodes[i].SetPosition(AINodes[i - 1].GetPosition().X + AINodes[i - 1].GetWidth() + 50, AINodes[i - 1].GetPosition().Y);
            canvas.Add(AINodes[i]);

            if (i < 4)
            {
                // buttons[i].title.Text = "" + Global.difficultyManager.level[i];
                buttons[i].title.Text = "" + Global.runData.difficultyManager.level[i];
                buttons[i].CenterText();
            }
            else
            {
                // buttons[i].title.Text = "" + Global.difficultyManager.m_level;
                buttons[i].title.Text = "" + Global.runData.difficultyManager.m_level;
                buttons[i].CenterText();
            }

            buttons[i].SetPosition(AINodes[i].GetPosition().X - 8,
                AINodes[i].GetPosition().Y + AINodes[i].GetHeight() + 10);
            canvas.Add(buttons[i]);
        }

        backer = new("office");
        backer.color = Color.Black;
        backer.visible = false;
        backer.opacity = .9f;
    }

    private void DecreaseAI(int id)
    {
        if (id < 4)
        {
            if (Global.runData.difficultyManager.level[id] > 0) 
            {
                buttons[id].title.Text = "" + (Global.runData.difficultyManager.level[id] - 1);
                Global.runData.difficultyManager.level[id]--;
            }
        }
        else
        {
            if (Global.runData.difficultyManager.m_level > 0) 
            {
                buttons[id].title.Text = "" + (Global.runData.difficultyManager.m_level - 1);
                Global.runData.difficultyManager.m_level--;
            }
        }
    }
    private void IncreaseAI(int id)
    {
        if (id < 4)
        {
            if (Global.runData.difficultyManager.level[id] < 20) 
            {
                buttons[id].title.Text = "" + (Global.runData.difficultyManager.level[id] + 1);
                Global.runData.difficultyManager.level[id]++;
            }
        }
        else
        {
            if (Global.runData.difficultyManager.m_level < 20) 
            {
                buttons[id].title.Text = "" + (Global.runData.difficultyManager.m_level + 1);
                Global.runData.difficultyManager.m_level++;
            }
        }
    }

    public override void OnStart()
    {
        
    }

    private Scene CheckInput()
    {
        // debug
        if (KeyboardManager.KeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
        {
            // save custom night data
            // File.WriteAllText(Global.content.RootDirectory + "/difficulties/custom.txt", JsonSerializer.Serialize(Global.difficultyManager));
            Global.SaveUserData();

            TransFlicker t = new(Global.mainMenu);
            t.Initialize();
            Global.sceneManager.currScene = t;
            return t;
        }
        if (KeyboardManager.KeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
        {
            // File.WriteAllText(Global.content.RootDirectory + "/difficulties/custom.txt", JsonSerializer.Serialize(Global.difficultyManager));
            // Global.LoadDifficulty("custom");
            Global.runData.shopAccessible = false;
            Global.runData.ResetTokens();
            Global.SaveUserData();

            AudioManager.PauseBGM();
            Global.loadNight = new();
            Global.loadNight.Initialize();
            return Global.loadNight;
        }
        // end debug

        foreach (SettingsButton s in buttons) s.Update();

        for (int i = 0; i < modNodes.Length; i++)
        {
            if (modNodes[i] != null && modNodes[i].GetBounds().Contains(MouseManager.Location))
            {
                if (selectionID != modNodes[i].id)
                {
                    titleDisp.visible = true;
                    descDisp.visible = true;
                    if (select.PlaybackClosed) AudioManager.AddSFX(select);
                    titleDisp.Text = ">" + modNodes[i].modifier.title + "<";
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
                    Global.runData.activeModifiers[i] = !Global.runData.activeModifiers[i];
                    if (Global.runData.activeModifiers[i]) modNodes[i].opacity = 1;
                    else modNodes[i].opacity = .5f;
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

        backer.visible = titleDisp.visible;
        backer.SetDimensions((int) descDisp.GetWidth(), (int) (descDisp.GetPosition().Y + descDisp.GetHeight() - titleDisp.GetPosition().Y));
        backer.SetPosition(descDisp.GetPosition().X, titleDisp.GetPosition().Y);

        camStatic.Update();
        return s;
    }

    public override void Draw()
    {
        canvas.Draw();
        foreach (ModNode m in modNodes) m?.Draw();
        camStatic.Draw();
        backer.Draw();
        titleDisp.Draw();
        descDisp.Draw();
    }
}
