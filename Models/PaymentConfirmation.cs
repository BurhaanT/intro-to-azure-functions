using System;

namespace AzureIntro
{
    public class PaymentConfirmation
    {
        public string PaymentReferenceId {get;set;}
        public int OrderId {get;set;}
        public decimal Amount {get;set;}
        public DateTime ProcessedDate {get;set;}
    }
}