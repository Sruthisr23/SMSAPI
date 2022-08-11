using LPMessengerAPI.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.DtoModel
{
    public class TemplateMasterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool? IsBodyHtml { get; set; }
        public int? UserId { get; set; }
        public int? ServiceId { get; set; }
        public List<FileAttachmentDto> fileAttachments { get; set; }

    }

    public class TemplateMasterData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool? IsBodyHtml { get; set; }
        public int? UserId { get; set; }
        public int? ServiceId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<IFormFile> FormFiles { get; set; }

    }
}
