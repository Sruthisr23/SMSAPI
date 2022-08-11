using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service
{
    public class FileAttachmentService : IFileAttachmentService
    {
        private readonly lpmessengerdbContext _context;
        private readonly IOptions<AppSettings> _appSettings;
        public FileAttachmentService(lpmessengerdbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        public async Task<FileAttachment> SaveFileTemplate(FileAttachment fileAttachment)
        {
            try
            {
                var path = _appSettings.Value.Location + fileAttachment.FilePath;
                fileAttachment.FilePath = path;
                var result = _context.FileAttachments.Add(fileAttachment);
                await _context.SaveChangesAsync();
                return result.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FileAttachment> UpdateFileTemplate(FileAttachment fileAttachment)
        {
            try
            {
                
                await _context.SaveChangesAsync();
                return fileAttachment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FileAttachment>> GetAttachmentsByTemplateId(int templateId)
        {
            try
            {
                var result = await (from g in _context.FileAttachments
                                    where g.TemplateId == templateId
                                    select g).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FileAttachment>> GetAttachmentsByChatTemplateId(int chatTemplateId)
        {
            try
            {
                var result = await (from g in _context.FileAttachments
                                    where g.ChatTemplateId == chatTemplateId
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
