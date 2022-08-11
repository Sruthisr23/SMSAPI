using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service.Interface
{
    public interface ISmsTransactionMessageStatusService
    {
        Task<SmsTransactionMessageStatus> PostSmsTransactionMessageStatus(SmsRequestDto smsRequestDto, string userId, string otp);
        Task<SmsTransactionMessageStatus> UpdateSmsTransactionMessageStatus(SmsTransactionMessageStatus smsTransactionMessageStatus, string otp, string referenceId);
    }
}
