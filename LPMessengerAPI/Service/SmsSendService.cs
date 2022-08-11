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
    public class SmsSendService : ISmsSendService
    {
        private readonly lpmessengerdbContext _context;
        public SmsSendService(lpmessengerdbContext context)
        {
            _context = context;
        }

        public async Task<SmsSend> PostSmsSend(SmsRequestDto smsRequestDto, int? groupId)
        {
            try
            {
                SmsSend smsSend = new SmsSend();

                smsSend.Phone = Int64.Parse(smsRequestDto.PhoneNumber);
                smsSend.Body = smsRequestDto.Message;
                smsSend.GroupId = groupId;

                var result = _context.SmsSends.Add(smsSend);

                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<SmsSend> UpdateSmsSend(int id, string otp, string refferenceId)
        {
            try
            {
                var data = await (from s in _context.SmsSends
                                  where s.Id == id
                                  select s).FirstOrDefaultAsync();
                if(data != null)
                {
                    string[] refferenceList = refferenceId.Split("|");

                    int refId = int.Parse(refferenceList[2]);
                    //int refId = int.Parse(" 34008");
                    data.Otp = otp;                    
                    data.RefferenceNumber = refId.ToString();

                    await _context.SaveChangesAsync();

                    return data;
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> GetSmsSend(string otp, string refferenceId)
        {
            try
            {
                var data = await (from s in _context.SmsSends
                                  where s.Otp == otp && s.RefferenceNumber == refferenceId
                                  select s).FirstOrDefaultAsync();

                if (data != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
