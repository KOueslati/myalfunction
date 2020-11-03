using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared;

namespace myalfunction
{
    public static class myalfunctionqueuetrigger
    {
        [FunctionName("myalfunctionqueuetrigger")]
        public static async Task Run([QueueTrigger("my-queue1", Connection = "AzureWebJobsStorage")]Command command,
        IBinder binder,
         ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed");
            var text = JsonConvert.SerializeObject(command);
            var myblob = await binder.BindAsync<TextWriter>(new BlobAttribute($"my-blob1/{command.userId}_{command.product}.json",
            FileAccess.Write));
            myblob.WriteLine(text);

            log.LogInformation($"C# Blob output data {text}  processed");
        }
    }
}
