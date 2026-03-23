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

        leftBound = Global.graphics.PreferredBackBufferWidth / 3;
        rightBound = Global.graphics.PreferredBackBufferWidth - Global.graphics.PreferredBackBufferWidth / 3;
    }

    private void UpdateView()
    {
        float a = 25 * 
            Math.Abs(Global.graphics.PreferredBackBufferWidth / 2 - MouseManager.Location.X) /
            (Global.graphics.PreferredBackBufferWidth / 2);

        if (MouseManager.Location.X <= leftBound 
            && Global.office.bg.GetPosition().X < 0)
        {
            Global.office.bg.SetPosition(
                Global.office.bg.GetPosition().X + a, Global.office.bg.GetPosition().Y
                );

            if (Global.office.bg.GetPosition().X > 0)
            {
                Global.office.bg.SetPosition(Vector2.Zero);
            }
        }
        else if (MouseManager.Location.X >= rightBound
            && Global.office.bg.GetPosition().X > -Global.office.bg.GetWidth() + Global.graphics.PreferredBackBufferWidth + 32) 
        {
            Global.office.bg.SetPosition(
                Global.office.bg.GetPosition().X - a, Global.office.bg.GetPosition().Y
                );
            
            if (Global.office.bg.GetPosition().X < -Global.office.bg.GetWidth() + Global.graphics.PreferredBackBufferWidth + 32)
            {
                Global.office.bg.SetPosition(-Global.office.bg.GetWidth() + 
                    Global.graphics.PreferredBackBufferWidth + 32, 0);
            }
        }

        // doors
        Global.office.door_L.SetPosition(Global.office.bg.GetPosition() + new Vector2(70, 0));
        Global.office.door_R.SetPosition(Global.office.bg.GetPosition() + new Vector2(1272, 0));

        // door buttons
        Global.office.door_button_holder_l.SetPosition(Global.office.bg.GetPosition() + new Vector2(-8, 270));
        Global.office.door_button_l.SetPosition(Global.office.door_button_holder_l.GetPosition() + new Vector2(29, 52));
        Global.office.door_button_holder_r.SetPosition(Global.office.bg.GetPosition() + new Vector2(Global.office.bg.GetWidth() - 116, 270));
        Global.office.door_button_r.SetPosition(Global.office.door_button_holder_r.GetPosition() + new Vector2(23, 52));
    }

    private bool MouseOverCamBar()
    {
        return Global.office.camBar.GetBounds().Contains(MouseManager.Location);
    }
    private State CheckAction()
    {
        if (KeyboardManager.KeyPressed(Keys.A))
        {
            Global.office.door_L.Toggle();
        }
        if (KeyboardManager.KeyPressed(Keys.D))
        {
            Global.office.door_R.Toggle();
        }

        if (KeyboardManager.KeyPressed(Keys.S) 
            || (Global.office.camBar.visible && MouseOverCamBar()))
        {
            Global.office.camBar.visible = false;
            return Global.openCams;
        }
        else if (!Global.office.camBar.visible && !MouseOverCamBar())
        {
            Global.office.camBar.visible = true;
        }

        if (!MouseManager.LeftButtonClicked) return null;
        if (Global.office.door_button_l.GetBounds().Contains(MouseManager.Location)) Global.office.door_L.Toggle();
        if (Global.office.door_button_r.GetBounds().Contains(MouseManager.Location)) Global.office.door_R.Toggle();

        return null;
    }

    public override State Update()
    {
        UpdateView();
        return CheckAction();
    }
}
