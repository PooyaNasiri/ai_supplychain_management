namespace SupplychainManagementAI.Agents;

public class WarehouseAgent
{
    private static readonly Random rnd = new();
    public int CpU;
    public int UsedCapacity;
    public int Wasted;

    public WarehouseAgent(int usedCapacity)
    {
        UsedCapacity = usedCapacity;
    }

    public void Act(ManufactureAgent ma, int max, State yesterday)
    {
        UsedCapacity -= yesterday.provided;
        Wasted = Math.Max(0, ma.Provided + UsedCapacity - max);
        // Console.WriteLine("ma.Provided : "+ ma.Provided + "   UsedCapacity " + UsedCapacity +  " wawa " + Wasted);
        UsedCapacity = Math.Min(max, ma.Provided + yesterday.waCa);
        CpU += ma.CpU;
    }


    public void Set(int round, int maximumUints, int parts)
    {
        var lowRnd = maximumUints / parts * round + 1;
        var highRnd = maximumUints / parts * (round + 1);
        CpU = rnd.Next(lowRnd, highRnd);
    }
}