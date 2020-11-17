using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.OData;
using Shared;

namespace myvsdurablefunctions
{
    public static class CreateOrderActivity
    {
        [FunctionName("CreateOrder")]
        public static async Task<string> Run([ActivityTrigger] IDurableActivityContext context,
            [Table("orders")] IAsyncCollector<Order> orders,
            ILogger log)
        {
            var order = context.GetInput<Order>();
            
            if (order == null) return "Command Non Valid";

            order.RowKey = order.OrderId.ToString();
            order.PartitionKey = "orders";
            order.Status = "Order Created";
            await orders.AddAsync(order);

            return order.Status;
        }
    }
}
