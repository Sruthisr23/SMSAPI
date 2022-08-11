using LPMessengerAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service.Interface
{
    public interface ISenderConfigService
    {
        Task<MlSenderConfig> GetSenderConfigAsync(int id);
    }
}
