using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.DtoModel
{
    public class AppSettings
    {
        public string Location { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public string MailHost { get; set; }
        public int MailPort { get; set; }
        public bool EnableSslEncryption { get; set; }
        public string SmsUserName { get; set; }
        public string SmsPassword { get; set; }
        public string SmsBaseUrl { get; set; }
        public string SmsSendUrl { get; set; }
        public string WhatsAppUserName { get; set; }
        public string WhatsAppPassword { get; set; }
        public string WhatsAppBaseUrl { get; set; }
        public string WhatsAppTokenUrl { get; set; }
        public string WhatsAppSendUrl { get; set; }
        public string WhatsAppRedirectUrl { get; set; }
        public string WhatsAppFile { get; set; }
        public string SmsFile { get; set; }
        public string MailFile { get; set; }
    }
}
