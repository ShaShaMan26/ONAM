namespace ONAM;

public class RedMiku : Miku
{
    public RedMiku() : base("miku")
    {
        level = 5;
        startDelay = 10;
    }

    public override void MakeMove()
    {
        counter = progress;
        progress++;
        if (progress > 3)
        {
            progress = 1;
        }
        if (Global.camNum == progress || Global.camNum == counter) 
            Global.camView.InterruptCam(Global.camNum);
    }
}
