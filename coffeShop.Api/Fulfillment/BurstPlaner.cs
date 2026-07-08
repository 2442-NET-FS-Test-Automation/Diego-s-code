using coffeShop.data.entities;
using coffeShop.data.Entities;

namespace coffeShop.Api.Fulfillment;

public class BurstPlaner
{
    
    public IReadOnlyList<int> OrderByPriority(IEnumerable<Order> orders)
    {
        
        PriorityQueue<int, int> pq = new PriorityQueue<int, int>();

        foreach (Order o in orders)
        {
            pq.Enqueue(o.OrderId, o.Priority == Priority.Expedited ? 0 : 1); 
        }

        var orderedByPriority = new List<int>();

        while(pq.TryDequeue(out int id, out _))
        {

            orderedByPriority.Add(id);
        }

        return orderedByPriority;
    }

}