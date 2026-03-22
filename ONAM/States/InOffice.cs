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

        leftBound = Global.graphics.PreferredBackBufferWidth / 4;
        rightBound = Global.graphics.PreferredBackBufferWidth - Global.graphics.PreferredBackBufferWidth / 4;
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
            && Global.office.bg.GetPosition().X > -Global.office.bg.GetWidth() + Global.graphics.PreferredBackBufferWidth) 
        {
            Global.office.bg.SetPosition(
                Global.office.bg.GetPosition().X - a, Global.office.bg.GetPosition().Y
                );
            
            if (Global.office.bg.GetPosition().X < -Global.office.bg.GetWidth() + Global.graphics.PreferredBackBufferWidth)
            {
                Global.office.bg.SetPosition(-Global.office.bg.GetWidth() + 
                    Global.graphics.PreferredBackBufferWidth, 0);
            }
        }
    }

    private bool MouseOverCamBar()
    {
        return Global.office.camBar.GetBounds().Contains(MouseManager.Location);
    }
    private State CheckAction()
    {
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

        return null;
    }

    public override State Update()
    {
        UpdateView();
        return CheckAction();
    }
}
