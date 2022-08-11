using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.DtoModel
{
    public class WhatsAppExcelUploadDto
    {
        public long? SenderPhone { get; set; }
        public long? Phone { get; set; }
        public string Message { get; set; }
        public string TemplateId { get; set; }
        public string MessageType { get; set; }
        public string MediaUrl { get; set; }
    }
}
