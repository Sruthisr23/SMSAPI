using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.DtoModel
{
    public class ExcelMailTemplateDto
    {
        public int? Id { get; set; }
        public string Email { get; set; }
    }
    public class ExcelMailTemplateDtos
    {
        public int? ID { get; set; }
        public string COMPANY_NAME { get; set; }
        public string EMAIL { get; set; }
    }    
}