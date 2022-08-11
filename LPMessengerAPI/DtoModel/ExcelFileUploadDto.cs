using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.DtoModel
{
    public class ExcelFileUploadDto
    {
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public string FileName { get; set; }
        public int? serviceId { get; set; }
        
    }
}
