using System;
using System.Collections.Generic;

#nullable disable

namespace LPMessengerAPI.Models
{
    public partial class MlSendmail
    {
        public int Id { get; set; }
        public int? MailId { get; set; }
        public ulong? MailSendStatus { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
