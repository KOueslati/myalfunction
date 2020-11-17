using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace myvsdurablefunctions
{
    public static class OrchestratorCreateOrder
    {
        [FunctionName("OrchestratorCreateOrder")]
        public static async Task<string> Run([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            
            var order = context.GetInput<Order>();

            log.LogInformation($"Start create order {order.OrderId}");
            
            var status = await context.CallActivityAsync<string>("CreateOrder", order);
            if (!string.IsNullOrEmpty(status))
            {
                if (status.Equals("Order Created"))
                {
                    await context.CallActivityAsync<string>("CreateCommand", order);
                    var paymentId = await context.CallActivityAsync<string>("CreatePayment", order.OrderId);
                    if (!string.IsNullOrEmpty(paymentId))
                    {
                        status = await context.CallActivityAsync<string>("ValidateCommand", order.OrderId);
                    }
                }
                return status;
            }

            return "Invalid Operation, please contact...";
        }
    }
}
