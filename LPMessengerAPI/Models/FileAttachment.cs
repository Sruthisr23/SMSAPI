using System;
using System.Collections.Generic;

#nullable disable

namespace LPMessengerAPI.Models
{
    public partial class FileAttachment
    {
        public int Id { get; set; }
        public int? TemplateId { get; set; }
        public int? ChatTemplateId { get; set; }
        public string FilePath { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
