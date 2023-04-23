/*
 * Supply Chain Management AI
 *  - A simulation of a supply chain management system using AI
 *  - The program is a part of the AI course at the University of Padova (Spring 2023)
 *  - It is written in C# using .NET 6
 */

using System.Diagnostics;
using SupplyChainManagementAI;
using SupplyChainManagementAI.Agents;

LinkedList<State> states = new(); // a linked list of all the states
State? currentState = new();  // the current state
Random rnd = new(); // a random number generator
Stopwatch watch = new(); // a stopwatch to measure the time
SupplierAgent supplierAgent = new(); // the supplier agent
ManufactureAgent manufactureAgent = new(); // the manufacture agent
WarehouseAgent warehouseAgent = new(0); // the warehouse agent
DeliveryAgent deliveryAgent = new(); // the delivery agent
const int possibilities = 3, maximumUints = 100, days = 200; // the number of possibilities for each agent, the maximum number of units that can be produced and the number of days to simulate
int calculatedStates = 0; // the maximum number of states calculated

Console.WriteLine("Calculating... Please wait.");
watch.Restart(); // start the stopwatch
while (currentState.Days < days) // while the current state is not the last day
{
    int consumerDemand = rnd.Next(60, 90); // generate a random consumer demand
    // Console.WriteLine("days : " + currentState.days + " scunt: " + states.Count);
    for (int iSa = 0; iSa < possibilities; iSa++) // for each possibility of the supplier agent
    {
        supplierAgent.Set(iSa, maximumUints, possibilities); // set the supplier agent's new state
        for (int iMa = 0; iMa < possibilities; iMa++) // for each possibility of the manufacture agent
        {
            manufactureAgent.Set(iMa, maximumUints, possibilities); // set the manufacture agent's new state
            manufactureAgent.Act(supplierAgent); // let the manufacture agent act according to the supplier agent's state
            for (int iWa = 0; iWa < possibilities; iWa++) // for each possibility of the warehouse agent
            { 
                warehouseAgent.Set(iWa, maximumUints, possibilities); // set the warehouse agent's new state
                warehouseAgent.Act(manufactureAgent, maximumUints, currentState); // let the warehouse agent act according to the manufacture agent's state
                for (int iDa = 0; iDa < possibilities; iDa++) // for each possibility of the delivery agent
                {
                    deliveryAgent.Set(iDa, maximumUints, possibilities); // set the delivery agent's new state
                    deliveryAgent.Act(warehouseAgent, consumerDemand); // let the delivery agent act according to the warehouse agent's state
                    KeepOnlyN(currentState.Days + 1, days + 1); // keep only the (day+1) best states for the next day
                    states.AddLast(new State(consumerDemand, deliveryAgent.Cost, deliveryAgent.Provided, // add the new state to the list of states
                        deliveryAgent.CpU, manufactureAgent.Wasted, warehouseAgent.Wasted, warehouseAgent.UsedCapacity,
                        currentState));
                }
            }
        }
    }
    
    if (states.Count > 0) // if the list of states is not empty
    {
        // set the current state to the first state in the list and remove it from the list
        currentState = states.First(); 
        states.RemoveFirst(); 
    }
    else // if the list of states is empty
    { 
        // set the current state to null and break the loop
        currentState = null; 
        break; 
    }

    calculatedStates++; // increment the number of states calculated
}

watch.Stop(); // stop the stopwatch
State? bestState = states.Count > 0 
    ? states.Aggregate((minState, state) => Math.Max(0, state.ConsumerDemand - state.Provided) < Math.Max(0, minState.ConsumerDemand - minState.Provided) ? state : minState) : currentState;  // find the best state in the remaining list of states according to the hunger

if (bestState != null) // if there is a best state (if there is a way to satisfy the consumer demand for all the days of the simulation)
{
    long hunger = 0, units = 0, cost = 0, warehouseWaste = 0, manufactureWaste = 0, cpu = 0; 
    do // while the best state has a parent (while the best state is not the first state) calculate the total hunger, total units provided, total cost, total warehouse waste, total manufacture waste and average cost per unit
    { 
        hunger += Math.Max(0, bestState.ConsumerDemand - bestState.Provided);
        units += bestState.Provided;
        cost += bestState.Cost;
        warehouseWaste += bestState.WarehouseWaste;
        manufactureWaste += bestState.ManufactureWaste;
        cpu += bestState.CpU;
        bestState = bestState.Parent; // set the best state to its parent (the previous day)
    } while (bestState.Parent != null);

    Console.WriteLine("\n - States calculated: " + calculatedStates +
                      "\n - Time  elapsed: " + watch.ElapsedMilliseconds + "ms" +
                      "\n--------------------------------" +
                      "\n After " + days + " days :" +
                      "\n - Total hunger : " + hunger +
                      "\n - Total units provided : " + units +
                      "\n - Avg. Cost per unit : " + cpu / days +
                      "\n - Total cost : " + cost +
                      "\n - Total manufacture waste : " + manufactureWaste +
                      "\n - Total warehouse waste : " + warehouseWaste);
}
else
    Console.WriteLine("No way!"); 


void KeepOnlyN(int day, int n) 
{
    while (true)
    {
        int sameDayStates = 0;
        int worstCost = 0, worstHunger = 0, worstWaste = 0;
        State? worstHungerState = null, worstCostState = null, worstWasteState = null;
        LinkedListNode<State>? currentNode = states.First;
        // find the worst state in terms of hunger, waste and cost among all the states of the current day
        while (currentNode != null) 
        {
            State state = currentNode.Value;
            if (state.Days == day)
            {
                sameDayStates++;

                ////////////////////////find worst hunger
                int hunger = Math.Max(0, state.ConsumerDemand - state.Provided);
                if (hunger > worstHunger)
                {
                    worstHunger = hunger;
                    worstHungerState = state;
                }

                ////////////////////////find worst waste
                int waste = state.WarehouseWaste + state.ManufactureWaste;
                if (waste > worstWaste)
                {
                    worstWaste = waste;
                    worstWasteState = state;
                }

                ////////////////////////find worst cost
                if (state.Cost > worstCost)
                {
                    worstCost = state.Cost;
                    worstCostState = state;
                }
            }

            currentNode = currentNode.Next;
        }

        if (sameDayStates > n) // if there are more than n states of the current day, remove the worst state, else break the loop
            states.Remove(worstHungerState ?? worstWasteState ?? worstCostState);
        else
            return;
    }
}