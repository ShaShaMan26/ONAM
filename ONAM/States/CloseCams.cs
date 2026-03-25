using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class CloseCams : State
{
    private int i;
    private double counter;
    private SFXObject cam_flip;

    public override void Initialize()
    {
        base.Initialize();

        i = Global.office.tabAni.Length - 1;
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
        Global.canvas = Global.office;
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter > .017 || i == Global.office.tabAni.Length - 1)
        {
            if(i < 0)
            {
                i = Global.office.tabAni.Length - 1;
                Global.office.camTablet.visible = false;
                return Global.inOffice;
            }
            Global.office.camTablet.SetTexture(Global.office.tabAni[i]);
            i--;
            counter = 0;
        }

        return null;
    }
}
