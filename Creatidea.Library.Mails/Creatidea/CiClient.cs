namespace Creatidea.Library.Mails.Creatidea
{
    public class CiClient
    {
        #region Properties  
        public string Server { get; set; }
        public int Port { get; set; }
        public bool? Ssl { get; set; }
        public string Account { get; set; }
        public string Pass { get; set; }
        public string Sender { get; set; }
        public string SenderName { get; set; }
        #endregion
    }
}
