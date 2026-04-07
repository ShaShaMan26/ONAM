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
        Global.night.camView.seal_vent_bar.opacity = 0.6f;
        if (counter >= .5 || textureIndex < 1)
        {
            if (textureIndex >= textures.Length)
            {
                Global.night.camView.seal_vent_dots.visible = false;
                Global.night.camView.seal_vent_bar.opacity = 1;
                AudioManager.AddSFX(close);
                Global.night.camView.SealVent(Global.night.camNum - 5);
                Global.night.camView.seal_vent_bar_active.visible = true;
                return Global.night.inCams;
            }

            Global.night.camView.seal_vent_dots.SetTexture(textures[textureIndex]);
            if (textureIndex < 1) 
            {
                Global.night.camView.seal_vent_dots.visible = true;
                AudioManager.AddSFX(beep);
            }
            textureIndex++;
            counter = 0;
        }

        return null;
    }
}
