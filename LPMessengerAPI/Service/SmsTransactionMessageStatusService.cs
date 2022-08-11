using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service
{
    public class SmsTransactionMessageStatusService : ISmsTransactionMessageStatusService
    {
        private readonly lpmessengerdbContext _context;
        public SmsTransactionMessageStatusService(lpmessengerdbContext context)
        {
            _context = context;
        }

        public async Task<SmsTransactionMessageStatus> PostSmsTransactionMessageStatus(SmsRequestDto smsRequestDto, string userId, string otp)
        {
            try
            {
                SmsTransactionMessageStatus smsTransactionMessageStatus = new SmsTransactionMessageStatus();

                smsTransactionMessageStatus.PhoneNumber = smsRequestDto.PhoneNumber.ToString();
                smsTransactionMessageStatus.MessageDlrStatus = 0;
                smsTransactionMessageStatus.MessageText = smsRequestDto.Message;
                smsTransactionMessageStatus.SendType = 1;
                smsTransactionMessageStatus.UserId = userId;
                smsTransactionMessageStatus.ServiceChannel = 1;
                smsTransactionMessageStatus.Otp = otp;

                var result = _context.SmsTransactionMessageStatuses.Add(smsTransactionMessageStatus);

                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SmsTransactionMessageStatus> UpdateSmsTransactionMessageStatus(SmsTransactionMessageStatus smsTransactionMessageStatus, string otp,string referenceId)
        {
            try
            {                                
                smsTransactionMessageStatus.Otp = otp;
                smsTransactionMessageStatus.RefferenceNumber = referenceId;

                //var result = _context.SmsTransactionMessageStatuses.Add(smsTransactionMessageStatus);

                await _context.SaveChangesAsync();

                return smsTransactionMessageStatus ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SmsTransactionMessageStatus> GetSmsTransactionMessageStatus(SmsTransactionMessageStatus smsTransactionMessageStatus, string otp, string referenceId)
        {
            try
            {
                smsTransactionMessageStatus.Otp = otp;
                smsTransactionMessageStatus.RefferenceNumber = referenceId;

                //var result = _context.SmsTransactionMessageStatuses.Add(smsTransactionMessageStatus);

                await _context.SaveChangesAsync();

                return smsTransactionMessageStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
