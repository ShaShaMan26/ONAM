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
            basePos = Global.night.office.jumpscarePNG.GetPosition();
            Global.night.office.jumpscarePNG.visible = true;
            AudioManager.CloseSFXAll();
            AudioManager.AddSFX(sfx);
            AudioManager.PauseBGM();
        }
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter > 1.5)
        {
            Global.gameOver = new();
            Global.gameOver.Initialize();
            Global.sceneManager.currScene = Global.gameOver;
        }
        if (counter - prevCounter > .021)
        {
            offset = new Vector2(r.Next(-1, 2), r.Next(-1, 2)) * intensity;
            Global.night.office.jumpscarePNG.SetPosition(basePos + offset);
            prevCounter = counter;
        }

        return null;
    }
}
