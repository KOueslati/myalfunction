namespace myalfunction
{
    using System.IO;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public static class myalfunctionblobtrigger
    {
        [FunctionName("myalfunctionblobtrigger")]
        public static async Task Run([BlobTrigger("my-blob1/{name}.json")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation("C# BLOB trigger function processed a request.");

            var stream = new StreamReader(myBlob);
            var json = await stream.ReadToEndAsync();

            log.LogInformation($"C# file {name} output data {json} processed");
        }
    }
}