using System;
using System.Collections.Generic;

#nullable disable

namespace LPMessengerAPI.Models
{
    public partial class WhatsappSend
    {
        public int Id { get; set; }
        public int? SenderPhoneCofigId { get; set; }
        public long? Phone { get; set; }
        public string Message { get; set; }
        public string TemplateId { get; set; }
        public int? ChatTemplateId { get; set; }
        public string MessageType { get; set; }
        public string MediaUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ulong? IsSend { get; set; }
        public int? GroupId { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
