using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.DtoModel
{
    public class MailDto
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public string Subject { get; set; }
        public bool IsBodyHtml { get; set; }        
        public string Body { get; set; }         
    }

    public class MultipleMailDto
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string[] ReceiverEmail { get; set; }
        public string Subject { get; set; }
        public bool IsBodyHtml { get; set; }
        public string Body { get; set; }
    }
}
