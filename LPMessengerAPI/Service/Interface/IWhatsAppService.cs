using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service.Interface
{
    public interface IWhatsAppService
    {
        Task<string> ImportExcelFile(string fileName, int? SenderPhoneCofigId, int? chatTemplateId);
    }
}
