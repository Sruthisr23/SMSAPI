using System;
using System.Collections.Generic;

#nullable disable

namespace LPMessengerAPI.Model
{
    public partial class SenderPhoneConfig
    {
        public int Id { get; set; }
        public long? Phone { get; set; }
        public string SenderName { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
