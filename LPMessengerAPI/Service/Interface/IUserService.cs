using LPMessengerAPI.DtoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service.Interface
{
    public interface IUserService
    {
        Task<UserLoginDto> Authenticate(string userName, string password);
        Task<string> GetToken(int userId);
    }
}
