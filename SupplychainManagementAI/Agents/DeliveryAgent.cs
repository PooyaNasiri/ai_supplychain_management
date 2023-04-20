namespace SupplychainManagementAI.Agents;

public class DeliveryAgent
{
    private static readonly Random rnd = new();
    public int Capacity;
    public int cost;
    public int CpU;
    public int provided;

    public void Act(WarehouseAgent wa, int consumerDemand)
    {
        provided = Math.Min(Math.Min(Capacity, wa.UsedCapacity), consumerDemand);
        CpU += wa.CpU;
        cost = CpU * provided;
    }

    public void Set(int round, int maximumUints, int parts)
    {
        var lowRnd = maximumUints / parts * round + 1;
        var highRnd = maximumUints / parts * (round + 1);
        Capacity = rnd.Next(lowRnd, highRnd);
        CpU = rnd.Next(lowRnd, highRnd);
    }
}