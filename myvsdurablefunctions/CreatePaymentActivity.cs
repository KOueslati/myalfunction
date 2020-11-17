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
    public static class CreatePaymentActivity
    {
        [FunctionName("CreatePayment")]
        public static async Task<string> Run([ActivityTrigger] IDurableActivityContext context,
            IBinder binder,
            [Table("payments")]IAsyncCollector<Payment> payments,
            ILogger log)
        {
            var orderId = context.GetInput<Guid>().ToString();
            if (!string.IsNullOrEmpty(orderId))
            {
                var command = await binder.BindAsync<Command>(new TableAttribute("commands", "commands", orderId));
                if (command != null)
                {
                    var payment = new Payment()
                    {
                        PaymentId = Guid.NewGuid(),
                        RowKey = command.RowKey,
                        PartitionKey = "payments",
                        Amount = double.Parse(command.price),
                        PaymentDate = DateTime.UtcNow
                    };
                    await payments.AddAsync(payment);

                    return payment.PaymentId.ToString();
                }
            }

            return null;
        }
    }
}
