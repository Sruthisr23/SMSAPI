using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service.Interface
{
    public interface IMailService
    {
        Task<MlMail> PostMail(MlMail mail);
        Task<MlMail> UpdateMail(MlMail mail);
        Task<List<MlMail>> GetMailListViaGroupAndTemplate(int? groupId, int? templateId);
        Task<List<MlMailDto>> GetGroupMailList(int? groupId);
        Task<bool> ValidateOtp(string otp, string referenceId);
    }
}
