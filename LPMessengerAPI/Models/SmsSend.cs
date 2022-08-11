using System;
using System.Collections.Generic;

#nullable disable

namespace LPMessengerAPI.Models
{
    public partial class SmsSend
    {
        public int Id { get; set; }
        public long? Phone { get; set; }
        public string Body { get; set; }
        public string Otp { get; set; }
        public string RefferenceNumber { get; set; }
        public int? GroupId { get; set; }
        public int? TemplateId { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
