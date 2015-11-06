# Creatidea.Libary.Mails
創鈺共用類別庫系列

[![BuildStatus](https://travis-ci.org/lettucebo/Creatidea.Libary.Mails.png?branch=develope)](https://travis-ci.org/lettucebo/Creatidea.Libary.Mails)

See the [changelog](https://github.com/lettucebo/Creatidea.Libary.Mails/blob/develope/CHANGELOG.md) for updates. 

#Requirements

this library requires .NET 4.5 and above.

#Installation

You can either <a href="https://github.com/lettucebo/Creatidea.Libary.Mails.git">download</a> the source and build your own dll or, if you have the NuGet package manager installed, you can grab them automatically.

```
PM> Install-Package Creatidea.Library.Mails 
```

Once you have the SendGrid libraries properly referenced in your project, you can include calls to them in your code. 
For a sample implementation, check the [Example](https://github.com/sendgrid/sendgrid-csharp/tree/master/SendGrid/Example) folder.

Add the following namespaces to use the library:
```csharp
using System;
using System.Net;
using System.Net.Mail;
using Creatidea.Library.Mails;
```

#How to: Create an email

Use the static **new SendGridMessage** constructor to create an email message that is of type **SendGridMessage**. Once the message is created, you can use **SendGridMessage** properties and methods to set values including the email sender, the email recipient, and the subject and body of the email.

The following example demonstrates how to create an email object and populate it:

```csharp
// Create the email object first, then add the properties.
var message = new CiMessage();

// Add the message properties.
message.From = new MailAddress("no-reply@creatidea.com.tw");

// Add multiple addresses to the To field.
List<String> recipients = new List<String>
{
    @"Jeff Smith <user1@creatidea.com.tw>",
    @"Anna Lidman <user2@creatidea.com.tw>",
    @"Peter Saddow <user3@creatidea.com.tw>"
};

message.AddTo(recipients);

// Add single address
message.AddTo(new MailAddress("user4@creatidea.com.tw"));

message.Subject = "Testing the Creatidea Mail";

//Add the HTML and Text bodies
message.Html = "<p>Hello World!</p>";
message.Text = "Hello World plain text!";
```

#How to: Send an Email

After creating an email message, you can send it using the Web API provided by SendGrid.

Sending email requires that you supply your SendGrid account credentials (username and password) OR a SendGrid API Key. API Key is the preferred method. API Keys are in beta. To configure API keys, visit https://sendgrid.com/beta/settings/api_keys

Using Credentials
```csharp
// Create network credentials to access your SendGrid account.
var username = "your_sendgrid_username";
var pswd = "your_sendgrid_password";

var credentials = new NetworkCredential(username, pswd);
```
To send an email message, use the **DeliverAsync** method on the **Web** transport class, which calls the SendGrid Web API. The following example shows how to send a message.


```csharp
// Create the email object first, then add the properties.
SendGridMessage myMessage = new SendGridMessage();
myMessage.AddTo("anna@example.com");
myMessage.From = new MailAddress("john@example.com", "John Smith");
myMessage.Subject = "Testing the SendGrid Library";
myMessage.Text = "Hello World!";

// Create credentials, specifying your user name and password.
var credentials = new NetworkCredential("username", "password");

// Create an Web transport for sending email, using credentials...
//var transportWeb = new Web(credentials);

// ...OR create a Web transport, using API Key (preferred)
var transportWeb = new Web("This string is an API key");

// Send the email.
transportWeb.DeliverAsync(myMessage);
// If your developing a Console Application, use the following
// transportWeb.DeliverAsync(myMessage).Wait();
```

#How to: Add an Attachment

Attachments can be added to a message by calling the **AddAttachment** method and specifying the name and path of the file you want to attach, or by passing a stream. You can include multiple attachments by calling this method once for each file you wish to attach. The following example demonstrates adding an attachment to a message:

```csharp
CiMessage message = new CiMessage();
message.AddTo("anna@example.com");
message.From = new MailAddress("john@example.com", "John Smith");
message.Subject = "Testing the Creatidea Library";
message.Text = "Hello World!";

message.AddAttachment(@"C:\file1.txt");
```

You can also add attachments from the data's **Stream**. It can be done by calling the same method as above, **AddAttachment**, but by passing in the Stream of the data, and the filename you want it to show as in the message.

```csharp
CiMessage message = new CiMessage();
message.AddTo("anna@example.com");
message.From = new MailAddress("john@example.com", "John Smith");
message.Subject = "Testing the SendGrid Library";
message.Text = "Hello World!";

using (var attachmentFileStream = new FileStream(@"C:\file.txt", FileMode.Open))
{
    message.AddAttachment(attachmentFileStream, "My Cool File.txt");
}
```

[Any Question](mailto:abc12207@gmail.com)

