using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service.Interface
{
    public interface IWhatsAppSendService
    {
        Task<WhatsappSend> PostWhatsAppSend(WhatsAppRequestDto whatsAppRequestDto);
        Task<List<WhatsAppRequestDto>> GetWhatsAppSendData(int? groupId);
        Task<WhatsappSend> PostWhatsAppBulkSend(WhatsappSend whatsAppRequestDto);
    }
}
