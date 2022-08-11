using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service
{
    public class ServiceAvailableService : IServiceAvailableService
    {
        private readonly lpmessengerdbContext _context;
        public ServiceAvailableService(lpmessengerdbContext context)
        {
            _context = context;
        }

        public async Task<ServicesAvailable> GetServiceAvailableById(int serviceId)
        {
            try
            {
                var result = await (from g in _context.ServicesAvailables
                                    where g.Id == serviceId
                                    select g).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
