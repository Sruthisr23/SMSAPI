using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Model
{
    public class WhatsappSenderConfig
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long? PhoneNumber { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
