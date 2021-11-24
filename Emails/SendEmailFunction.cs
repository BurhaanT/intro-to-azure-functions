using System;
using System.IO;
using System.Threading.Tasks;
using AzureIntro.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AzureIntro.Emails
{
    public static class SendEmailFunction
    {
        [FunctionName("SendEmailFunction")]
        public static async Task RunAsync([QueueTrigger("%EmailQueueName%", Connection = @"StorageConnection")]string queueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {queueItem}");

            var emailData = JsonConvert.DeserializeObject<Email>(queueItem);

            var apiKey = Environment.GetEnvironmentVariable("SendGridAPIKey");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("burhaan@sixpivot.com.au", "Burhaan");
            var subject = emailData.EmailSubject;
            var to = new EmailAddress(emailData.EmailTo, "Example User");
            var plainTextContent = emailData.EmailBody;
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, "");
            var response = await client.SendEmailAsync(msg);
        }
    }
}
