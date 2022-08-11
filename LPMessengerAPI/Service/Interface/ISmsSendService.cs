using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service.Interface
{
    public interface ISmsSendService
    {
        Task<SmsSend> PostSmsSend(SmsRequestDto smsRequestDto, int? groupId);
        Task<SmsSend> UpdateSmsSend(int id, string otp, string refferenceId);
        Task<bool> GetSmsSend(string otp, string refferenceId);
    }
}
