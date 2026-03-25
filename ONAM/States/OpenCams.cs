using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class OpenCams : State
{
    private int i;
    private double counter;
    private SFXObject cam_flip;


    public override void Initialize()
    {
        base.Initialize();

        i = 0;
        cam_flip = new(Global.content.Load<SoundEffect>("sfx/cam_flip"));
        cam_flip.Volume = .9f;
    }

    public override State Update()
    {
        if (!Global.office.camTablet.visible)
        {
            Global.office.camTablet.visible = true;
            AudioManager.AddSFX(cam_flip);
        }

        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter > .017 || i == 0)
        {
            if(i >= Global.office.tabAni.Length)
            {
                i = 0;
                Global.office.camTablet.visible = false;
                Global.canvas = Global.camView;
                Global.camView.CauseFlicker();
                AudioManager.AddSFX(Global.camView.cam_switch);
                return Global.inCams;
            }
            Global.office.camTablet.SetTexture(Global.office.tabAni[i]);
            i++;
            counter = 0;
        }

        return null;
    }
}
