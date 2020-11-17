using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace myvsdurablefunctions
{
    public static class StarterCreateOrder
    {
        [FunctionName("ProcessOrder")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "Post")]
            HttpRequest req, 
            [DurableClient] IDurableClient starter, 
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            var requestMessage = await new StreamReader(req.Body).ReadToEndAsync();

            var order = JsonConvert.DeserializeObject<Order>(requestMessage);

            var instanceId = await starter.StartNewAsync("OrchestratorCreateOrder", order);


            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
