using System.Diagnostics;
using SupplychainManagementAI;
using SupplychainManagementAI.Agents;

LinkedList<State> states = new();
State? currentState = new();
Random rnd = new();
Stopwatch watch = new();
SupplierAgent supplierAgent = new();
ManufactureAgent manufactureAgent = new();
WarehouseAgent warehouseAgent = new(0);
DeliveryAgent deliveryAgent = new();
const int parts = 3, maximumUints = 100, days = 365;
var maxStateCount = 0;

watch.Restart();
while (currentState.days < days)
{
    var consumerDemand = rnd.Next(60, 90);
    // Console.WriteLine("days : " + currentState.days + " scunt: " + states.Count);
    for (var iSa = 0; iSa < parts; iSa++)
    {
        supplierAgent.Set(iSa, maximumUints, parts);
        for (var iMa = 0; iMa < parts; iMa++)
        {
            manufactureAgent.Set(iMa, maximumUints, parts);
            manufactureAgent.Act(supplierAgent);
            for (var iWa = 0; iWa < parts; iWa++)
            {
                warehouseAgent.Set(iWa, maximumUints, parts);
                warehouseAgent.Act(manufactureAgent, maximumUints, currentState);
                for (var iDa = 0; iDa < parts; iDa++)
                {
                    deliveryAgent.Set(iDa, maximumUints, parts);
                    deliveryAgent.Act(warehouseAgent, consumerDemand);
                    KeepOnlyN(currentState.days + 1, days + 1);
                    states.AddLast(new State(consumerDemand, deliveryAgent.cost, deliveryAgent.provided,
                        deliveryAgent.CpU, manufactureAgent.Wasted, warehouseAgent.Wasted, warehouseAgent.UsedCapacity,
                        currentState));
                }
            }
        }
    }

    if (states.Count > 0)
    {
        currentState = states.First();
        states.RemoveFirst();
    }
    else
    {
        currentState = null;
        break;
    }

    maxStateCount++;
}

watch.Stop();
var bestState = states.Count > 0
    ? states.Aggregate((minState, state) => state.cost < minState.cost ? state : minState)
    : currentState ?? null;

if (bestState != null)
{
    var winnerState = bestState;
    long hunger = 0, units = 0, cost = 0, waWaste = 0, maWaste = 0, cpu = 0;
    do
    {
        hunger += Math.Max(0, winnerState.consumerDemand - winnerState.provided);
        units += winnerState.provided;
        cost += winnerState.cost;
        waWaste += winnerState.waWa;
        maWaste += winnerState.waMa;
        cpu += winnerState.cpu;
        // Console.WriteLine("demand: " + winnerState.consumerDemand + " winnerState.provided " + winnerState.provided+ " hunger " + Math.Max(0, winnerState.consumerDemand - winnerState.provided));
        winnerState = winnerState.parent;
    } while (winnerState.parent != null);

    Console.WriteLine("\n - States calculated: " + maxStateCount +
                      "\n - Time  elapsed: " + watch.ElapsedMilliseconds + "ms" +
                      "\n--------------------------------" +
                      "\n After " + days + " days :" +
                      "\n - Total hunger : " + hunger +
                      "\n - Total units provided : " + units +
                      "\n - Avg. Cost per unit : " + cpu / days +
                      "\n - Total cost : " + cost +
                      "\n - Total manufacture waste : " + maWaste +
                      "\n - Total warehouse waste : " + waWaste);
}
else
{
    Console.WriteLine("No way!");
}

void KeepOnlyN(int day, int n)
{
    while (true)
    {
        var sameDayStates = 0;
        int worstCost = 0, worstHunger = 0, worstWaste = 0;
        State? worstHungerState = null, worstCostState = null, worstWasteState = null;
        var currentNode = states.First;
        while (currentNode != null)
        {
            var state = currentNode.Value;
            if (state.days == day)
            {
                sameDayStates++;

                ////////////////////////find worst hunger
                var hunger = Math.Max(0, state.consumerDemand - state.provided);
                if (hunger > worstHunger)
                {
                    worstHunger = hunger;
                    worstHungerState = state;
                }

                ////////////////////////find worst waste
                var waste = state.waWa + state.waMa;
                if (waste > worstWaste)
                {
                    worstWaste = waste;
                    worstWasteState = state;
                }

                ////////////////////////find worst cost
                if (state.cost > worstCost)
                {
                    worstCost = state.cost;
                    worstCostState = state;
                }
            }

            currentNode = currentNode.Next;
        }

        if (sameDayStates > n)
            states.Remove(worstHungerState ?? worstWasteState ?? worstCostState);
        else
            return;
    }
}