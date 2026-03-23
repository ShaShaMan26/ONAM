namespace ONAM;

public class CloseCams : State
{
    private int i;
    private double counter;

    public override void Initialize()
    {
        base.Initialize();

        i = Global.office.tabAni.Length - 1;
    }

    public override State Update()
    {
        Global.office.camTablet.visible = true;
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
