namespace ONAM;

public class BlueMiku : Miku
{
    public BlueMiku() : base("miku")
    {
        level = 1;
        moveDelay = 3.5;
        startDelay = 30;
    }

    public override void MakeMove()
    {
        counter = progress;
        if (progress == 2) 
            progress = r.Next(3, 5);
        else progress++;
        if (progress > 4 || (counter == 3 && progress == 4)) progress = 1;   
        if (Global.camNum == progress || Global.camNum == counter) 
            Global.camView.InterruptCam(Global.camNum);
    }
}
