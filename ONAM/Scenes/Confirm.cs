using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class Confirm : Scene
{
    private Scene prevScene;
    private string msg, msg2;
    private Action onAccept, prevUpdate;

    private Canvas canvas;
    private GameElement box;
    private TextDisplay[] buttons;
    private TextDisplay buttonHighlight, msgTxt, msgTxt2;
    private SFXObject select;

    public Confirm(Scene prevScene, string msg, Action onAccept, Action prevUpdate)
    {
        this.prevScene = prevScene;
        this.msg = msg;
        this.onAccept = onAccept;
        this.prevUpdate = prevUpdate;
        msg2 = null;
    }
    public Confirm(Scene prevScene, string msg, string msg2, Action onAccept, Action prevUpdate) : this(prevScene, msg, onAccept, prevUpdate)
    {
        this.msg2 = msg2;
    }

    public override void Initialize()
    {
        select = new(Global.content.Load<SoundEffect>("sfx/cam_switch"));

        canvas = new();
        canvas.Initialize();

        box = new("confirm-panel");
        // box.SetTexture(Global.multiTexture);
        // box.color = Color.Black;
        // box.SetDimensions(
        //     Global.renderTarget.Width / 2,
        //     Global.renderTarget.Width / 4
        // );
        box.SetPosition(
            Global.renderTarget.Width / 2 - box.GetWidth() / 2,
            Global.renderTarget.Height / 2 - box.GetHeight() / 2
        );
        // box.opacity = .95f;
        canvas.Add(6, box);
        
        msgTxt = new(msg, "consolas");
        msgTxt.SetPosition(
            Global.renderTarget.Width / 2 - msgTxt.GetWidth() / 2,
            box.GetPosition().Y + 50
        );
        canvas.Add(7, msgTxt);
        if(msg2 != null)
        {
            msgTxt2 = new(msg2, "fnaf");
            msgTxt2.SetPosition(
                Global.renderTarget.Width / 2 - msgTxt2.GetWidth() / 2,
                msgTxt.GetBounds().Bottom
            );
            canvas.Add(7, msgTxt2);
        }
        else
        {
            msgTxt.SetPosition(msgTxt.GetPosition().X, msgTxt.GetPosition().Y + 20);
        }

        buttonHighlight = new(">>", "consolas");
        buttonHighlight.visible = false;
        buttonHighlight.MapBoundsToTextSize();
        canvas.Add(7, buttonHighlight);

        buttons = [new ("Yes", "consolas"), new("No", "consolas")];
        buttons[0].SetPosition(
            Global.renderTarget.Width / 2 - buttons[0].GetWidth() / 2 - 75,
            box.GetBounds().Bottom - 80
        );
        buttons[1].SetPosition(
            Global.renderTarget.Width / 2 - buttons[1].GetWidth() / 2 + 75,
            box.GetBounds().Bottom - 80
        );
        canvas.Add(7, buttons[0]);
        canvas.Add(7, buttons[1]);
    }

    public override void OnStart()
    {
        
    }

    private Scene CheckInput()
    {
        if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape)) return prevScene;
        if (KeyboardManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter)) onAccept.Invoke();

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetBounds().Contains(MouseManager.Location))
            {
                if (buttonHighlight.GetPosition().Y != buttons[i].GetPosition().Y)
                {
                    buttonHighlight.SetPosition(
                        buttons[i].GetPosition().X - buttonHighlight.GetWidth() - 5,
                        buttons[i].GetPosition().Y);
                    if (select.PlaybackClosed) AudioManager.AddSFX(select);
                    buttonHighlight.visible = true;
                }
                
                if (MouseManager.LeftButtonReleased)
                {
                    switch (i)
                    {
                        case 0:
                            onAccept.Invoke();
                            break;
                        case 1:
                            return prevScene;
                    }
                }
                return null;
            }
        }
        if (buttonHighlight.visible)
        {
            buttonHighlight.visible = false;
            buttonHighlight.SetPosition(0, 0);
        }
        return null;
    }
    public override Scene Update()
    {
        prevUpdate.Invoke();
        return CheckInput();
    }

    public override void Draw()
    {
        prevScene.Draw();
        canvas.Draw();
    }
}
