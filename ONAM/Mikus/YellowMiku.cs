namespace ONAM;

public class YellowMiku : Miku
{
    public YellowMiku() : base("miku")
    {
        level = 4;
        startDelay = 12;
    }

    public override void MakeMove()
    {
        counter = progress;
        progress++;
        if (progress == 3) progress = 4;
        else if (progress > 4) progress = 1;
        if (Global.camNum == progress || Global.camNum == counter) 
            Global.camView.InterruptCam(Global.camNum);
    }
}
