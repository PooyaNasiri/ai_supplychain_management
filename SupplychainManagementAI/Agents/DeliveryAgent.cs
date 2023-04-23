namespace SupplyChainManagementAI.Agents;

/*
 * This class is used to represent the delivery agent.
 *  The delivery agent is responsible for delivering the products from the warehouse to the consumer.
 *  It has a capacity, which is the maximum number of units it can deliver in a day.
 *  It has a cost per unit, which is the cost of delivering one unit.
 *  It has a cost, which is the total cost of delivering the units it has delivered.
 *  It has a provided, which is the number of units it has delivered.
 *  The delivery agent has a method called Act, which is called every day to deliver the products
 *    from the warehouse to the consumer and update its cost and provided values accordingly based on the warehouse's CpU and used capacity.
 */

public class DeliveryAgent
{
    private static readonly Random Rnd = new();
    private int _capacity;
    public int Cost;
    public int CpU;
    public int Provided;

    public void Act(WarehouseAgent wa, int consumerDemand)
    {
        Provided = Math.Min(Math.Min(_capacity, wa.UsedCapacity), consumerDemand);
        CpU += wa.CpU;
        Cost = CpU * Provided;
    }

    public void Set(int round, int maximumUints, int parts)
    {
        var lowRnd = maximumUints / parts * round + 1;
        var highRnd = maximumUints / parts * (round + 1);
        _capacity = Rnd.Next(lowRnd, highRnd);
        CpU = Rnd.Next(lowRnd, highRnd);
    }
}