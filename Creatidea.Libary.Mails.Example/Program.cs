

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

            //myMessage.Client.Server = "smtp.gmail.com";
            //myMessage.Client.Port = 587;
            //myMessage.Client.Ssl = true;
            //myMessage.Client.Account = "no-reply@creatidea.com.tw";
            //myMessage.Client.Pass = "d}8ED*$?c)78K@z";
            //myMessage.Client.Sender = "no-reply@creatidea.com.tw";
            //myMessage.Client.SenderName = "test";

            myMessage.Send();

            Console.WriteLine("Sent " + DateTime.Now);

            Console.Read();
        }
    }
}
