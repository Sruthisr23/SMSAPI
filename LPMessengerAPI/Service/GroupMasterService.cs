using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service
{
    public class GroupMasterService : IGroupMasterService
    {
        private readonly lpmessengerdbContext _context;
        public GroupMasterService(lpmessengerdbContext context)
        {
            _context = context;
        }

        public async Task<GroupMaster> SaveGroup(GroupMaster groupMaster)
        {
            try
            {
                var result = _context.GroupMasters.Add(groupMaster);
                await _context.SaveChangesAsync();
                return result.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<GroupMaster>> GetGroupBasedOnServiceId(int serviceId)
        {
            try
            {
                var result = await (from g in _context.GroupMasters
                                    where g.ServiceId == serviceId
                                    select g).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
