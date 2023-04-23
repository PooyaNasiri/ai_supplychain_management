namespace SupplyChainManagementAI.Agents;

/*
 * This class is used to represent the manufacture agent.
 *  The manufacture agent is responsible for manufacturing the products.
 *  It has a capacity, which is the maximum number of units it can manufacture in a day.
 *  It has a cost per unit, which is the cost of manufacturing one unit.
 *  It has a provided, which is the number of units it has manufactured.
 *  It has a wasted, which is the number of units it has wasted because supplier provided but manufacture could not use them according to its capacity.
 *  The manufacture agent has a method called Act, which is called every day to manufacture the products and update its CpU, provided and wasted values accordingly based on the supplier's CpU and capacity.
 */

public class ManufactureAgent
{
    private static readonly Random Rnd = new();
    private int _capacity;
    public int CpU, Provided, Wasted;
    
    public void Act(SupplierAgent supplierAgent)
    {
        Wasted = Math.Max(supplierAgent.Capacity - _capacity, 0);
        Provided = Math.Min(_capacity, supplierAgent.Capacity);
        CpU += supplierAgent.CpU;
    }
    
    public void Set(int round, int maximumUints, int parts)
    {
        var lowRnd = maximumUints / parts * round + 1;
        var highRnd = maximumUints / parts * (round + 1);
        _capacity = new Random().Next(lowRnd, highRnd);
        CpU = Rnd.Next(lowRnd, highRnd);
    }
}