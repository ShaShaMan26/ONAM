using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class EnterCams : TutorialState
{
    private int i;
    private double counter;
    private SFXObject cam_flip;


    public override void Initialize()
    {
        base.Initialize();

        i = 0;
        cam_flip = new(Global.content.Load<SoundEffect>("sfx/cam_flip"));

        nextState = new ViewCams();
        nextState.Initialize();
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override State Update()
    {
        if (!Global.night.office.camTablet.visible)
        {
            Global.night.office.camTablet.visible = true;
            AudioManager.AddSFX(cam_flip);
        }

        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter > Global.aniDelay || i == 0)
        {
            if(i >= Global.night.office.tabAni.Length)
            {
                i = 0;
                Global.night.office.camTablet.visible = false;
                Global.night.canvas = Global.night.camView;
                Global.night.camView.CauseFlicker();
                AudioManager.AddSFX(Global.night.camView.cam_switch);
                return nextState;
            }
            Global.night.office.camTablet.SetTexture(Global.night.office.tabAni[i]);
            i++;
            counter = 0;
        }

        return null;
    }
}
