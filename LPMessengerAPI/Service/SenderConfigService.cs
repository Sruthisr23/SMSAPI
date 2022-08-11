using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service
{
    public class SenderConfigService : ISenderConfigService
    {
        private readonly lpmessengerdbContext _context;
        public SenderConfigService(lpmessengerdbContext context)
        {
            _context = context;
        }

        public async Task<MlSenderConfig> GetSenderConfigAsync(int id)
        {
            try
            {
                var mlSenderConfig = await _context.MlSenderConfigs.FindAsync(id);

                return mlSenderConfig;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}



