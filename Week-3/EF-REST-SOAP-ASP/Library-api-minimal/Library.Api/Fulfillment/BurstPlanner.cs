using Library.Data.Entities;

namespace Library.Api.Fulfillment;

public class BurstPlanner
{
    
    //Method to plan fulfillment order
public IReadOnlyList<int> OrderByPriority(IEnumerable<Order> orders)
    {
        
        //We could make our own custom implementation on this - we won't 
        //We can use a PriorityQueue - allows for FIFO processing with priority taken into account
        //First int = OrderId, second int = Priority.
        //We are going yo use a Lower number = higher priority
        PriorityQueue<int, int> pq = new PriorityQueue<int, int>();

        foreach (Order o in orders)
            //Enqueue each order, if it's priority is expedited, give it a 0 value, if normal give it 1.
            pq.Enqueue(o.Id, o.Priority == Priority.Expedited ? 0 : 1);

        //This list will hold everithyng we want to process already in order to pass to our other methods
        var OrderedByPriority = new List<int>();

        //While out PriorityQueue has stuff in it - Loop and add those things in the order they exit
        //to our orderedByPriority list - uses out params
        while(pq.TryDequeue(out int id, out _))
        {
            OrderedByPriority.Add(id);
        }

        return OrderedByPriority;

    }
}