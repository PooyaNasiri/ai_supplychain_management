namespace SupplyChainManagementAI;

/* 
 *  This class is used to represent a state in the search tree.
 *  A state is a snapshot of the system at a given time, which in this case is a day.
 *  It contains all the information about the state, and a reference to the parent state.
 *  The parent state is used to trace back the path from the goal state to the root state.
 *  The root state is the state where the search started, with no parent, days = 0, no cost, no provided, no waste, etc.
 *  The goal state is the state where the consumer demand is satisfied and waste and cost are minimized.
 */


public class State
{
    public readonly int ConsumerDemand, Provided, CpU, ManufactureWaste, WarehouseWaste, Cost, Days, WarehouseCapacity; // CpU = Cost per uni
    public readonly State? Parent;

    public State()
    {
        Days = 0;
    }

    public State(int consumerDemand, int cost, int provided, int cpU, int manufactureWaste, int warehouseWaste,
        int warehouseCapacity, State parent)
    {
        ConsumerDemand = consumerDemand;
        Parent = parent;
        Days = parent.Days + 1;
        Cost = cost;
        Provided = provided;
        CpU = cpU;
        ManufactureWaste = manufactureWaste;
        WarehouseWaste = warehouseWaste;
        WarehouseCapacity = warehouseCapacity;
    }
}