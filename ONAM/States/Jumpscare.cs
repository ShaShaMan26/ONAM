using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class Jumpscare : State
{
    private Random r;
    private double counter, prevCounter;
    private Vector2 basePos, offset;
    private int intensity;

    private SFXObject sfx;

    public override void Initialize()
    {
        base.Initialize();
        r = new();
        counter = 0;
        prevCounter = 0;

        intensity = 6;

        sfx = new(Global.content.Load<SoundEffect>("sfx/jumpscare"));
        sfx.Volume = 0.4f;
    }

    public override State Update()
    {
        if (counter <= 0)
        {
            basePos = Global.office.jumpscarePNG.GetPosition();
            Global.office.jumpscarePNG.visible = true;
            AudioManager.AddSFX(sfx);
        }
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter > 1.5)
        {
            Global.office.jumpscarePNG.visible = false;
            // Global.Initialize();
            return Global.inOffice;
        }
        if (counter - prevCounter > .021)
        {
            offset = new Vector2(r.Next(-1, 2), r.Next(-1, 2)) * intensity;
            Global.office.jumpscarePNG.SetPosition(basePos + offset);
            prevCounter = counter;
        }

        return null;
    }
}
