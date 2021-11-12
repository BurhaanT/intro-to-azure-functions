using System.Threading.Tasks;

namespace AzureIntro.Payments
{
    public interface IPaymentQueueProcessor
    {
        public Task<bool> QueuePaymentConfirmation(PaymentConfirmation paymentConfirmation);
    }
}