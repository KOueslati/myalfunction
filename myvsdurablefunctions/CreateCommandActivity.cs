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
    public static class CreateCommandActivity
    {
        [FunctionName("CreateCommand")]
        public static async Task<string> Run([ActivityTrigger] IDurableActivityContext context,
            [Table("commands")] IAsyncCollector<Command> commands,
            ILogger log)
        {
            var order = context.GetInput<Order>();

            if (order == null) return "Command Non Valid";

            var command = new Command
            {
                RowKey = order.OrderId.ToString(),
                PartitionKey = "commands",
                commandId = Guid.NewGuid().ToString(),
                userMail = order.User.Name,
                product = order.Product.ProductId.ToString(),
                price = order.Product.TotalTtc.ToString()
            };

            await commands.AddAsync(command);

            order.Status = "Command Created";
            return order.Status;
        }
    }
}
