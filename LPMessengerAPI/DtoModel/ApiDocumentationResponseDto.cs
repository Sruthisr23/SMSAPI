using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.DtoModel
{
    public class ApiDocumentationResponseDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public string Token { get; set; }
    }
}
