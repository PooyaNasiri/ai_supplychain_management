namespace SupplychainManagementAI;

public class State
{
    public int consumerDemand, provided, cpu, waMa, waWa, cost, days, waCa;
    public State? parent;

    public State()
    {
        days = 0;
    }

    public State(int consumerDemand, int cost, int provided, int cpu, int waMa, int waWa, int waCa, State parent)
    {
        this.consumerDemand = consumerDemand;
        this.parent = parent;
        days = parent.days + 1;
        this.cost = cost;
        this.provided = provided;
        this.cpu = cpu;
        this.waMa = waMa;
        this.waWa = waWa;
        this.waCa = waCa;
    }
}