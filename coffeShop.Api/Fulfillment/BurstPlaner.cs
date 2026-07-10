using coffeShop.data.entities;
using coffeShop.data.Entities;

namespace coffeShop.Api.Fulfillment;

public class BurstPlaner
{
    
    public IReadOnlyList<int> OrderByPriority(IEnumerable<Order> orders)
    {
        
        PriorityQueue<int, int> pq = new PriorityQueue<int, int>();

        foreach (Order o in orders) //Itereates thorug the incoming orders
        {
            pq.Enqueue(o.OrderId, o.Priority == Priority.VIP ? 0 : 1);  //if an order is VIP 0 (higher priority) is not 1 (normal)
        }

        var orderedByPriority = new List<int>(); //Create a list with the VIP orders on the top

        while(pq.TryDequeue(out int id, out _))
        {

            orderedByPriority.Add(id); //Draind the priorit queue and extract the ids in the correct order
        }

        return orderedByPriority;
    }

}