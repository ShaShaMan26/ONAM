namespace ONAM;

public class OpenCams : State
{
    private int i;
    private double counter;

    public override void Initialize()
    {
        base.Initialize();

        i = 0;
    }

    public override State Update()
    {
        Global.office.camTablet.visible = true;
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter > .017 || i == 0)
        {
            Global.office.camTablet.SetTexture(Global.office.tabAni[i]);
            i++;
            counter = 0;
            if(i >= Global.office.tabAni.Length)
            {
                i = 0;
                Global.office.camTablet.visible = false;
                Global.canvas = Global.camView;
                return Global.inCams;
            }
        }

        return null;
    }
}
