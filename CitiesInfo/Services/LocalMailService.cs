using System;
using System.Diagnostics;

namespace CitiesInfo.Services
{
    public class LocalMailService
    {
        private string _mailTo = "matturrego92@gmail.com";
        private string _mailFrom = "mateo.urregoh@gmail.com";

        public void Send(string subject, string message) 
        {
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with LocalMailService.");
            Debug.WriteLine($"Subject: {subject}.");
            Debug.WriteLine($"Message: {message}.");

        }

    }
}
