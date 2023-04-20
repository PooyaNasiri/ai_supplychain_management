namespace SupplychainManagementAI.Agents;

public class ManufactureAgent
{
    private static readonly Random rnd = new();
    public int Capacity;
    public int CpU;
    public int Provided;
    public int Wasted;

    public void Act(SupplierAgent sa)
    {
        Wasted = Math.Max(sa.Capacity - Capacity, 0);
        Provided = Math.Min(Capacity, sa.Capacity);
        CpU += sa.CpU;
    }


    public void Set(int round, int maximumUints, int parts)
    {
        var lowRnd = maximumUints / parts * round + 1;
        var highRnd = maximumUints / parts * (round + 1);
        Capacity = new Random().Next(lowRnd, highRnd);
        CpU = rnd.Next(lowRnd, highRnd);
    }
}