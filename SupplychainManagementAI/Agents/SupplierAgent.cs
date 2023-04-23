namespace SupplyChainManagementAI.Agents;

/*
 * This class is used to represent the supplier agent.
 * The supplier agent is responsible for providing the products to the manufacture.
 *  It has a capacity, which is the maximum number of units it can provide in a day.
 *  It has a cost per unit, which is the cost of providing one unit.
 *  It has a provided, which is the number of units it has provided.
 *  The supplier agent has a method called Set, which is called every day to provide the products and update its CpU and capacity values accordingly.
 */

public class SupplierAgent
{
    private static readonly Random Rnd = new();
    public int Capacity;
    public int CpU;

    public void Set(int round, int maximumUints, int parts)
    {
        var lowRnd = maximumUints / parts * round + 1;
        var highRnd = maximumUints / parts * (round + 1);
        Capacity = new Random().Next(lowRnd, highRnd);
        CpU = Rnd.Next(lowRnd, highRnd);
    }
}