using AzureIntro.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;

namespace AzureIntro.Orders
{
    public static class ProcessOrderFunction
    {
        [FunctionName("ProcessOrderFunction")]
        [return: Table("%OrdersTableName%", Connection = @"StorageConnection")]
        public static Order Run([QueueTrigger("%StorageQueueName%", Connection = @"StorageConnection")] string myQueueItem,
            ILogger log,
            [Queue("%EmailQueueName%", Connection = @"StorageConnection")] ICollector<Email> emailQueue)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var paymentItem = JsonConvert.DeserializeObject<PaymentConfirmation>(myQueueItem);

            var orderItem = new Order { OrderId = paymentItem.OrderId, PaymentDate = paymentItem.ProcessedDate, PaymentStatus = PaymentStatus.Paid };
            var emailToCustomer = new Email { EmailTo = "burhaan@sixpivot.com.au", EmailSubject = "Order Confirmation", EmailBody = "Your order has been confirmed. Enjoy!" };
            emailQueue.Add(emailToCustomer);

            return orderItem;
        }
    }
}
