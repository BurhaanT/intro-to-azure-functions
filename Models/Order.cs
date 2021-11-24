using AzureIntro.Utilities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace AzureIntro.Models
{
    public class Order: ComplexTableEntity
    {
        public Order()
        {
            PartitionKey = "Http"; 
            RowKey = Guid.NewGuid().ToString();
        }

        public int OrderId { get; set; }
        public DateTime PaymentDate { get; set; }

        [EntityPropertyConverter(typeof(PaymentStatus))]
        public PaymentStatus PaymentStatus {get; set; }
    }
}

namespace AzureIntro.Models
{
    public enum PaymentStatus
    {
        Paid,
        Unapid
    }
}