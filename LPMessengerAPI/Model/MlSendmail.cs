using System;
using System.Collections.Generic;

#nullable disable

namespace LPMessengerAPI.Model
{
    public partial class MlSendmail
    {
        public int Id { get; set; }
        public int? MailId { get; set; }
        public bool MailSendStatus { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
