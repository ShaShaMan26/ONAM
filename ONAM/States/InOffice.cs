using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ONAM;

public class InOffice : State
{
    private int leftBound, rightBound;

    public override void Initialize()
    {
        base.Initialize();

        leftBound = Global.renderTarget.Width / 4;
        rightBound = Global.renderTarget.Width - Global.renderTarget.Width / 4;
        UpdateView();
    }

    protected void UpdateView()
    {
        float a = (float) (Global.gameTime.ElapsedGameTime.TotalSeconds * rightBound * 1.5 *  
            Math.Abs(Global.renderTarget.Width / 2 - MouseManager.Location.X) /
            (Global.renderTarget.Width / 2));

        if (MouseManager.Location.X <= leftBound 
            && Global.night.office.bg.GetPosition().X < 0)
        {
            Global.night.office.bg.SetPosition(
                Global.night.office.bg.GetPosition().X + a, Global.night.office.bg.GetPosition().Y
                );

            if (Global.night.office.bg.GetPosition().X > 0)
            {
                Global.night.office.bg.SetPosition(Vector2.Zero);
            }
        }
        else if (MouseManager.Location.X >= rightBound
            && Global.night.office.bg.GetPosition().X > -Global.night.office.bg.GetWidth() + Global.renderTarget.Width + 32) 
        {
            Global.night.office.bg.SetPosition(
                Global.night.office.bg.GetPosition().X - a, Global.night.office.bg.GetPosition().Y
                );
            
            if (Global.night.office.bg.GetPosition().X < -Global.night.office.bg.GetWidth() + Global.renderTarget.Width + 32)
            {
                Global.night.office.bg.SetPosition(-Global.night.office.bg.GetWidth() + 
                    Global.renderTarget.Width + 32, 0);
            }
        }

        // doors
        Global.night.office.door_L.SetPosition(Global.night.office.bg.GetPosition() + new Vector2(70, 0));
        Global.night.office.door_R.SetPosition(Global.night.office.bg.GetPosition() + new Vector2(1272, 0));

        // door buttons
        Global.night.office.door_button_holder_l.SetPosition(Global.night.office.bg.GetPosition() + new Vector2(-8, 270));
        Global.night.office.door_button_l.SetPosition(Global.night.office.door_button_holder_l.GetPosition() + new Vector2(29, 52));
        Global.night.office.door_button_holder_r.SetPosition(Global.night.office.bg.GetPosition() + new Vector2(Global.night.office.bg.GetWidth() - 116, 270));
        Global.night.office.door_button_r.SetPosition(Global.night.office.door_button_holder_r.GetPosition() + new Vector2(23, 52));

        // eyes
        Global.night.office.door_eyes_l.SetPosition(Global.night.office.bg.GetPosition() + new Vector2(-20, 360));
        Global.night.office.door_eyes_r.SetPosition(Global.night.office.bg.GetPosition() + 
            new Vector2(Global.night.office.bg.GetWidth() - 400, 
            360));

        Global.night.office.mikulingManager.SetPosition(Global.night.office.bg.GetPosition());
    }

    protected bool MouseOverCamBar()
    {
        return Global.night.office.camBar.GetBounds().Contains(MouseManager.Location);
    }

    protected State CheckCamFlip()
    {
        if (KeyboardManager.KeyPressed(Keys.S) 
            || (Global.night.office.camBar.visible && MouseOverCamBar()))
        {
            Global.night.office.camBar.visible = false;
            return Global.night.openCams;
        }
        else if (!Global.night.office.camBar.visible && !MouseOverCamBar())
        {
            Global.night.office.camBar.visible = true;
        }
        return null;
    }

    protected void CheckAction()
    {
        if (KeyboardManager.KeyPressed(Keys.A))
        {
            Global.night.office.door_L.Toggle();
        }
        if (KeyboardManager.KeyPressed(Keys.D))
        {
            Global.night.office.door_R.Toggle();
        }

        if (KeyboardManager.KeyPressed(Keys.Space)) Global.night.jumpytime = true;

        if (!MouseManager.LeftButtonClicked) return;
        if (Global.night.office.door_button_l.GetBounds().Contains(MouseManager.Location)) Global.night.office.door_L.Toggle();
        if (Global.night.office.door_button_r.GetBounds().Contains(MouseManager.Location)) Global.night.office.door_R.Toggle();
    }

    public override State Update()
    {
        UpdateView();
        CheckAction();
        return CheckCamFlip();
    }
}
