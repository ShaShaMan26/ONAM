using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class SealingVent : State
{
    private SFXObject beep, close;

    private double counter;
    private Texture2D[] textures;
    private int textureIndex;

    public override void Initialize()
    {
        base.Initialize();

        beep = new(Global.content.Load<SoundEffect>("sfx/vent_beep"));
        beep.Volume = .4f;
        close = new(Global.content.Load<SoundEffect>("sfx/vent_close"));

        counter = 0;
        textureIndex = 0;

        textures = new Texture2D[4];
        for (int i = 0; i < textures.Length; i++)
        {
            textures[i] = Global.content.Load<Texture2D>("ani_vent_seal/" + i);
        }
    }

    public override State Update()
    {
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        Global.camView.seal_vent_bar.opacity = 0.6f;
        if (counter >= .84 || textureIndex < 1)
        {
            if (textureIndex >= textures.Length)
            {
                Global.camView.seal_vent_dots.visible = false;
                Global.camView.seal_vent_bar.opacity = 1;
                AudioManager.AddSFX(close);
                Global.camView.SealVent(Global.camNum - 5);
                Global.camView.seal_vent_bar_active.visible = true;
                return Global.inCams;
            }

            Global.camView.seal_vent_dots.SetTexture(textures[textureIndex]);
            if (textureIndex < 1) Global.camView.seal_vent_dots.visible = true;
            AudioManager.AddSFX(beep);
            textureIndex++;
            counter = 0;
        }

        return null;
    }
}
