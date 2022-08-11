using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service.Interface
{
    public interface ITemplateService
    {
        Task<List<TemplateMaster>> GetTemplateByServiceId(int serviceId);
        //Task<TemplateMasterDto> GetTemplateById(int templateId);
        Task<TemplateMasterData> GetTemplateById(int templateId);
    }
}
