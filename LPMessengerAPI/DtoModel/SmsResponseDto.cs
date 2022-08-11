using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.DtoModel
{
    public class SmsResponseDto
    {
        public List<SmsResponseData> response { get; set; }
    }

    public class SmsResponseData
    {
        public string data { get; set; }
    }
}
