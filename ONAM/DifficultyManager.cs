namespace ONAM;

public class DifficultyManager
{
    public int id { get; set; } = 0;
    public int power { get; set; } = 0;

    // [blue, red, yellow, green]
    public int[] level { get; set; } = [0, 0, 0, 0];
    public double[] startDelay { get; set; } = [0, 0, 0, 0];
    public double[] moveDelay { get; set; } = [0, 0, 0, 0];
    public double[] attackDelay { get; set; } = [0, 0, 0, 0];
    public double[] health { get; set; } = [0, 0, 0, 0];

    public double m_startDelay { get; set; } = 0;
    public int m_level { get; set; } = 0;
    public int numTillDeath { get; set; } = 0;

    public void Apply(Night n)
    {
        n.totalPower = power;

        for (int i = 0; i < n.mikus.Length; i++)
        {
            n.mikus[i].level = level[i];
            n.mikus[i].startDelay = startDelay[i];
            n.mikus[i].moveDelay = moveDelay[i];
            n.mikus[i].attackDelay = attackDelay[i];
            n.mikus[i].health = health[i];
        }

        n.office.mikulingManager.startDelay = m_startDelay;
        n.office.mikulingManager.level = m_level;
        n.office.mikulingManager.numTillDeath = numTillDeath;
    }

    public string DiffToString()
    {
        switch (id)
        {
            case 1:
                return "Easy";
            case 2:
                return "Medium";
            case 3:
                return "Hard";
            default:
                return "???";
        }
    }
}
