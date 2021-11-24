using System;

namespace AzureIntro.Models
{
    public class PaymentConfirmation
    {
        public string PaymentReferenceId {get;set;}
        public int OrderId {get;set;}
        public decimal Amount {get;set;}
        public DateTime ProcessedDate {get;set;}
    }
}