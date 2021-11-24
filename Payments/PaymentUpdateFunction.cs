using System.IO;
using System.Net;
using System.Threading.Tasks;
using AzureIntro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace AzureIntro.Payments
{
    public class PaymentUpdateFunction
    {
        private readonly IPaymentQueueProcessor _paymentQueueProcessor;

        public PaymentUpdateFunction(IPaymentQueueProcessor paymentQueueProcessor)
        {
            this._paymentQueueProcessor = paymentQueueProcessor;
        }

        
        [OpenApiOperation(operationId: "Run", tags: new[] { "PaymentInformation" })]
        [OpenApiRequestBody("application/json", typeof(PaymentConfirmation), Required = true, Example = typeof(PaymentConfirmation))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        [FunctionName(nameof(PaymentUpdateFunction))]
        
        public static async Task<IActionResult>
            Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log,
            [Queue("%StorageQueueName%", Connection = @"StorageConnection")] IAsyncCollector<PaymentConfirmation> queueMessages)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var paymentData = JsonConvert.DeserializeObject<PaymentConfirmation>(requestBody);

            if (paymentData != null)
            {
                await queueMessages.AddAsync(paymentData);
                return new ObjectResult("Payment confirmation received");
            }
            
            return new ObjectResult("Payment confirmation not provided");
            
        }
    }
}

