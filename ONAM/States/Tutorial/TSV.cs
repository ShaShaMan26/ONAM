using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class TSV : SealingVent
{
    private TutorialState prevState;

    public override void Initialize()
    {
        base.Initialize();

        beep = new(Global.content.Load<SoundEffect>("sfx/vent_beep"));
        beep.Volume = ModifierManager.autoSeal ? .1f : .3f;
        close = new(Global.content.Load<SoundEffect>("sfx/vent_close"));
        close.Volume = ModifierManager.autoSeal ? .1f : .8f;

        counter = 0;
        textureIndex = 0;

        textures = new Texture2D[4];
        for (int i = 0; i < textures.Length; i++)
        {
            textures[i] = Global.content.Load<Texture2D>("ani_vent_seal/" + i);
        }
    }

    public void OnStart(TutorialState prevState)
    {
        this.prevState = prevState;
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

                foreach (CamButton b in Global.night.camView.camButtons) b.opacity = 1;
                Global.night.camView.camBar.opacity = 1;

                return prevState;
            }

            Global.night.camView.seal_vent_dots.SetTexture(textures[textureIndex]);
            if (textureIndex < 1) 
            {
                Global.night.camView.seal_vent_dots.visible = true;

                foreach (CamButton b in Global.night.camView.camButtons) b.opacity = .5f;
                Global.night.camView.camBar.opacity = .5f;

                AudioManager.AddSFX(beep);
            }
            textureIndex++;
            counter = 0;
        }

        return null;
    }
}
