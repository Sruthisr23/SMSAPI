using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.DtoModel
{
    public class SmsRequestDto
    {
        public string PhoneNumber { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
    }
    public class SmsRequestAuthDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int MessageSendType { get; set; }
        public string Message { get; set; }
        public string PhoneNumber { get; set; }
        public string SenderName { get; set; }
        public string MsgUniqueId { get; set; }

    }
}
