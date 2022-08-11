using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service.Interface
{
    public interface ISmsService
    {
        Task<SmsTransactionMessageStatus> SendSms(SmsRequestDto smsRequestDto);
        Task<string> ImportExcelFile(ExcelFileUploadDto excelFileUploadDto);
        Task<int> RandomNumber();
        Task<List<SmsRequestDto>> GetGroupmessage(int? groupId, int? templateId);
    }
}
