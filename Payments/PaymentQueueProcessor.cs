using System.Threading.Tasks;

namespace AzureIntro.Payments
{
    public class PaymentQueueProcessor : IPaymentQueueProcessor
    {
        public Task<bool> QueuePaymentConfirmation(PaymentConfirmation paymentConfirmation)
        {
            return Task.FromResult(true);
        }
    }
}