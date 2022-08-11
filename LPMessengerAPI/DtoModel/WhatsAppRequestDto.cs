using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.DtoModel
{
    public class WhatsAppRequestDto
    {
        public string Phone { get; set; }
        public string Message { get; set; }
        public string TemplateId { get; set; }
        public string MessageType { get; set; }
        public string MediaUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
    public class WhatsAppMainRequestDto
    {
        public string[] DestinationPhoneNumbers { get; set; }
        public string Title { get; set; }
        public string MessageTemplateId { get; set; }
        public string Description { get; set; }
        public string TextMessage { get; set; }
        public string MediaMessage { get; set; }
        public string RedirectUrl { get; set; }
    }

}

