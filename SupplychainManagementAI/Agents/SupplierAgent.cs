namespace SupplychainManagementAI.Agents;

public class SupplierAgent
{
    private static readonly Random rnd = new();
    public int Capacity;
    public int CpU;

    public void Set(int round, int maximumUints, int parts)
    {
        var lowRnd = maximumUints / parts * round + 1;
        var highRnd = maximumUints / parts * (round + 1);
        Capacity = new Random().Next(lowRnd, highRnd);
        CpU = rnd.Next(lowRnd, highRnd);
    }
}