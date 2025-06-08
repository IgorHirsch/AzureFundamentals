using AzureTangyFunc.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace AzureTangyFunc
{
    public class FunctionOutput
    {
        [QueueOutput("SalesRequestInBound", Connection = "AzureWebJobsStorage")]
        public SalesRequest? SalesRequest { get; set; }

        public HttpResponseData? HttpResponse { get; set; }
    }

    public static class OnSalesUploadWriteToQueue
    {
        [Function("OnSalesUploadWriteToQueue")]
        public static async Task<FunctionOutput> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext context)
        {
            var logger = context.GetLogger("OnSalesUploadWriteToQueue");
            logger.LogInformation("Sales Request received.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var salesRequest = JsonConvert.DeserializeObject<SalesRequest>(requestBody);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"Sales Request received for {salesRequest?.Name}.");

            if (salesRequest == null)
            {
                logger.LogError("Failed to deserialize SalesRequest.");
            }
            else
            {
                logger.LogInformation($"SalesRequest.Name = {salesRequest.Name}");
            }
            return new FunctionOutput
            {
                SalesRequest = salesRequest,
                HttpResponse = response
            };
        }
    }
}
