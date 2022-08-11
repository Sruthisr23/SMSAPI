using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service.Interface
{
    public interface IMailerService
    {       
        Task<MlMail> SaveMail(MailDto mail, string referenceId);
        //Task SendMail(MlMail mail);
        Task SendMail(MlMail mail, List<IFormFile> attachment, List<string> attachmentPath);
        Task<string> ImportExcelFile(ExcelFileUploadDto excelFileUploadDto);
        Task<string[]> SaveFileInFolder(List<IFormFile> formFiles);
        Task DeleteFileInFolder(List<IFormFile> formFiles);
    }
}
