using System;
using System.Collections.Generic;

#nullable disable

namespace LPMessengerAPI.Models
{
    public partial class GroupMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? UserId { get; set; }
        public int? ServiceId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
