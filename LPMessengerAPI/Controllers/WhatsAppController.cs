using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace LPMessengerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController : ControllerBase
    {
        private readonly IExternalService _externalService;
        private readonly IWhatsAppSendService _whatsAppSendService;
        private readonly IWhatsAppService _whatsAppService;
        private readonly IUserService _userService;

        public WhatsAppController(IExternalService externalService, IWhatsAppSendService whatsAppSendService,
            IUserService userService, IWhatsAppService whatsAppService)
        {
            _whatsAppSendService = whatsAppSendService;
            _externalService = externalService;
            _userService = userService;
            _whatsAppService = whatsAppService;
        }

        [HttpPost]
        public async Task<ActionResult> PostWhatsApp(WhatsAppRequestDto whatsAppRequestDto, int userId, string token)
        {
            try
            {
                var data = await _userService.GetToken(userId);

                if (data != token)// false  
                {
                    return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
                }
                var phoneList = whatsAppRequestDto.Phone;

                string[] phoneNumbers = whatsAppRequestDto.Phone.Split(',');

                foreach (var phone in phoneNumbers)
                {
                    whatsAppRequestDto.Phone = phone;
                    var whatsapp = await _whatsAppSendService.PostWhatsAppSend(whatsAppRequestDto);
                }

                whatsAppRequestDto.Phone = phoneList;

                var result = _externalService.PostwhatsAppToken();

                _externalService.postWhatsApp(result, whatsAppRequestDto);               

                return Ok("success");                

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GenerateOtp")]
        public async Task<ActionResult> GenerateOtp(WhatsAppRequestDto whatsAppRequestDto, int userId, string token)
        {
            try
            {
                var data = await _userService.GetToken(userId);

                if (data != token)// false  
                {
                    return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
                }
                var phoneList = whatsAppRequestDto.Phone;

                string[] phoneNumbers = whatsAppRequestDto.Phone.Split(',');

                var otp = await _externalService.RandomNumber();

                foreach (var phone in phoneNumbers)
                {
                    whatsAppRequestDto.Phone = phone;
                    var whatsapp = await _whatsAppSendService.PostWhatsAppSend(whatsAppRequestDto);
                }

                whatsAppRequestDto.Phone = phoneList;

                var result = _externalService.PostwhatsAppToken();
                whatsAppRequestDto.Message = whatsAppRequestDto.Message==""? otp.ToString(): "Please use this OTP Number: "+ otp.ToString();
                var reponse = _externalService.postWhatsApp(result, whatsAppRequestDto);

                return Ok("success");


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ImportExcel")]
        public async Task<ActionResult> SendWhatsAppViaExcel(int userId, string token, int? senderConfigPhoneId, int? chatTemplateId, IFormFile formFile )
        {
            try
            {
                var data = await _userService.GetToken(userId);
                if (data != token)// false  
                {
                    return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
                }

                await _externalService.SaveFileInFolder(formFile);

                var res = await _whatsAppService.ImportExcelFile(formFile.FileName, senderConfigPhoneId, chatTemplateId);

                await _externalService.DeleteFileInFolder(formFile);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok();
        }

        [HttpPost("sendBulkMessages")]
        public async Task<ActionResult> SendWhatsAppGroupMessage(int userId, string token, int? chatTemplateId)
        {
            try
            {
                var data = await _userService.GetToken(userId);
                if (data != token)// false  
                {
                    return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
                }
                var response = await _whatsAppSendService.GetWhatsAppSendData(chatTemplateId);

                if (response != null)
                {
                    var result = _externalService.PostwhatsAppToken();

                    foreach (var detail in response)
                    {
                        var reponse = _externalService.postWhatsApp(result, detail);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
