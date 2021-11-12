using System.IO;
using System.Net;
using System.Threading.Tasks;
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

        [FunctionName(nameof(PaymentUpdateFunction))]
        [OpenApiOperation(operationId: "Run", tags: new[] { "PaymentInformation" })]
        //[OpenApiParameter(name: "PaymentInformation", Required = true, In = ParameterLocation.Path, Type = typeof(PaymentConfirmation), Description = "Payment callback details", Example = typeof(PaymentConfirmation))]
        [OpenApiRequestBody("application/json", typeof(PaymentConfirmation), Required = true, Example = typeof(PaymentConfirmation))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var paymentData = JsonConvert.DeserializeObject<PaymentConfirmation>(requestBody);

            var responseMsg = 
                (paymentData == null) ? "No payment details provided" : (await _paymentQueueProcessor.QueuePaymentConfirmation(paymentData)).ToString();

            return new OkObjectResult(responseMsg);
        }
    }
}

