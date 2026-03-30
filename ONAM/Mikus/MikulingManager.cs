namespace ONAM;

public class MikulingManager
{
    private Mikuling[] mikulings;

    public MikulingManager()
    {
        
    }

    public void Initialize()
    {
        mikulings = new Mikuling[16];

        for (int i = 0; i < mikulings.Length; i++)
        {
            mikulings[i] = new();
            mikulings[i].Initialize();
        }
    }

    private void CheckInterations()
    {
        if (Global.stateManager.currState == Global.inOffice && MouseManager.RightButtonClicked)
        {   
            foreach (Mikuling m in mikulings)
            {
                if (m.GetBounds().Contains(MouseManager.Location))
                {
                    // m.reset();
                }
            }
        }
    }

    public void Update()
    {
        CheckInterations();
    }

    // public override void Draw()
    // {
    //     // base.Draw();
    // }
}
