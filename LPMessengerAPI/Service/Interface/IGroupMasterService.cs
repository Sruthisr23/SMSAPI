using LPMessengerAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service.Interface
{
    public interface IGroupMasterService
    {
        Task<GroupMaster> SaveGroup(GroupMaster groupMaster);
        Task<List<GroupMaster>> GetGroupBasedOnServiceId(int serviceId);
    }
}
