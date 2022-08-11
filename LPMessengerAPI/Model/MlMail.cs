using System;
using System.Collections.Generic;

#nullable disable

namespace LPMessengerAPI.Model
{
    public partial class MlMail
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        //public List<string> ReceiverEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public bool IsBodyHtml { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Otp { get; set; }
        public string ReferenceId { get; set; }
        public int? GroupId { get; set; }
        public int? TemplateId { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
