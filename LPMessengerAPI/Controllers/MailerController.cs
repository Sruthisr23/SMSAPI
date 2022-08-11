using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LPMessengerAPI.Model;
using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Service.Interface;
using System.IO;
using CsvHelper;
using Microsoft.Extensions.Options;

namespace LPMessengerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailerController : ControllerBase
    {
        private readonly IMailerService _mailerService;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;
        private readonly ISenderConfigService _senderConfigService;
        private readonly IExternalService _externalService;

        public MailerController(IMailerService mailerService,
            IMailService mailService, ISenderConfigService senderConfigService,
            IUserService userService, IExternalService externalService)
        {
            _mailerService = mailerService;
            _mailService = mailService;
            _senderConfigService = senderConfigService;
            _userService = userService;
            _externalService = externalService;
        }       

        [HttpPost]
        public async Task<ActionResult<string[]>> SendMail(MailRequestDto mailRequestDto)
        {
            try
            {
                var tokenResponse = await _userService.GetToken(mailRequestDto.userId);
                if (tokenResponse != mailRequestDto.token)// false
                {
                    return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
                }

                string[] subs = mailRequestDto.ReceiverEmail.Split(',');

                List<string> mailResponse = new List<string>();

                foreach (var mail in subs)
                {
                    var referenceId = await _externalService.RandomNumber();

                    //mailRequestDto.ReceiverEmail = mail;
                    MailDto mailDto = new MailDto();
                    mailDto.Body = mailRequestDto.Body;
                    mailDto.IsBodyHtml = mailRequestDto.IsBodyHtml;
                    mailDto.ReceiverEmail = mail;
                    mailDto.SenderEmail = mailRequestDto.SenderEmail;
                    mailDto.SenderName = mailRequestDto.SenderName;
                    mailDto.Subject = mailRequestDto.Subject;
                    var sendmail = await _mailerService.SaveMail(mailDto, referenceId.ToString());

                    if (sendmail == null)
                    {
                        mailResponse.Add("failed | " + sendmail.ReceiverEmail + " | " + referenceId);                        
                    }
                    else
                    {
                        await _mailerService.SendMail(sendmail, null, mailRequestDto.attachmentPaths);
                        mailResponse.Add("success | " + sendmail.ReceiverEmail + " | " + referenceId);
                    } 

                }

                return Ok(mailResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("Send")]
        public async Task<ActionResult<string[]>> SendMailData(MailRequestDto mailRequestDto)
        {
            try
            {
                var tokenResponse = await _userService.GetToken(mailRequestDto.userId);
                if (tokenResponse != mailRequestDto.token)// false
                {
                    return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
                }

                string[] subs = mailRequestDto.ReceiverEmail.Split(',');

                List<string> mailResponse = new List<string>();

                foreach (var mail in subs)
                {
                    var referenceId = await _externalService.RandomNumber();

                    //mailRequestDto.ReceiverEmail = mail;
                    MailDto mailDto = new MailDto();
                    mailDto.Body = mailRequestDto.Body;
                    mailDto.IsBodyHtml = mailRequestDto.IsBodyHtml;
                    mailDto.ReceiverEmail = mail;
                    mailDto.SenderEmail = mailRequestDto.SenderEmail;
                    mailDto.SenderName = mailRequestDto.SenderName;
                    mailDto.Subject = mailRequestDto.Subject;
                    var sendmail = await _mailerService.SaveMail(mailDto, referenceId.ToString());

                    if (sendmail == null)
                    {
                        mailResponse.Add("failed | " + sendmail.ReceiverEmail + " | " + referenceId);
                    }
                    else
                    {
                        await _mailerService.SendMail(sendmail, null, mailRequestDto.attachmentPaths);
                        mailResponse.Add("success | " + sendmail.ReceiverEmail + " | " + referenceId);
                    }

                }

                return Ok(mailResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("generateOtp")]
        public async Task<ActionResult<string[]>> SendMailOtp(MailDto mlMail, int userId, string token, int? length, bool? alphaNumeric)
        {
            try
            {
                var tokenResponse = await _userService.GetToken(userId);
                if (tokenResponse != token)// false
                {
                    return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
                }
                string[] subs = mlMail.ReceiverEmail.Split(',');

                List<string> mailResponse = new List<string>();

                foreach (var mail in subs)
                {
                    mlMail.ReceiverEmail = mail;

                    var mlMailData = await MapMail(mlMail, length, alphaNumeric);

                    var sendmail = await _mailService.PostMail(mlMailData);

                    if (sendmail == null)
                    {
                        mailResponse.Add("failed | " + sendmail.ReceiverEmail + " | " + mlMailData.ReferenceId);                        
                    }
                    else
                    {
                        await _mailerService.SendMail(sendmail,null,null);

                        mailResponse.Add("success | " + sendmail.ReceiverEmail + " | " + mlMailData.ReferenceId);
                    }
                    
                }
                return Ok(mailResponse.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("ValidateOtpService")]
        public async Task<ActionResult<string>> ValidateOtpService(int userId, string token, string otp, string referenceId)
        {
            try
            {
                var data = await _userService.GetToken(userId);
                if (data != token)// false
                {
                    return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
                }

                var otpExist = await _mailService.ValidateOtp(otp, referenceId);

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

        [HttpPost("ImportExcel")]
        public async Task<ActionResult<MlMail>> SendMailViaExcel(IFormFile formFile, int userId, string token, int? groupId, string groupName, int? serviceId)
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
                //excelFileUploadDto.FileName = excelFileUploadDto.formFile.FileName + ".csv";
                await _mailerService.ImportExcelFile(excelFileUploadDto);

                await _externalService.DeleteFileInFolder(formFile);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return Ok();
        }       

        [HttpPost("SendMail")]
        public async Task<ActionResult> PostSendMail(int userId, string token, int? groupId, int? templateId)
        {
            var tokenResponse = await _userService.GetToken(userId);
            if (tokenResponse != token)// false
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            var mails = await _mailService.GetMailListViaGroupAndTemplate(groupId, templateId);
            //var senderMail = await _senderConfigService.GetSenderConfigAsync(senderConfigMailId);
            foreach (var data in mails)
            {
                //data.SenderEmail = senderMail.Email;
                await _mailerService.SendMail(data,null,null);
            }
            
            return Ok();

        }

        private async Task<MlMail> MapMail(MailDto mlMailDto, int? length, bool? alphaNumeric)
        {
            var refId = await _externalService.RandomNumber();
            //var otp = await _externalService.RandomNumber();
            var otpData = ""; 
            if(alphaNumeric == true)
            {
                otpData = await _externalService.RandomAlphaNumeric(length);
            }
            else
            {
                otpData = await _externalService.RandomNumberAsync(length);
            }
            

            MlMail mlMail = new MlMail();

            mlMail.ReferenceId = refId.ToString();
            mlMail.Otp = otpData.ToString();

            mlMail.IsBodyHtml = mlMailDto.IsBodyHtml;
            mlMail.Body = mlMailDto.Body == "" ? "Please use this OTP Number: " + otpData : mlMailDto.Body + otpData;
            mlMail.ReceiverEmail = mlMailDto.ReceiverEmail;
            mlMail.SenderEmail = mlMailDto.SenderEmail;
            mlMail.SenderName = mlMailDto.SenderName;
            mlMail.Subject = mlMailDto.Subject;

            return mlMail;

        }

    }
}

