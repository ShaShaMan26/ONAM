using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class ExitCams : TutorialState
{
    private int i;
    private double counter2;
    private SFXObject cam_flip;

    public override void Initialize()
    {
        base.Initialize();

        i = Global.night.office.tabAni.Length - 1;
        cam_flip = new(Global.content.Load<SoundEffect>("sfx/cam_flip"));

        counter2 = 0;

        // nextState = new Goodbye();
        nextState = new Doors();
        nextState.Initialize();
    }

    public override State Update()
    {
        if (!Global.night.office.camTablet.visible)
        {
            Global.night.office.camTablet.visible = true;
            AudioManager.AddSFX(cam_flip);
            Global.night.canvas = Global.night.office;
        }
        counter2 += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter2 > Global.aniDelay || i == Global.night.office.tabAni.Length - 1)
        {
            if(i < 0)
            {
                i = Global.night.office.tabAni.Length - 1;
                Global.night.office.camTablet.visible = false;
                // nextState.OnStart();
                return nextState;
            }
            Global.night.office.camTablet.SetTexture(Global.night.office.tabAni[i]);
            i--;
            counter2 = 0;
        }

        return null;
    }
}
