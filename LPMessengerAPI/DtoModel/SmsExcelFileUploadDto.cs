using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.DtoModel
{
    public class SmsExcelFileUploadDto
    {
        public string SenderPhone { get; set; }
        public string ReceiverPhone { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }        
    }
}
