using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service.Interface
{
    public interface IExternalService
    {
        string PostSms(SmsRequestDto model, string msgUniqueId);
        string PostwhatsAppToken();
        string postWhatsApp(string token, WhatsAppRequestDto model);
        Task<int> RandomNumber();
        Task<string> RandomNumberAsync(int? length);
        Task<string> RandomAlphaNumeric(int? length);
        Task SaveFileInFolder(IFormFile formFile);
        Task DeleteFileInFolder(IFormFile formFile);
        Task<string> MoveFileToFolder(string fullPath);
    }
}
