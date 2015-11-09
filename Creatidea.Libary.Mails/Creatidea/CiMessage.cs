namespace Creatidea.Library.Mails.Creatidea
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    using global::Creatidea.Library.Mails.Creatidea;

    /// <summary>
    /// 電子郵件相關
    /// </summary>
    public class CiMessage
    {
        #region constants/vars

        // apps list and settings
        private readonly MailMessage _message;

        #endregion

        #region Initialization and Constructors

        public CiMessage()
        {
            this._message = new MailMessage();
            this.Client = new CiClient();
        }

        public CiMessage(MailAddress from, MailAddress[] to, string subject, string html, string text) : this()
        {
            this.From = from;
            this.To = to;

            this._message.Subject = subject;

            this.Text = text;
            this.Html = html;
        }

        #endregion

        #region Properties  

        public CiClient Client { get; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>寄件者.</value>
        public MailAddress From
        {
            get { return this._message.From; }
            set { if (value != null) this._message.From = value; }
        }

        /// <summary>
        /// Gets or sets the ReplyTo.
        /// </summary>
        /// <value>The ReplyTo.</value>
        public MailAddress[] ReplyTo
        {
            get { return this._message.ReplyToList.ToArray(); }
            set
            {
                this._message.ReplyToList.Clear();
                foreach (var replyTo in value)
                {
                    this._message.ReplyToList.Add(replyTo);
                }
            }
        }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>收件者.</value>
        public MailAddress[] To
        {
            get
            {
                return this._message.To.ToArray();
            }
            set
            {
                this._message.To.Clear();
                foreach (var mailAddress in value)
                {
                    this._message.To.Add(mailAddress);
                }
            }
        }

        /// <summary>
        /// Gets or sets the cc.
        /// </summary>
        /// <value>副本.</value>
        public MailAddress[] Cc
        {
            get { return this._message.CC.ToArray(); }
            set
            {
                this._message.CC.Clear();
                foreach (var mailAddress in value)
                {
                    this._message.CC.Add(mailAddress);
                }
            }
        }

        /// <summary>
        /// Gets or sets the BCC.
        /// </summary>
        /// <value>密件副本.</value>
        public MailAddress[] Bcc
        {
            get { return this._message.Bcc.ToArray(); }
            set
            {
                this._message.Bcc.Clear();
                foreach (var mailAddress in value)
                {
                    this._message.Bcc.Add(mailAddress);
                }
            }
        }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>主旨.</value>
        public string Subject
        {
            get { return this._message.Subject; }
            set { if (value != null) this._message.Subject = value; }
        }

        public string Html { get; set; }

        public string Text { get; set; }

        #endregion

        #region Methods for setting data

        private List<String> _attachments = new List<String>();
        private Dictionary<String, MemoryStream> _streamedAttachments = new Dictionary<string, MemoryStream>();
        private Dictionary<String, String> _contentImages = new Dictionary<string, string>();

        /// <summary>
        /// 加入收件者
        /// </summary>
        /// <param name="address">The address.</param>
        public void AddTo(String address)
        {
            var mailAddress = new MailAddress(address);
            this._message.To.Add(mailAddress);
        }

        /// <summary>
        /// 加入收件者.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        public void AddTo(IEnumerable<String> addresses)
        {
            if (addresses == null) return;

            foreach (var address in addresses.Where(address => address != null))
                this.AddTo(address);
        }

        /// <summary>
        /// 加入副本收件者.
        /// </summary>
        /// <param name="address">The address.</param>
        public void AddCc(string address)
        {
            var mailAddress = new MailAddress(address);
            this._message.CC.Add(mailAddress);
        }

        /// <summary>
        /// 加入副本收件者.
        /// </summary>
        /// <param name="address">The address.</param>
        public void AddCc(MailAddress address)
        {
            this._message.CC.Add(address);
        }

        /// <summary>
        /// 加入密件副本收件者.
        /// </summary>
        /// <param name="address">The address.</param>
        public void AddBcc(string address)
        {
            var mailAddress = new MailAddress(address);
            this._message.Bcc.Add(mailAddress);
        }

        /// <summary>
        /// 加入密件副本收件者.
        /// </summary>
        /// <param name="address">The address.</param>
        public void AddBcc(MailAddress address)
        {
            this._message.Bcc.Add(address);
        }

        /// <summary>
        /// Gets or sets the MemoryStream attachments.
        /// </summary>
        /// <value>The MemoryStream attachments.</value>
        public Dictionary<String, MemoryStream> StreamedAttachments
        {
            get { return this._streamedAttachments; }
            set { this._streamedAttachments = value; }
        }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        /// <value>The attachments.</value>
        public String[] Attachments
        {
            get { return this._attachments.ToArray(); }
            set { this._attachments = value.ToList(); }
        }

        /// <summary>
        /// 加入附件
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="name">The name.</param>
        public void AddAttachment(Stream stream, String name)
        {
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            this.StreamedAttachments[name] = ms;
        }

        /// <summary>
        /// 加入附件
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void AddAttachment(String filePath)
        {
            this._attachments.Add(filePath);
        }

        /// <summary>
        /// 取得所有收件者（收件者、副本、密件副本）.
        /// </summary>
        /// <returns>IEnumerable&lt;String&gt;.</returns>
        public IEnumerable<String> GetRecipients()
        {
            var tos = this._message.To.ToList();
            var ccs = this._message.CC.ToList();
            var bccs = this._message.Bcc.ToList();

            var rcpts = tos.Union(ccs.Union(bccs)).Select(address => address.Address);
            return rcpts;
        }

        #endregion

        /// <summary>
        /// 將所有相關參數整理至MailMessage
        /// </summary>
        /// <returns></returns>
        public MailMessage CreateMimeMessage()
        {
            this._message.Attachments.Clear();
            this._message.AlternateViews.Clear();

            if (this.Attachments != null)
            {
                foreach (var attachment in this.Attachments)
                {
                    this._message.Attachments.Add(new Attachment(attachment, MediaTypeNames.Application.Octet));
                }
            }

            if (this.StreamedAttachments != null)
            {
                foreach (var attachment in this.StreamedAttachments)
                {
                    attachment.Value.Position = 0;
                    this._message.Attachments.Add(new Attachment(attachment.Value, attachment.Key));
                }
            }

            if (this.Text != null)
            {
                var plainView = AlternateView.CreateAlternateViewFromString(this.Text, null, "text/plain");
                this._message.AlternateViews.Add(plainView);
            }

            if (this.Html == null) return this._message;

            var htmlView = AlternateView.CreateAlternateViewFromString(this.Html, null, "text/html");
            this._message.AlternateViews.Add(htmlView);
            this._message.Body = this.Html;
            this._message.IsBodyHtml = true;
            this._message.BodyEncoding = Encoding.UTF8;

            return this._message;
        }

        /// <summary>
        /// send mail.
        /// </summary>
        public void Send()
        {
            CheckMailServerVariable();

            using (var smtp = new SmtpClient())
            {
                smtp.Host = this.Client.Server;
                smtp.Port = this.Client.Port;
                smtp.EnableSsl = Convert.ToBoolean(this.Client.Ssl);

                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(this.Client.Account, this.Client.Pass);
                smtp.Credentials = credentials;

                this._message.From = new MailAddress(this.Client.Sender, this.Client.SenderName);
                var msg = this.CreateMimeMessage();

                smtp.Send(msg);

                this._message.Dispose();
            }
        }

        /// <summary>
        /// send mail as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task SendAsync()
        {
            CheckMailServerVariable();

            using (var smtp = new SmtpClient())
            {
                smtp.Host = this.Client.Server;
                smtp.Port = this.Client.Port;
                smtp.EnableSsl = Convert.ToBoolean(this.Client.Ssl);

                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(this.Client.Account, this.Client.Pass);
                smtp.Credentials = credentials;

                this._message.From = new MailAddress(this.Client.Sender, this.Client.SenderName);
                var msg = this.CreateMimeMessage();

                await smtp.SendMailAsync(this._message);

                this._message.Dispose();
            }
        }

        /// <summary>
        /// Helper function lets us look at the mime before it is sent
        /// </summary>
        /// <param name="directory">directory in which we store this mime message</param>
        internal void SaveMessage(string directory)
        {
            var client = new SmtpClient("localhost")
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = directory
            };
            var msg = this.CreateMimeMessage();
            client.Send(msg);
        }

        /// <summary>
        /// check mail server Variable
        /// </summary>
        private void CheckMailServerVariable()
        {
            // 若Client沒有複寫設定，則從config appsetting中讀取
            if (string.IsNullOrWhiteSpace(this.Client.Server))
            {
                try
                {
                    this.Client.Server = ConfigurationManager.AppSettings["ciMail.Server"];
                    if (string.IsNullOrWhiteSpace(this.Client.Server))
                        throw new ConfigurationErrorsException("取得 MailServer 資訊發生錯誤: ciMail.Server");
                }
                catch (Exception)
                {
                    throw new ConfigurationErrorsException("取得 MailServer 資訊發生錯誤: ciMail.Server");
                }
            }

            if (this.Client.Port == default(int))
            {
                try
                {
                    this.Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["ciMail.Port"]);
                    if (this.Client.Port == default(int))
                        throw new ConfigurationErrorsException("取得 MailServer 資訊發生錯誤: ciMail.Port");
                }
                catch (Exception)
                {
                    throw new ConfigurationErrorsException("取得 MailServer 資訊發生錯誤: ciMail.Port");
                }
            }

            if (this.Client.Ssl == default(bool?))
            {
                try
                {
                    this.Client.Ssl = Convert.ToBoolean(ConfigurationManager.AppSettings["ciMail.Ssl"]);
                    if (this.Client.Ssl == default(bool?))
                        throw new ConfigurationErrorsException("取得 MailServer 資訊發生錯誤: ciMail.Ssl");
                }
                catch (Exception)
                {
                    throw new ConfigurationErrorsException("取得 MailServer 資訊發生錯誤: ciMail.Ssl");
                }
            }

            if (string.IsNullOrWhiteSpace(this.Client.Account))
            {
                try
                {
                    this.Client.Account = ConfigurationManager.AppSettings["ciMail.Account"];
                    if (string.IsNullOrWhiteSpace(this.Client.Account))
                        throw new ConfigurationErrorsException("取得 MailServer 資訊發生錯誤: ciMail.Account");
                }
                catch (Exception)
                {
                    throw new ConfigurationErrorsException("取得 MailServer 資訊發生錯誤: ciMail.Account");
                }
            }

            if (string.IsNullOrWhiteSpace(this.Client.Pass))
            {
                try
                {
                    this.Client.Pass = ConfigurationManager.AppSettings["ciMail.Password"];
                    if (string.IsNullOrWhiteSpace(this.Client.Pass))
                        throw new ConfigurationErrorsException("取得 MailServer 資訊發生錯誤: ciMail.Password");
                }
                catch (Exception)
                {
                    throw new ConfigurationErrorsException("取得 MailServer 資訊發生錯誤: ciMail.Password");
                }
            }

            if (string.IsNullOrWhiteSpace(this.Client.Sender))
            {
                try
                {
                    this.Client.Sender = ConfigurationManager.AppSettings["ciMail.Sender"];
                    if (string.IsNullOrWhiteSpace(this.Client.Sender))
                        throw new ConfigurationErrorsException("取得 MailServer 資訊發生錯誤: ciMail.Sender");
                }
                catch (Exception)
                {
                    throw new ConfigurationErrorsException("取得 MailServer 資訊發生錯誤: ciMail.Sender");
                }
            }

            if (string.IsNullOrWhiteSpace(this.Client.SenderName))
            {
                try
                {
                    this.Client.SenderName = ConfigurationManager.AppSettings["ciMail.SenderName"];
                    if (string.IsNullOrWhiteSpace(this.Client.SenderName))
                        throw new ConfigurationErrorsException("取得 MailServer 資訊發生錯誤: ciMail.SenderName");
                }
                catch (Exception)
                {
                    throw new ConfigurationErrorsException("取得 MailServer 資訊發生錯誤: ciMail.SenderName");
                }
            }
        }
    }
}
