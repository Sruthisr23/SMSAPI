using CsvHelper;
using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly lpmessengerdbContext _context;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IWhatsAppSendService _whatsAppSendService;
        public WhatsAppService(lpmessengerdbContext context,
            IOptions<AppSettings> appSettings, ISmsSendService smsSendService,
            IWhatsAppSendService whatsAppSendService)
        {
            _context = context;
            _appSettings = appSettings;
            _whatsAppSendService = whatsAppSendService;
        }

        //public async Task<SmsTransactionMessageStatus> SendWhatsApp(SmsRequestDto smsRequestDto)
        //{
        //    try
        //    {
        //        if (smsRequestDto.Phone != 0)
        //        {
        //            var urlForm = _appSettings.Value.SmsBaseUrl;
        //            HttpClientHandler handler = new HttpClientHandler();
        //            //handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        //            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(_appSettings.Value.SmsUserName + ":" + _appSettings.Value.SmsPassword));
        //            HttpClient client = new HttpClient(handler);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");

        //            try
        //            {
        //                HttpResponseMessage messge = client.GetAsync(urlForm).Result;
        //                var serverResponse = messge.Content.ReadAsStringAsync().Result;
        //                var response = JsonConvert.DeserializeObject<SmsTransactionMessageStatus>(serverResponse);
        //                return (response);
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return null;
        //}

        public async Task<string> ImportExcelFile(string fileName, int? SenderPhoneCofigId, int? chatTemplateId)
        {
            try
            {
                List<WhatsAppExcelUploadDto> records = new List<WhatsAppExcelUploadDto>();

                string baseLocation = _appSettings.Value.Location;

                using (var steram = new StreamReader(@baseLocation + fileName))
                {
                    using (var csvReader = new CsvReader(steram, System.Globalization.CultureInfo.InvariantCulture))
                    {
                        records = csvReader.GetRecords<WhatsAppExcelUploadDto>().ToList();
                    }

                }

                foreach (var data in records)
                {
                    //var maildetails = await (from m in _context.SmsSends
                    //                         where m.Phone == data.ReceiverPhone
                    //                         && m.GroupId != 0
                    //                         select m).FirstOrDefaultAsync();

                    if (null == null) //maildetails
                    {
                        WhatsappSend whatsAppRequest = new WhatsappSend();
                        whatsAppRequest.MediaUrl = data.MediaUrl;
                        whatsAppRequest.Message = data.Message;
                        whatsAppRequest.Message = data.MessageType;
                        whatsAppRequest.Phone = data.Phone;
                        whatsAppRequest.TemplateId = data.TemplateId;
                        whatsAppRequest.ChatTemplateId = chatTemplateId;
                        whatsAppRequest.SenderPhoneCofigId = SenderPhoneCofigId;


                        var sendmail = await _whatsAppSendService.PostWhatsAppBulkSend(whatsAppRequest);

                    }
                    //else
                    //{
                    //    return "sms exist for the same group";
                    //}
                }

                return "Sms uploaded successfully";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
