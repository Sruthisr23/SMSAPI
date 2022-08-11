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
    public class TemplateService : ITemplateService
    {
        private readonly lpmessengerdbContext _context;
        public TemplateService(lpmessengerdbContext context)
        {
            _context = context;
        }

        public async Task<List<TemplateMaster>> GetTemplateByServiceId(int serviceId)
        {
            try
            {
                var result = await (from t in _context.TemplateMasters
                                    where t.ServiceId == serviceId
                                    select t).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TemplateMasterData> GetTemplateById(int templateId)
        {
            try
            {
                var result = await (from t in _context.TemplateMasters
                                    //join f in _context.FileAttachments on t.Id equals f.TemplateId
                                    where t.Id == templateId
                                    select new TemplateMasterData
                                    {
                                        Id = t.Id,
                                        Body = t.Body,
                                        IsBodyHtml = t.IsBodyHtml,
                                        Name = t.Name,
                                        ServiceId = t.ServiceId,
                                        Subject = t.Subject,
                                        UserId = t.UserId                                        
                                    }).FirstOrDefaultAsync();

                //result.fileAttachments = await (from f in _context.FileAttachments
                //                                where f.TemplateId == result.Id
                //                                select new FileAttachmentDto
                //                                {
                //                                    ChatTemplateId = f.ChatTemplateId,
                //                                    FilePath = f.FilePath,
                //                                    Id = f.Id,
                //                                    TemplateId = f.TemplateId,
                //                                    UserId = f.UserId,
                //                                }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
