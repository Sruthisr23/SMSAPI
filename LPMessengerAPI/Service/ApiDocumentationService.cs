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
    public class ApiDocumentationService : IApiDocumentationService
    {
        private readonly lpmessengerdbContext _context;
        public ApiDocumentationService(lpmessengerdbContext context)
        {
            _context = context;
        }

        public async Task<ApiDocumentationResponseDto> GetApiDocumentationDetails(int? serviceId)
        {
            try
            {
                var result = await (from a in _context.ApiDocumentations
                                    //join u in _context.ApiUsers on a.ApiUserId equals u.Id
                                    where a.ServiceId == serviceId  
                                    select new ApiDocumentationResponseDto
                                    {
                                        Id = a.Id,
                                        Url = a.Url,
                                        Body = a.Body,
                                        Token = a.Token
                                    }).FirstOrDefaultAsync();


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
