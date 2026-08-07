using System;
using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class Interesting : Scene
{
    private SFXObject speech;
    private Miku goldie;

    public override void Initialize()
    {
        speech = new(Global.content.Load<SoundEffect>("sfx/eyes"));
        goldie = new("gold-miku");
        goldie.SetDimensions(1500, 1499);
        goldie.SetPosition(Global.renderTarget.Width / 2 - goldie.GetWidth() / 2,
            -225);

        AudioManager.PauseBGM();
        AudioManager.PauseSFXAll();
        AudioManager.AddSFX(speech);
    }

    public override void OnStart()
    {
        
    }

    public override Scene Update()
    {
        if (speech.PlaybackClosed) Environment.Exit(0);
        return null;
    }

    public override void Draw()
    {
        goldie.Draw();
    }
}
