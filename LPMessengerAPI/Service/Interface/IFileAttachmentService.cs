using LPMessengerAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service.Interface
{
    public interface IFileAttachmentService
    {
        Task<FileAttachment> SaveFileTemplate(FileAttachment fileAttachment);
        Task<FileAttachment> UpdateFileTemplate(FileAttachment fileAttachment);
        Task<List<FileAttachment>> GetAttachmentsByTemplateId(int templateId);
        Task<List<FileAttachment>> GetAttachmentsByChatTemplateId(int chatTemplateId);
    }
}
