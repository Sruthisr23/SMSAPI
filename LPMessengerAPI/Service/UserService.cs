using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service
{
    public class UserService : IUserService
    {
        private readonly lpmessengerdbContext _context;
        public UserService(lpmessengerdbContext context)
        {
            _context = context;
        }

        public async Task<UserLoginDto> Authenticate(string userName , string password)
        {
            try
            {
                var result = await (from u in _context.Users
                                    where u.Email == userName && u.Password == password
                                    select new UserLoginDto
                                    {
                                        UserId = u.Id,
                                        FirstName = u.FirstName,
                                        LastName = u.LastName,
                                        Email = u.Email,
                                        Mobile = u.Mobile,
                                        Token = u.Token
                                    }).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetToken(int userId)
        {
            try
            {
                var result = await (from u in _context.Users
                                    where u.Id == userId
                                    select u.Token).FirstOrDefaultAsync();
                if(result == null)
                {
                    return "false";
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
