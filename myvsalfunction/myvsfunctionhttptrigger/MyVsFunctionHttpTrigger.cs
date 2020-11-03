using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared;
using Microsoft.Azure.Documents;

namespace myvsalfunction.myvsfunctionhttptrigger
{
    public static class MyVsFunctionHttpTrigger
    {
        [FunctionName("myvsfunctionhttptrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "commands/{partitionKey}/{id}")] HttpRequest req,
            [CosmosDB("mycosmosdbapisql", 
                    "commands", 
                    ConnectionStringSetting = "MyCosmosDBConnectionString",
                    Id = "{id}",
                    PartitionKey = "{partitionKey}")] Command command,
            [CosmosDB("mycosmosdbapisql",
                    "commands",
                    ConnectionStringSetting = "MyCosmosDBConnectionString",
                    CreateIfNotExists = true)] IAsyncCollector<Command> myCommands,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (command != null)
            {
                log.LogInformation($"Document {command.Id} is retrieve from Database");
                return new OkObjectResult($"An existing command {command.commandId} is founded");
            }

            var newCommandAsStringAsync = await new StreamReader(req.Body).ReadToEndAsync();

            var newCommand = JsonConvert.DeserializeObject<Command>(newCommandAsStringAsync);

            await myCommands.AddAsync(newCommand);

            return new OkObjectResult(newCommand);
        }
    }
}
