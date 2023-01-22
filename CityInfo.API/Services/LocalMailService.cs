﻿namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailTo = "admin@mycompany.com";
        private string _mailFrom = "noreplay@mycompany.com";

        public void Send(string subject, string message)
        {
            // send mail - output to console window
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo} with {nameof(LoaderOptimization)}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}