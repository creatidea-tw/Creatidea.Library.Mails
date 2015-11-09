

namespace Creatidea.Library.Mails.Example
{
    using System;
    using System.Net.Mail;

    using global::Creatidea.Library.Mails.Creatidea;

    class Program
    {
        static void Main(string[] args)
        {
            var myMessage = new CiMessage();
            myMessage.AddTo("abc12207@creatidea.com.tw");
            myMessage.From = new MailAddress("no-reply@creatidea.tw", "John Smith");
            myMessage.Subject = "Testing the SendGrid Library";
            myMessage.Text = "Hello World! %tag%";

            // myMessage.Client.Server = "";
            // myMessage.Client.Port = ;
            // myMessage.Client.Ssl = ;
            // myMessage.Client.Account = "";
            // myMessage.Client.Pass = "";
            // myMessage.Client.Sender = "";
            // myMessage.Client.SenderName = "";

            myMessage.Send();

            // OR use send async
            myMessage.SendAsync();

            // If your developing a Console Application, use the following
            myMessage.SendAsync().Wait();

            Console.WriteLine("Sent " + DateTime.Now);

            Console.Read();
        }
    }
}
