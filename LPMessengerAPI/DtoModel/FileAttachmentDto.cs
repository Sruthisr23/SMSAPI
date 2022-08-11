using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.DtoModel
{
    public class FileAttachmentDto
    {
        public int Id { get; set; }
        public int? TemplateId { get; set; }
        public int? ChatTemplateId { get; set; }
        public string FilePath { get; set; }
        public int? UserId { get; set; }
    }
}
