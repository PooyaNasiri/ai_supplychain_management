namespace SupplyChainManagementAI.Agents;

/*
 * This class is used to represent the warehouse agent.
 *  The warehouse agent is responsible for storing the products.
 *  It has a capacity, which is the maximum number of units it can store.
 *  It has a cost per unit, which is the cost of storing one unit.
 *  It has a used capacity, which is the number of units it has stored.
 *  It has a wasted, which is the number of units it has wasted because manufacture provided but warehouse could not store them according to its capacity.
 *  The warehouse agent has a method called Act, which is called every day to store the products and update its CpU, used capacity and wasted values accordingly based on the manufacture's CpU and provided.
 */

public class WarehouseAgent
{
    private static readonly Random Rnd = new();
    public int CpU;
    public int UsedCapacity;
    public int Wasted;

    public WarehouseAgent(int usedCapacity)
    {
        UsedCapacity = usedCapacity;
    }

    public void Act(ManufactureAgent manufactureAgent, int max, State yesterday)
    {
        UsedCapacity -= yesterday.Provided;
        Wasted = Math.Max(0, manufactureAgent.Provided + UsedCapacity - max);
        UsedCapacity = Math.Min(max, manufactureAgent.Provided + yesterday.WarehouseCapacity);
        CpU += manufactureAgent.CpU;
    }


    public void Set(int round, int maximumUints, int parts)
    {
        var lowRnd = maximumUints / parts * round + 1;
        var highRnd = maximumUints / parts * (round + 1);
        CpU = Rnd.Next(lowRnd, highRnd);
    }
}