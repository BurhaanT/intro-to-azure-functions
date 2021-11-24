using AzureIntro.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace AzureIntro.Payments
{
    public class PaymentQueueProcessor : IPaymentQueueProcessor
    {
        private readonly IConfiguration _configuration;

        public PaymentQueueProcessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<bool> QueuePaymentConfirmation(PaymentConfirmation paymentConfirmation)
        {
            

            return Task.FromResult(true);
        }
    }
}