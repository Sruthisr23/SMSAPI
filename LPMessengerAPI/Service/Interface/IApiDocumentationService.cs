using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service.Interface
{
    public interface IApiDocumentationService
    {
        Task<ApiDocumentationResponseDto> GetApiDocumentationDetails(int? serviceId);
    }
}
