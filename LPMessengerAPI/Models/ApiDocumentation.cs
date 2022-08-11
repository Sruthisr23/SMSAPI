using System;
using System.Collections.Generic;

#nullable disable

namespace LPMessengerAPI.Models
{
    public partial class ApiDocumentation
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Token { get; set; }
        public int? ServiceId { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
