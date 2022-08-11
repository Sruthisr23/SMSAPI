using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LPMessengerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly lpmessengerdbContext _context;
        private readonly ISmsService _smsService;
        private readonly IExternalService _externalService;
        private readonly ISmsSendService _smsSendService;
        private readonly IUserService _userService;
        

        public SmsController(lpmessengerdbContext context, ISmsService smsService, ISmsSendService smsSendService,
            IExternalService externalService, IUserService userService)
        {
            _context = context;
            _smsService = smsService;
            _smsSendService = smsSendService;
            _externalService = externalService;
            _userService = userService;
        }

        [HttpPost] 
        public async Task<ActionResult<string[]>> PostSms(SmsRequestDto smsRequestDto, int userId, string token)
        {
            try
            {
                List<string> smsresponse = new List<string>();

                var data = await _userService.GetToken(userId);
                if (data != token)// false
                {
                    return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
                }
                string[] subs = smsRequestDto.PhoneNumber.Split(',');

                foreach (var mobile in subs)
                {
                    smsRequestDto.PhoneNumber = mobile;
                    var sendSms = await _smsSendService.PostSmsSend(smsRequestDto, null);
                    var msgUniqueId = await _smsService.RandomNumber();

                    if (sendSms != null)
                    {
                        var result = _externalService.PostSms(smsRequestDto, msgUniqueId.ToString()); //"success | 919787709943 | 34557"; //

                        smsresponse.Add(result.ToString());
                    }
                    else
                    {
                        smsresponse.Add("failed | "+smsRequestDto.PhoneNumber+" | " + msgUniqueId.ToString());                        
                    }
                }

                return Ok(smsresponse.ToArray());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ImportExcel")]
        public async Task<ActionResult> SendSmsViaExcel(IFormFile formFile, int userId, string token, int? groupId, string groupName, int? serviceId)
        {
            try
            {
                var data = await _userService.GetToken(userId);
                if (data != token)// false
                {
                    return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
                }

                await _externalService.SaveFileInFolder(formFile);

                ExcelFileUploadDto excelFileUploadDto = new ExcelFileUploadDto();
                excelFileUploadDto.GroupId = groupId;
                excelFileUploadDto.GroupName = groupName;
                excelFileUploadDto.FileName = formFile.FileName;
                excelFileUploadDto.serviceId = serviceId;

                await _smsService.ImportExcelFile(excelFileUploadDto);

                await _externalService.DeleteFileInFolder(formFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok();
        }

        [HttpPost("SendBulkSMS")]
        public async Task<ActionResult> SendGroupMessage(int userId, string token, int? groupId, int? templateId)
        {
            try
            {
                var msgUniqueId = await _smsService.RandomNumber();
                var data = await _userService.GetToken(userId);
                if (data != token)// false
                {
                    return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
                }

                var response = await _smsService.GetGroupmessage(groupId, templateId);

                foreach(var sms in response)
                {                    
                    var result = _externalService.PostSms(sms, msgUniqueId.ToString()); 

                    return Ok(result);                    
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok();
        }

        [HttpPost("GenerateOtpService")]
        public async Task<ActionResult<string[]>> PostOtpSms(SmsRequestDto smsRequestDto, int userId, string token,int? length, bool? alphaNumeric)
        {
            try
            {
                List<string> smsresponse = new List<string>();

                var data = await _userService.GetToken(userId);
                if (data != token)// false
                {
                    return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
                }
                string[] subs = smsRequestDto.PhoneNumber.Split(',');

                foreach (var mobile in subs)
                {
                    smsRequestDto.PhoneNumber = mobile;

                    var sendSms = await _smsSendService.PostSmsSend(smsRequestDto, null);
                    var msgUniqueId = await _smsService.RandomNumber();

                    if (sendSms != null)
                    {
                        var otp = "";
                        if (alphaNumeric == true)
                        {
                            otp = await _externalService.RandomAlphaNumeric(length);
                        }
                        else
                        {
                            otp = await _externalService.RandomNumberAsync(length);
                        }

                        smsRequestDto.Message = smsRequestDto.Message == "" ? "Please use this OTP Number: " + otp : smsRequestDto.Message + otp;

                        var result = _externalService.PostSms(smsRequestDto, msgUniqueId.ToString());

                        var updateStatus = await _smsSendService.UpdateSmsSend(sendSms.Id, "123456", result);//otp.ToString()

                        smsresponse.Add(result.ToString());
                        
                    }
                    else
                    {
                        smsresponse.Add("failed | " + smsRequestDto.PhoneNumber + " | " + msgUniqueId.ToString());
                    }
                }

                return Ok(smsresponse.ToArray());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ValidateOtpService")]
        public async Task<ActionResult<SmsSend>> ValidateOtpService(int userId, string token, string otp, string referenceId)
        {
            try
            {
                var data = await _userService.GetToken(userId);
                if (data != token)// false
                {
                    return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
                }

                var otpExist = await _smsSendService.GetSmsSend(otp, referenceId);

                if (otpExist == true)
                {
                    return Ok("true");
                }
                else
                {
                    return NotFound("InValid OTP");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //private async Task<SmsSend> RandomNumber(SmsRequestDto smsRequestDto, int? groupId, string groupName)
        //{
        //    SmsSend smsSend= new SmsSend();
        //    smsSend.Body = smsRequestDto.Message;
        //    smsSend.GroupId = groupId;
        //    smsSend.

        //    return smsSend;
        //}

        //[HttpPost("SendSMS")]
        //public async Task<ActionResult<MlSendmail>> PostSendSMS(int senderConfigMailId, int userId, string token, int? templateId)
        //{
        //    var tokenResponse = await _userService.GetToken(userId);
        //    if (tokenResponse != token)// false
        //    {
        //        return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
        //    }

        //    var mails = await _mailService.GetMailListViaGroupAndTemplate(groupId, templateId);
        //    var senderMail = await _senderConfigService.GetSenderConfigAsync(senderConfigMailId);
        //    foreach (var data in mails)
        //    {
        //        data.SenderEmail = senderMail.Email;
        //        await _mailerService.SendMail(data);
        //    }

        //    return Ok("All Mails send successfully");

        //}
    }
}
