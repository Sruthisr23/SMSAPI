using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Model
{
    public class ChatTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
        public string MediaUrl { get; set; }
        public int? WhatsappSenderConfigId { get; set; }
        public int? UserId { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
