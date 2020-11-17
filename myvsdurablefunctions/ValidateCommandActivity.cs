using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Shared;

namespace myvsdurablefunctions
{
    public static class ValidateCommandActivity
    {
        [FunctionName("ValidateCommand")]
        public static async Task<string> Run([ActivityTrigger] string orderId,
            IBinder binder,
            [Table("orders")] IAsyncCollector<Order> orders,
            ILogger log)
        {
            var order = await binder.BindAsync<Order>(new TableAttribute("orders","orders", orderId));

            if (order == null) return "Command Non Validate! Please contact administrator...";

            order.Status = "Command Validated";
          
            return order.Status;

        }
    }
}
