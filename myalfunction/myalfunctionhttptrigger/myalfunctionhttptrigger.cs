using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared;

namespace myalfunction
{
    public static class myalfunctionhttptrigger
    {
        [FunctionName("myalfunctionhttptrigger")]
        
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", "get", Route = null)] HttpRequest req,
            [Queue("my-queue1")] ICollector<Command> msg,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Command data = JsonConvert.DeserializeObject<Command>(requestBody);
            msg.Add(data);
           
            return new OkObjectResult(data);
        }
    }
}
